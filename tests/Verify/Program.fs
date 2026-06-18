module Verify

open System
open System.Text
open System.Diagnostics
open Thomoku

let playGame (seed: int) =
    Computer.random <- Random(seed)
    let c = Computer()
    c.A <- Array.init 40 (fun _ -> Array.zeroCreate<int> 40)
    c.kokovuoro <- 0
    c.arpamaara <- 1
    let sb = StringBuilder()
    let cx = 17 + c.arpa()
    let cy = 17 + c.arpa4()
    c.A.[cx].[cy] <- 1
    c.vuoro <- 2
    c.kokovuoro <- c.kokovuoro + 1
    let mutable placed = 1
    sb.Append(sprintf "X%d,%d;" cx cy) |> ignore
    let mutable vuororasti = false
    let mutable winner = 0
    let mutable stuck = false
    let mutable turn = 0
    while not stuck && winner = 0 && turn < 1600 do
        c.vuoro <- if vuororasti then 1 else 2
        c.prionollaus()
        if c.kokovuoro < 10 then c.alotus() |> ignore
        c.CmpComString <- " "
        c.mietipaikka() |> ignore
        let p = c.haeparas()
        if p.Y < 0 || p.Y >= 40 || p.X < 0 || p.X >= 40 || c.A.[p.Y].[p.X] <> 0 then
            sb.Append("STUCK;") |> ignore
            winner <- -1
            stuck <- true
        else
            c.A.[p.Y].[p.X] <- c.vuoro
            placed <- placed + 1
            sb.Append(sprintf "%c%d,%d;" (if c.vuoro = 1 then 'X' else 'O') p.Y p.X) |> ignore
            winner <- c.TarkistaVoitto()
            c.kokovuoro <- c.kokovuoro + 1
            if winner = 0 then vuororasti <- not vuororasti
        turn <- turn + 1
    placed, winner, sb.ToString()

// Time a single move on a board where the computer (O) can complete 5 immediately,
// vs a normal mid-game move, to see whether the cheap win-detection short-circuits.
let winSpeedTest () =
    let mk () =
        let c = Computer()
        c.A <- Array.init 40 (fun _ -> Array.zeroCreate<int> 40)
        c.kokovuoro <- 20          // > 10 so the opening book (alotus) is skipped
        c.arpamaara <- 0
        c.vuoro <- 2               // computer plays O
        c
    // (a) immediate win: O has 4 vertical with both ends open
    let cWin = mk ()
    for r in 18 .. 21 do cWin.A.[20].[r] <- 2
    let sw1 = Stopwatch.StartNew()
    cWin.prionollaus()
    let r1 = cWin.mietipaikka()
    let p1 = cWin.haeparas()
    sw1.Stop()
    printfn "immediate-win:  mietipaikka=%d move=(Y=%d,X=%d) elapsed=%dms" r1 p1.Y p1.X sw1.ElapsedMilliseconds
    // (b) a plain scattered position (no immediate win/threat)
    let cMid = mk ()
    cMid.A.[20].[20] <- 2
    cMid.A.[22].[19] <- 1
    cMid.A.[19].[21] <- 2
    let sw2 = Stopwatch.StartNew()
    cMid.prionollaus()
    let r2 = cMid.mietipaikka()
    let p2 = cMid.haeparas()
    sw2.Stop()
    printfn "normal-move:    mietipaikka=%d move=(Y=%d,X=%d) elapsed=%dms" r2 p2.Y p2.X sw2.ElapsedMilliseconds

[<EntryPoint>]
let main argv =
    if argv.Length > 0 && argv.[0] = "win" then winSpeedTest (); exit 0
    let games = if argv.Length > 0 then int argv.[0] else 10
    let goldenPath = IO.Path.Combine(__SOURCE_DIRECTORY__, "..", "golden_signatures.txt")
    let golden =
        if IO.File.Exists goldenPath then
            IO.File.ReadAllLines goldenPath
            |> Array.map (fun line ->
                let seed = int ((line.Split ' ').[0].Substring 5)
                let sg = line.Substring(line.IndexOf "sig=" + 4)
                seed, sg)
            |> Map.ofArray
        else Map.empty

    let sw = Stopwatch.StartNew()
    let mutable total = 0
    let mutable mn = Int32.MaxValue
    let mutable mx = 0
    let mutable bad = 0
    let mutable mism = 0
    for seed in 1 .. games do
        let moves, winner, sg = playGame seed
        total <- total + moves
        mn <- min mn moves
        mx <- max mx moves
        if winner <= 0 || moves < 15 || moves >= 1600 then
            bad <- bad + 1
            printfn "  BAD seed=%d moves=%d winner=%d" seed moves winner
        match Map.tryFind seed golden with
        | Some g when g <> sg -> mism <- mism + 1; printfn "  MISMATCH seed=%d" seed
        | _ -> ()
    sw.Stop()
    printfn "games=%d avg=%.1f min=%d max=%d | envelope-fails=%d golden-mismatches=%d | %.1fs"
        games (float total / float games) mn mx bad mism sw.Elapsed.TotalSeconds
    if bad = 0 && mism = 0 then 0 else 1
