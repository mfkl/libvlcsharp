using System.Reflection;
using BenchmarkDotNet.Running;

namespace LibVLCSharp.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            new BenchmarkSwitcher(typeof(Program).GetTypeInfo().Assembly).Run(args);
        }
    }
}
