// Self-play sanity + equivalence test for the F# Thomoku engine.
//
//   1) The computer plays itself. A healthy game ends decisively in ~50 moves.
//      We assert: every game ends with a winner, no game is < 15 moves (would mean an
//      instant/false end or a wiring bug), and no game fills the 40x40 board (1600 moves,
//      which would mean we never detect a win / call the engine wrong).
//   2) Exact equivalence: with the same System.Random seed, the F# port must reproduce the
//      C# reference oracle move-for-move (golden_signatures.txt was produced by the C# DLL port).
//
// Run:  dotnet fsi tests/selfplay.fsx

#r "../src/Thomoku.Core/bin/Debug/net10.0/Thomoku.Core.dll"

open System
open System.Text
open Thomoku

/// Play one full computer-vs-computer game for a given RNG seed.
/// Mirrors MainPage.Konevuoro's per-move pipeline for BOTH sides.
/// Returns (stones placed incl. the seed mark, winner 0/1/2/-1, move signature string).
let playGame (seed: int) =
    Computer.random <- Random(seed)        // seed BEFORE constructing (ctor consumes the RNG)
    let c = Computer()
    c.A <- Array.init 40 (fun _ -> Array.zeroCreate<int> 40)
    c.kokovuoro <- 0
    c.arpamaara <- 1

    let sb = StringBuilder()

    // Seed mark: X near the centre, exactly like the original MainPage ctor.
    let cx = 17 + c.arpa()
    let cy = 17 + c.arpa4()
    c.A.[cx].[cy] <- 1
    c.vuoro <- 2
    c.kokovuoro <- c.kokovuoro + 1
    let mutable placed = 1
    sb.Append(sprintf "X%d,%d;" cx cy) |> ignore

    let mutable vuororasti = false      // false => O (2) to move next, true => X (1)
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

// ---------------------------------------------------------------------------
// Single pass over seeds 1..N: gather the statistical envelope AND compare every
// game move-for-move against the C# reference oracle (golden_signatures.txt).
// NOTE: the depth-5 search is heavy (~15s/game), so the default of 10 games takes
// a few minutes. Pass a smaller number as the first arg for a quick check.
// ---------------------------------------------------------------------------
let games =
    match fsi.CommandLineArgs with
    | [| _; n |] -> int n
    | _ -> 10

let goldenPath = IO.Path.Combine(__SOURCE_DIRECTORY__, "golden_signatures.txt")
let golden =
    if IO.File.Exists goldenPath then
        IO.File.ReadAllLines goldenPath
        |> Array.map (fun line ->
            let seed = int ((line.Split ' ').[0].Substring 5)
            seed, line.Substring(line.IndexOf "sig=" + 4))
        |> Map.ofArray
    else Map.empty

let mutable total = 0
let mutable mn = Int32.MaxValue
let mutable mx = 0
let mutable failures = []
let mutable mismatches = 0
for seed in 1 .. games do
    let moves, winner, sgn = playGame seed
    total <- total + moves
    mn <- min mn moves
    mx <- max mx moves
    if winner <= 0 then failures <- (sprintf "seed %d: no winner (winner=%d, %d moves)" seed winner moves) :: failures
    if moves < 15 then failures <- (sprintf "seed %d: game too short (%d moves) — instant end / wiring bug" seed moves) :: failures
    if moves >= 1600 then failures <- (sprintf "seed %d: board full (%d) — win never detected" seed moves) :: failures
    match Map.tryFind seed golden with
    | Some g when g <> sgn -> mismatches <- mismatches + 1; printfn "MISMATCH vs oracle at seed=%d" seed
    | _ -> ()

let avg = float total / float games
printfn "Self-play: %d games, avg=%.1f moves, min=%d, max=%d" games avg mn mx
if not (Map.isEmpty golden) then
    printfn "Equivalence vs C# oracle: %s" (if mismatches = 0 then "ALL MATCH ✓" else sprintf "%d MISMATCH(es) ✗" mismatches)
else
    printfn "WARN: golden_signatures.txt not found — skipping equivalence check"

// ---------------------------------------------------------------------------
let ok = List.isEmpty failures && mismatches = 0
if not (List.isEmpty failures) then
    printfn "ENVELOPE FAILURES:"
    failures |> List.rev |> List.iter (printfn "  - %s")
printfn "\n%s" (if ok then "PASS ✓" else "FAIL ✗")
exit (if ok then 0 else 1)
