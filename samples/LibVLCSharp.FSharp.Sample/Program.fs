﻿open System
open LibVLCSharp

[<EntryPoint>]
let main argv =
    Core.Initialize()
    let libVLC = new LibVLC(true)
    let mp = new MediaPlayer(libVLC)
    let media = new Media(new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"))
    mp.Play(media) |> ignore
    media.Dispose()
    Console.ReadKey() |> ignore
    mp.Dispose()
    libVLC.Dispose()
    0
