using System;
using System.Runtime.InteropServices;

namespace LibVLCSharp.Shared
{
    /// <summary>LibVLCEvent types</summary>
    public enum EventType
    {
        /// <summary>
        /// Metadata of a media item changed.
        /// </summary>
        MediaMetaChanged = 0,

        /// <summary>
        /// Subitem was added to a media item. 
        /// </summary>
        MediaSubItemAdded = 1,

        /// <summary>
        /// Duration of a media item changed.
        /// </summary>
        MediaDurationChanged = 2,

        /// <summary>
        /// Parsing state of a media item changed.
        /// </summary>
        MediaParsedChanged = 3,

        /// <summary>
        /// A media item was freed.
        /// </summary>
        MediaFreed = 4,

        /// <summary>
        /// State of a media item changed
        /// </summary>
        MediaStateChanged = 5,

        /// <summary>
        /// Subitem tree was added to a media item.
        /// </summary>
        MediaSubItemTreeAdded = 6,

        /// <summary>
        /// Media changed in the media player.
        /// </summary>
        MediaPlayerMediaChanged = 256,

        /// <summary>
        /// Nothing special happening.
        /// </summary>
        MediaPlayerNothingSpecial = 257,

        /// <summary>
        /// Mediaplayer is opening a media
        /// </summary>
        MediaPlayerOpening = 258,

        /// <summary>
        /// Mediaplayer is buffering a media
        /// </summary>
        MediaPlayerBuffering = 259,

        /// <summary>
        /// Mediaplayer is playing a media
        /// </summary>
        MediaPlayerPlaying = 260,

        /// <summary>
        /// Mediaplayer is paused
        /// </summary>
        MediaPlayerPaused = 261,

        /// <summary>
        /// Mediaplayer is stopped
        /// </summary>
        MediaPlayerStopped = 262,

        /// <summary>
        /// Mediaplayer is seeking forward
        /// </summary>
        MediaPlayerForward = 263,

        /// <summary>
        /// Mediaplayer is rewinding backward
        /// </summary>
        MediaPlayerBackward = 264,

        /// <summary>
        /// Mediaplayer playback end reached
        /// </summary>
        MediaPlayerEndReached = 265,

        /// <summary>
        /// Mediaplayer encountered an error
        /// </summary>
        MediaPlayerEncounteredError = 266,

        /// <summary>
        /// Mediaplayer time changed
        /// </summary>
        MediaPlayerTimeChanged = 267,

        /// <summary>
        /// Mediaplayer position changed
        /// </summary>
        MediaPlayerPositionChanged = 268,

        /// <summary>
        /// Mediaplayer seekable capability changed
        /// </summary>
        MediaPlayerSeekableChanged = 269,

        /// <summary>
        /// Mediaplayer pausable capability changed
        /// </summary>
        MediaPlayerPausableChanged = 270,

        /// <summary>
        /// Mediaplayer media title changed
        /// </summary>
        MediaPlayerTitleChanged = 271,

        /// <summary>
        /// Mediaplayer took a snapshot
        /// </summary>
        MediaPlayerSnapshotTaken = 272,

        /// <summary>
        /// Media length changed in mediaplayer
        /// </summary>
        MediaPlayerLengthChanged = 273,

        /// <summary>
        /// Mediaplayer has a new vout
        /// </summary>
        MediaPlayerVout = 274,

        /// <summary>
        /// Mediaplayer has a new scrambled state
        /// </summary>
        MediaPlayerScrambledChanged = 275,

        /// <summary>
        /// Mediaplayer has a new Elementary Stream (ES)
        /// </summary>
        MediaPlayerESAdded = 276,

        /// <summary>
        /// Mediaplayer has one less Elementary Stream (ES)
        /// </summary>
        MediaPlayerESDeleted = 277,

        /// <summary>
        /// Mediaplayer has selected a Elementary Stream (ES)
        /// </summary>
        MediaPlayerESSelected = 278,

        /// <summary>
        /// The playback is paused automatically for a higher priority audio stream
        /// </summary>
        MediaPlayerCorked = 279,

        /// <summary>
        /// The playback is unpaused automatically after a higher priority audio stream ends
        /// </summary>
        MediaPlayerUncorked = 280,

        /// <summary>
        /// The audio of the mediaplayer is muted
        /// </summary>
        MediaPlayerMuted = 281,

        /// <summary>
        /// The audio of the mediaplayer is unmuted
        /// </summary>
        MediaPlayerUnmuted = 282,

        /// <summary>
        /// The current audio volume changes
        /// </summary>
        MediaPlayerAudioVolume = 283,

        /// <summary>
        /// The current audio output device changes
        /// </summary>
        MediaPlayerAudioDevice = 284,

        /// <summary>
        /// The current chapter changes
        /// </summary>
        MediaPlayerChapterChanged = 285,

        /// <summary>
        /// A media item was added to a media list.
        /// </summary>
        MediaListItemAdded = 512,

        /// <summary>
        /// A media item is about to get added to a media list.
        /// </summary>
        MediaListWillAddItem = 513,

        /// <summary>
        /// A media item was deleted from a media list.
        /// </summary>
        MediaListItemDeleted = 514,

        /// <summary>
        /// A media item is about to get deleted from a media list.
        /// </summary>
        MediaListWillDeleteItem = 515,

        /// <summary>
        /// A media list has reached the end.
        /// All items were either added (in case of a libvlc_media_discoverer_t) or parsed (preparser)
        /// </summary>
        MediaListEndReached = 516,

        /// <summary>
        /// Playback of a media list player has started.
        /// </summary>
        MediaListPlayerPlayed = 1024,

        /// <summary>
        /// The current item of a media list player has changed to a different item.
        /// </summary>
        MediaListPlayerNextItemSet = 1025,

        /// <summary>
        /// Playback of a media list player has stopped.
        /// </summary>
        MediaListPlayerStopped = 1026,

        /// <summary>
        /// Useless event, it will be triggered only when calling libvlc_media_discoverer_start()
        /// </summary>
        MediaDiscovererStarted = 1280,

        /// <summary>
        /// Useless event, it will be triggered only when calling libvlc_media_discoverer_stop()
        /// </summary>
        MediaDiscovererStopped = 1281,

        /// <summary>
        /// A new renderer item was found by a renderer discoverer.
        /// The renderer item is valid until deleted. 
        /// </summary>
        RendererDiscovererItemAdded = 1282,

        /// <summary>
        /// A previously discovered renderer item was deleted by a renderer discoverer.
        /// The renderer item is no longer valid. 
        /// </summary>
        RendererDiscovererItemDeleted = 1283,
    }
    
    /// <summary>
    /// A LibVLC event
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LibVLCEvent
    {
        /// <summary>
        /// Type of the event
        /// </summary>
        public EventType Type;

        /// <summary>
        /// Native reference to the sender
        /// </summary>
        public IntPtr Sender;

        /// <summary>
        /// Native reference to a RendererItem
        /// </summary>

        /// <summary>
        /// Event union
        /// </summary>
        public EventUnion Union;

        /// <summary>
        /// Union definition of all event types
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct EventUnion
        {
            /// <summary>
            /// Metadata of a media item changed.
            /// </summary>
            [FieldOffset(0)]
            public MediaMetaChanged MediaMetaChanged;

            /// <summary>
            /// Subitem was added to a media item. 
            /// </summary>
            [FieldOffset(0)]
            public MediaSubItemAdded MediaSubItemAdded;

            /// <summary>
            /// Duration of a media item changed.
            /// </summary>
            [FieldOffset(0)]
            public MediaDurationChanged MediaDurationChanged;

            /// <summary>
            /// Parsing state of a media item changed.
            /// </summary>
            [FieldOffset(0)]
            public MediaParsedChanged MediaParsedChanged;

            /// <summary>
            /// A media item was freed.
            /// </summary>
            [FieldOffset(0)]
            public MediaFreed MediaFreed;

            /// <summary>
            /// State of a media item changed
            /// </summary>
            [FieldOffset(0)]
            public MediaStateChanged MediaStateChanged;

            /// <summary>
            /// Subitem tree was added to a media item.
            /// </summary>
            [FieldOffset(0)]
            public MediaSubItemTreeAdded MediaSubItemTreeAdded;

            /// <summary>
            /// Mediaplayer is buffering a media
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerBuffering MediaPlayerBuffering;

            /// <summary>
            /// The current chapter changes
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerChapterChanged MediaPlayerChapterChanged;

            /// <summary>
            /// Mediaplayer position changed
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerPositionChanged MediaPlayerPositionChanged;

            /// <summary>
            /// Mediaplayer time changed
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerTimeChanged MediaPlayerTimeChanged;

            /// <summary>
            /// MediaPlayer title changed
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerTitleChanged MediaPlayerTitleChanged;

            /// <summary>
            /// Mediaplayer seekable changed
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerSeekableChanged MediaPlayerSeekableChanged;

            /// <summary>
            /// Mediaplayer pausable changed
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerPausableChanged MediaPlayerPausableChanged;

            /// <summary>
            /// Mediaplayer scrambled changed
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerScrambledChanged MediaPlayerScrambledChanged;

            /// <summary>
            /// Mediaplayer vout changed
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerVoutChanged MediaPlayerVoutChanged;

            // medialist
            [FieldOffset(0)]
            public MediaListItemAdded MediaListItemAdded;
            [FieldOffset(0)]
            public MediaListWillAddItem MediaListWillAddItem;
            [FieldOffset(0)]
            public MediaListItemDeleted MediaListItemDeleted;
            [FieldOffset(0)]
            public MediaListWillDeleteItem MediaListWillDeleteItem;
            [FieldOffset(0)]
            public MediaListPlayerNextItemSet MediaListPlayerNextItemSet;
            // mediaplayer
            [FieldOffset(0)]
            public MediaPlayerSnapshotTaken MediaPlayerSnapshotTaken;

            /// <summary>
            /// Mediaplayer length changed
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerLengthChanged MediaPlayerLengthChanged;

            /// <summary>
            /// Media changed in the media player.
            /// </summary>
            [FieldOffset(0)]
            public MediaPlayerMediaChanged MediaPlayerMediaChanged;

            /// <summary>
            /// Mediaplayer has a new Elementary Stream (ES)
            /// </summary>
            [FieldOffset(0)]
            public EsChanged EsChanged;

            /// <summary>
            /// The current audio volume changes
            /// </summary>
            [FieldOffset(0)]
            public VolumeChanged MediaPlayerVolumeChanged;

            /// <summary>
            /// The current audio output device changes
            /// </summary>
            [FieldOffset(0)]
            public AudioDeviceChanged AudioDeviceChanged;

            /// <summary>
            /// A media item is about to get deleted from a media list
            /// </summary>
            [FieldOffset(0)]
            public RendererDiscovererItemAdded RendererDiscovererItemAdded;

            /// <summary>
            /// The current item of a media list player has changed to a different item
            /// </summary>
            [FieldOffset(0)]
            public RendererDiscovererItemDeleted RendererDiscovererItemDeleted; 

            
        }

        #region Media
        [StructLayout(LayoutKind.Sequential)]
        public struct MediaMetaChanged
        {
            public Media.MetadataType MetaType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaSubItemAdded
        {
            public IntPtr NewChild;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaDurationChanged
        {
            public long NewDuration;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaParsedChanged
        {
            public Media.MediaParsedStatus NewStatus;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaFreed
        {
            public IntPtr MediaInstance;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaStateChanged
        {
            public VLCState NewState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaSubItemTreeAdded
        {
            public IntPtr MediaInstance;
        }

        #endregion

        #region MediaPlayer 

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerBuffering
        {
            public float NewCache;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerChapterChanged
        {
            public int NewChapter;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerPositionChanged
        {
            public float NewPosition;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerTimeChanged
        {
            public long NewTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerTitleChanged
        {
            public int NewTitle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerSeekableChanged
        {
            public int NewSeekable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerPausableChanged
        {
            public int NewPausable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerScrambledChanged
        {
            public int NewScrambled;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerVoutChanged
        {
            public int NewCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerSnapshotTaken
        {
            public IntPtr Filename;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerLengthChanged
        {
            public long NewLength;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EsChanged
        {
            public TrackType Type;
            public int Id;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AudioDeviceChanged
        {
            public IntPtr Device;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerMediaChanged
        {
            public IntPtr NewMedia;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VolumeChanged
        {
            public float Volume;
        }

        #endregion

        #region MediaList

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListItemAdded
        {
            public IntPtr MediaInstance;
            public int Index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListWillAddItem
        {
            public IntPtr MediaInstance;
            public int Index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListItemDeleted
        {
            public IntPtr MediaInstance;
            public int Index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListWillDeleteItem
        {
            public IntPtr MediaInstance;
            public int Index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListPlayerNextItemSet
        {
            public IntPtr MediaInstance;
        }

        #endregion MediaList
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RendererDiscovererItemAdded
        {
            public IntPtr item;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RendererDiscovererItemDeleted
        {
            public IntPtr item;
    }

    #region Media events

    public class MediaMetaChangedEventArgs : EventArgs
    {
        public readonly Media.MetadataType MetadataType;

        public MediaMetaChangedEventArgs(Media.MetadataType metadataType)
        {
            MetadataType = metadataType;
        }
    }

    public class MediaParsedChangedEventArgs : EventArgs
    {
        public readonly Media.MediaParsedStatus ParsedStatus;

        public MediaParsedChangedEventArgs(Media.MediaParsedStatus parsedStatus)
        {
            ParsedStatus = parsedStatus;
        }
    }

    public class MediaSubItemAddedEventArgs : EventArgs
    {
        public readonly Media SubItem;

        public MediaSubItemAddedEventArgs(IntPtr mediaPtr)
        {
            SubItem = new Media(mediaPtr);
        }
    }

    public class MediaDurationChangedEventArgs : EventArgs
    {
        public readonly long Duration;

        public MediaDurationChangedEventArgs(long duration)
        {
            Duration = duration;
        }
    }

    public class MediaFreedEventArgs : EventArgs
    {
        public readonly Media Media;

        public MediaFreedEventArgs(IntPtr mediaPtr)
        {
            Media = new Media(mediaPtr);
        }
    }

    public class MediaStateChangedEventArgs : EventArgs
    {
        public readonly VLCState State;

        public MediaStateChangedEventArgs(VLCState state)
        {
            State = state;
        }
    }

    public class MediaSubItemTreeAddedEventArgs : EventArgs
    {
        public readonly Media SubItem;

        public MediaSubItemTreeAddedEventArgs(IntPtr subItemPtr)
        {
            SubItem = new Media(subItemPtr);
        }
    }

    #endregion

    #region MediaPlayer events

    public class MediaPlayerMediaChangedEventArgs : EventArgs
    {
        public readonly Media Media;

        public MediaPlayerMediaChangedEventArgs(IntPtr mediaPtr)
        {
            Media = new Media(mediaPtr);
        }
    }

    public class MediaPlayerBufferingEventArgs : EventArgs
    {
        public readonly float Cache;

        public MediaPlayerBufferingEventArgs(float cache)
        {
            Cache = cache;
        }
    }

    public class MediaPlayerTimeChangedEventArgs : EventArgs
    {
        public readonly long Time;

        public MediaPlayerTimeChangedEventArgs(long time)
        {
            Time = time;
        }
    }

    public class MediaPlayerPositionChangedEventArgs : EventArgs
    {
        public readonly float Position;

        public MediaPlayerPositionChangedEventArgs(float position)
        {
            Position = position;
        }
    }

    public class MediaPlayerSeekableChangedEventArgs : EventArgs
    {
        public readonly int Seekable;

        public MediaPlayerSeekableChangedEventArgs(int seekable)
        {
            Seekable = seekable;
        }
    }

    public class MediaPlayerPausableChangedEventArgs : EventArgs
    {
        public readonly int Pausable;

        public MediaPlayerPausableChangedEventArgs(int pausable)
        {
            Pausable = pausable;
        }
    }

    public class MediaPlayerTitleChangedEventArgs : EventArgs
    {
        public readonly int Title;

        public MediaPlayerTitleChangedEventArgs(int title)
        {
            Title = title;
        }
    }

    public class MediaPlayerChapterChangedEventArgs : EventArgs
    {
        public readonly int Chapter;

        public MediaPlayerChapterChangedEventArgs(int chapter)
        {
            Chapter = chapter;
        }
    }

    public class MediaPlayerSnapshotTakenEventArgs : EventArgs
    {
        public readonly string Filename;

        public MediaPlayerSnapshotTakenEventArgs(string filename)
        {
            Filename = filename;
        }
    }

    public class MediaPlayerLengthChangedEventArgs : EventArgs
    {
        public readonly long Length;

        public MediaPlayerLengthChangedEventArgs(long length)
        {
            Length = length;
        }
    }

    public class MediaPlayerVoutEventArgs : EventArgs
    {
        public readonly int Count;

        public MediaPlayerVoutEventArgs(int count)
        {
            Count = count;
        }
    }

    public class MediaPlayerScrambledChangedEventArgs : EventArgs
    {
        public readonly int Scrambled;

        public MediaPlayerScrambledChangedEventArgs(int scrambled)
        {
            Scrambled = scrambled;
        }
    }

    public class MediaPlayerESAddedEventArgs : EventArgs
    {
        public readonly int Id;

        public MediaPlayerESAddedEventArgs(int id)
        {
            Id = id;
        }
    }

    public class MediaPlayerESDeletedEventArgs : EventArgs
    {
        public readonly int Id;

        public MediaPlayerESDeletedEventArgs(int id)
        {
            Id = id;
        }
    }

    public class MediaPlayerESSelectedEventArgs : EventArgs
    {
        public readonly int Id;

        public MediaPlayerESSelectedEventArgs(int id)
        {
            Id = id;
        }
    }

    public class MediaPlayerAudioDeviceEventArgs : EventArgs
    {
        public readonly string AudioDevice;

        public MediaPlayerAudioDeviceEventArgs(string audioDevice)
        {
            AudioDevice = audioDevice;
        }
    }

    public class MediaPlayerVolumeChangedEventArgs : EventArgs
    {
        public readonly float Volume;

        public MediaPlayerVolumeChangedEventArgs(float volume)
        {
            Volume = volume;
        }
    }

    #endregion

    #region MediaList events

    public abstract class MediaListBaseEventArgs : EventArgs
    {
        public readonly Media Media;
        public readonly int Index;

        protected MediaListBaseEventArgs(Media media, int index)
        {
            Media = media;
            Index = index;
        }
    }

    public class MediaListItemAddedEventArgs : MediaListBaseEventArgs
    {
        public MediaListItemAddedEventArgs(Media media, int index) : base(media, index)
        {
        }
    }

    public class MediaListWillAddItemEventArgs : MediaListBaseEventArgs
    {
        public MediaListWillAddItemEventArgs(Media media, int index) : base(media, index)
        {
        }
    }

    public class MediaListItemDeletedEventArgs : MediaListBaseEventArgs
    {
        public MediaListItemDeletedEventArgs(Media media, int index) : base(media, index)
        {
        }
    }

    public class MediaListWillDeleteItemEventArgs : MediaListBaseEventArgs
    {
        public MediaListWillDeleteItemEventArgs(Media media, int index) : base(media, index)
        {
        }
    }

    #endregion

    #region MediaListPlayer events

    public class MediaListPlayerNextItemSetEventArgs : EventArgs
    {
        public readonly Media Media;

        public MediaListPlayerNextItemSetEventArgs(Media media)
        {
            Media = media;
        }
    }

    #endregion

    #region VLM events

    public class VLMMediaEventArgs : EventArgs
    {
        public readonly string InstanceName;
        public readonly string MediaName;

        public VLMMediaEventArgs(string mediaName = "", string instanceName = "")
        {
            MediaName = mediaName;
            InstanceName = instanceName;
        }
    }

    #endregion

    #region RendererDiscoverer events

    public class RendererDiscovererItemAddedEventArgs : EventArgs
    {
        public RendererDiscovererItemAddedEventArgs(RendererItem rendererItem)
        {
            RendererItem = rendererItem;
        }

        public RendererItem RendererItem { get; }
    }

    public class RendererDiscovererItemDeletedEventArgs : EventArgs
    {
        public RendererDiscovererItemDeletedEventArgs(RendererItem rendererItem)
        {
            RendererItem = rendererItem;
        }

        public RendererItem RendererItem { get; }
    }

    #endregion

    public sealed class LogEventArgs : EventArgs
    {
        public LogEventArgs(LogLevel level, string message, string module, string sourceFile, uint? sourceLine)
        {
            Level = level;
            Message = message;
            Module = module;
            SourceFile = sourceFile;
            SourceLine = sourceLine;
        }

        /// <summary>
        /// The severity of the log message.
        /// By default, you will only get error messages, but you can get all messages by specifying "-vv" in the options.
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// The log message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The name of the module that emitted the message
        /// </summary>
        public string Module { get; }

        /// <summary>
        /// The source file that emitted the message.
        /// This may be <see langword="null"/> if that info is not available, i.e. always if you are using a release version of VLC.
        /// </summary>
        public string SourceFile { get; }

        /// <summary>
        /// The line in the <see cref="SourceFile"/> at which the message was emitted.
        /// This may be <see langword="null"/> if that info is not available, i.e. always if you are using a release version of VLC.
        /// </summary>
        public uint? SourceLine { get; }
    }
}