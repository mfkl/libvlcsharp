//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solutionName = "LibVLCSharp";
var solutionFile = IsRunningOnWindows() ? $"{solutionName}.sln" : $"{solutionName}.Mac.sln";
var solutionPath = $"../src/{solutionFile}";
var libvlcsharpCsproj = "../src/libvlcsharp/libvlcsharp.csproj";
var testCsproj = "../src/LibVLCSharp.Tests/LibVLCSharp.Tests.csproj";

var packagesDir = "../packages";
var isCiBuild = BuildSystem.IsRunningOnAzurePipelines || BuildSystem.IsRunningOnAzurePipelinesHosted;
var suffixVersion = $"alpha-{DateTime.Today.ToString("yyyyMMdd")}-{BuildSystem.AzurePipelines.Environment.Build.Id}";
var feedzLVSSource = "https://f.feedz.io/videolan/preview/nuget/index.json";
var FEEDZ = "FEEDZ";
const uint totalPackageCount = 9;

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var artifactsDir = Directory("../nugets");
var artifactRelativePathPattern = "./../nugets/*";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDir);
    if(DirectoryExists(packagesDir))
    {
        DeleteDirectory(packagesDir, new DeleteDirectorySettings 
        {
            Recursive = true,
        });
    }
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath);
    MoveDirectory("../src/packages", packagesDir);
});

Task("BuildNet6")
    .Does(() =>
{
    DotNetCoreBuild(libvlcsharpCsproj, GetNetCoreBuildSettings());
});


Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(solutionPath, GetBuildSettings());
});

// just for (faster) testing
Task("Build-only-libvlcsharp")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(libvlcsharpCsproj, GetBuildSettings());
});

Task("Test")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Loggers = new []{ "console;verbosity=detailed" }
    };

    DotNetCoreTest(testCsproj, settings);
});

Task("CIDeploy")
    .Does(() =>
{
    var packages = GetFiles(artifactRelativePathPattern);
    
    Information($"packages count: {packages.Count}");

    if(packages.Count != totalPackageCount)
    {
        throw new Exception($"There should be {totalPackageCount} packages but there is {packages.Count} packages");
    }

    NuGetPush(packages, new NuGetPushSettings 
    {
        Source = feedzLVSSource,
        ApiKey = EnvironmentVariable(FEEDZ)
    });
});

MSBuildSettings GetBuildSettings()
{
    var settings = new MSBuildSettings();
    settings.SetConfiguration(configuration)
            .WithProperty("PackageOutputPath", MakeAbsolute(artifactsDir).FullPath);

    if(isCiBuild)
    {
        settings.WithProperty("VersionSuffix", suffixVersion);
    }
    return settings;
}

DotNetCoreBuildSettings GetNetCoreBuildSettings()
{
    var settings = new DotNetCoreMSBuildSettings();
    settings.WithProperty("Net6", "true");
    settings.SetConfiguration(configuration)
            .WithProperty("PackageOutputPath", MakeAbsolute(artifactsDir).FullPath);
    
    if(isCiBuild)
    {
        settings.WithProperty("VersionSuffix", suffixVersion);
    }

    var netCoreSettings = new DotNetCoreBuildSettings
    {
        MSBuildSettings = settings
    };

    return netCoreSettings;
}

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
