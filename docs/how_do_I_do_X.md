# How Do I do X?

This document contains a bunch of Q&A from users that asked how to do a specific operation with LibVLC or LibVLCSharp.

As a general rule, it's a good idea to check how to perform a given operation on official VLC app (iOS, Android, Windows, etc.). From there, logs can give you information, and looking at the code as well.

[Back](home.md)

## How do I play a DVD?

Create your media with `dvd:///` as per the [documentation](https://wiki.videolan.org/VLC_command-line_help/).

## How do I cast my media to my Chromecast?

Check out our [Chromecast sample](https://code.videolan.org/mfkl/libvlcsharp-samples/tree/master/Chromecast)

## How do I browse and play media on my local network (NAS/UPnP)?

Check out our [LocalNetwork sample](https://code.videolan.org/mfkl/libvlcsharp-samples/tree/master/LocalNetwork)

## How do I record a media to the file system?

Check out our [RecordHLS sample](https://code.videolan.org/mfkl/libvlcsharp-samples/tree/master/RecordHLS)

## How do I create a mosaic of views for videos?

Check out our [VideoMosaic sample](https://code.videolan.org/mfkl/libvlcsharp-samples/tree/master/VideoMosaic)

## How do I navigate a 360 video and change viewpoint?

Check out our [Gestures sample](https://code.videolan.org/mfkl/libvlcsharp-samples/tree/master/Gestures)

## How do I get codec information about a media?

Inspect the Video and Audio tracks objects with `Media.Tracks`.

## How do I get libvlc logs?

Subscribe to the `LibVLC.Log` event.

## How do I play media with RTSP or HLS protocols?

Like any other media. [Here](https://www.videolan.org/vlc/features.html) is a non exhaustive list of protocols we support.

## How do I get individual frames out of a video?

You may want to have a look at the WPF control of Vlc.DotNet which does exactly that: https://github.com/ZeBobo5/Vlc.DotNet/blob/develop/src/Vlc.DotNet.Wpf/VlcVideoSourceProvider.cs

From libvlc, that means libvlc_video_set_callbacks and libvlc_video_set_format_callbacks.

## How do I do transcoding?

Pretty similarly to how you would do it from the CLI. Read https://wiki.videolan.org/Transcode/ and try media options with `Media.AddOption`.

## How do I enable/disable hardware decoding?

```csharp
MediaPlayer.EnableHardwareDecoding = true
```

## How do I play a YouTube video?

Like this, for example:

```csharp
Core.Initialize();

using(var libVLC = new LibVLC())
{
    var media = new Media(libVLC, "https://www.youtube.com/watch?v=dQw4w9WgXcQ", FromType.FromLocation);
    await media.Parse(MediaParseOptions.ParseNetwork);
    using (var mp = new MediaPlayer(media.SubItems.First()))
    {
            var r = mp.Play();
            Console.ReadKey();
    }
}
```

# Subtitles

Full list commands and arguments https://wiki.videolan.org/VLC_command-line_help/

## How do I set subtitles?

```csharp
Core.Initialize();

using(var libVLC = new LibVLC())
{
    var media = new Media(_libVLC, "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", FromType.FromLocation);
    using (var mp = new MediaPlayer(media))
    {
            mp.AddSlave(MediaSlaveType.Subtitle, "file:///C:\\Users\\Me\\Desktop\\subs.srt", true);
            var r = mp.Play();
            Console.ReadKey();
    }
}
```

## How do I change the subtitle font size?

```csharp
media.AddOption(":freetype-rel-fontsize=10"); // Usually 10, 13, 16 or 19
```

## How do I change the subtitle color?

```csharp
media.AddOption(":freetype-color=16711680"); // Red
```

## How do I change the subtitle encoding?

```csharp
media.AddOption(":subsdec-encoding=Windows-125");
```

# How do I change aspect ratio?

If you are using the `MediaPlayerElement`, then it is built-in in the UI.

If not, you should play with the `Scale` and `AspectRatio` MediaPlayer properties.

See https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/src/LibVLCSharp/MediaPlayerElement/AspectRatioManager.cs

# How do I change the movie play rate?

```csharp
MediaPlayer.SetRate(float rate)
```

# How do I change deblocking filter settings?

```csharp
new LibVLC("--avcodec-skiploopfilter=2")
```
```
 --avcodec-skiploopfilter={0 (None), 1 (Non-ref), 2 (Bidir), 3 (Non-key), 4 (All)} 
                                 Skip the loop filter for H.264 decoding
          Skipping the loop filter (aka deblocking) usually has a detrimental
          effect on quality. However it provides a big speedup for high
          definition streams.
```
# How do I take a snapshot of the video?

```csharp
MediaPlayer.TakeSnapshot(uint num, string? filePath, uint width, uint height)
```

# How do I enable loopback playback?

```csharp
new LibVLC("--input-repeat=2")
```
https://stackoverflow.com/questions/56487740/how-to-achieve-looping-playback-with-libvlcsharp

# How do I retrieve Video/Audio/Subtitle track information?

```csharp
using var libVLC = new LibVLC();
using var media = new Media(libVLC, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"));

await media.Parse(MediaParseOptions.ParseNetwork);

foreach(var track in media.Tracks)
{
    switch(track.TrackType)
    {
        case TrackType.Audio:
            Debug.WriteLine("Audio track");
            Debug.WriteLine($"{nameof(track.Data.Audio.Channels)}: {track.Data.Audio.Channels}");
            Debug.WriteLine($"{nameof(track.Data.Audio.Rate)}: {track.Data.Audio.Rate}");
            break;
        case TrackType.Video:
            Debug.WriteLine("Video track");
            Debug.WriteLine($"{nameof(track.Data.Video.FrameRateNum)}: {track.Data.Video.FrameRateNum}");
            Debug.WriteLine($"{nameof(track.Data.Video.FrameRateDen)}: {track.Data.Video.FrameRateDen}");
            Debug.WriteLine($"{nameof(track.Data.Video.Height)}: {track.Data.Video.Height}");
            Debug.WriteLine($"{nameof(track.Data.Video.Width)}: {track.Data.Video.Width}");
            break;
    }
}
```