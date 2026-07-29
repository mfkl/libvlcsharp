open System
open LibVLCSharp.Shared

[<EntryPoint>]
let main argv =
    let libVLC = new LibVLC(true)
    let mp = new MediaPlayer(libVLC)
    // F# projects can't consume the C# shared-source project (libVlcSharp.Samples), so this URL is kept inline.
    let media = new Media(libVLC, new Uri("https://dn720409.ca.archive.org/0/items/BigBuckBunny/big_buck_bunny_480p_stereo.avi"))
    mp.Play(media) |> ignore
    media.Dispose()
    Console.ReadKey() |> ignore
    mp.Dispose()
    libVLC.Dispose()
    0
