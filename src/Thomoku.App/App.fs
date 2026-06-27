module App

open Feliz
open Browser.Dom
open Thomoku

// ---------------------------------------------------------------------------
// THO-MOKU — Fable + Feliz UI over the ported C# gomoku engine.
// Replicates the original Silverlight MainPage game flow.
// ---------------------------------------------------------------------------

let boardSize = 40

type Mode =
    | ComputerVsPlayer   // computer = X (moves first)
    | PlayerVsPlayer
    | PlayerVsComputer   // computer = O

/// The two players. X always moves first. The engine stores moves as ints
/// (1 = X, 2 = O); these helpers convert at that boundary so the UI never
/// juggles magic numbers.
[<Struct>]
type Player =
    | X
    | O

module Player =
    let toInt = function X -> 1 | O -> 2
    let ofInt = function 2 -> O | _ -> X     // engine: 1 = X, 2 = O
    let other = function X -> O | O -> X

/// A board cell as the UI sees it (the engine's board is a 0/1/2 int grid).
[<Struct>]
type Cell =
    | EmptyCell
    | Taken of Player

module Cell =
    let ofInt = function 1 -> Taken X | 2 -> Taken O | _ -> EmptyCell

/// The winning five-in-a-row, as board-cell endpoints (the engine reports
/// these as suora1x/suora1y → suora2x/suora2y).
type WinLine = { X1: int; Y1: int; X2: int; Y2: int }

/// All mutable game state. The engine (Computer) is itself stateful, so we keep one instance
/// and mirror just enough for rendering. A React tick forces re-render after each mutation.
type Game =
    { mutable c: Computer
      mutable toMove: Player            // whose turn it is (X moves first)
      mutable mode: Mode
      mutable winner: Player option     // None until someone wins
      mutable last: (int * int) option  // last placed (col,row) for the active marker
      mutable comment: string
      mutable thinking: bool             // computer is computing its move
      mutable winLine: WinLine option }  // the winning row, once there is one

let newGame (mode: Mode) =
    let c = Computer()
    // A fresh Computer already has kokovuoro = arpamaara = 0; pin the board to boardSize.
    c.A <- Array.init boardSize (fun _ -> Array.zeroCreate<int> boardSize)
    match mode with
    | ComputerVsPlayer ->
        // Computer is X and opens at a random-ish centre (the original's auto first move).
        let cx = 17 + c.arpa()
        let cy = 17 + c.arpa4()
        c.A.[cx].[cy] <- 1
        c.vuoro <- 2
        c.kokovuoro <- 1
        { c = c; toMove = O; mode = mode; winner = None; last = Some(cx, cy); comment = ""; thinking = false; winLine = None }
    | PlayerVsPlayer | PlayerVsComputer ->
        // X moves first on an empty board (the human in PvC; the first player in PvP).
        { c = c; toMove = X; mode = mode; winner = None; last = None; comment = ""; thinking = false; winLine = None }

/// The computer's turn — mirrors MainPage.Konevuoro.
let konevuoro (g: Game) =
    let c = g.c
    c.vuoro <- Player.toInt g.toMove
    c.prionollaus()
    // alotus() returns 1 when the pre-taught opening book chose this move (it boosts that
    // cell's PRIORITY to 500 so haeparas picks it), or 0 when play has left the book and no
    // book move applies. We don't use it for move selection (haeparas reads PRIORITY), but it
    // tells us whether the book is driving this turn. On a book-move turn mietipaikka() still
    // runs and sets CmpComString from its own analysis, which doesn't describe the book move —
    // hence bogus remarks like "Now you had the straight four" with no four on the board. So
    // suppress the comment only while the book is actually driving; the moment the opening
    // derails alotus() returns 0 and real commentary resumes automatically.
    let bookMove = if c.kokovuoro < 10 then c.alotus() else 0
    c.CmpComString <- " "
    c.mietipaikka() |> ignore
    g.comment <- if bookMove = 1 then "" else c.CmpComString
    let p = c.haeparas()
    if p.Y >= 0 && p.Y < boardSize && p.X >= 0 && p.X < boardSize && c.A.[p.Y].[p.X] = 0 then
        c.A.[p.Y].[p.X] <- c.vuoro
        g.last <- Some(p.Y, p.X)
    let w = c.TarkistaVoitto()
    if w <> 0 then
        g.winner <- Some(Player.ofInt w)
        g.winLine <- Some { X1 = c.suora1x; Y1 = c.suora1y; X2 = c.suora2x; Y2 = c.suora2y }
    else g.toMove <- Player.other g.toMove
    c.kokovuoro <- c.kokovuoro + 1

/// Is it the computer's turn to move in the current mode?
let private computersTurn (g: Game) =
    match g.mode with
    | PlayerVsComputer -> g.toMove = O          // computer = O
    | ComputerVsPlayer -> g.toMove = X          // computer = X
    | PlayerVsPlayer -> false

/// Run the computer's move on a deferred tick so the browser can first paint the human's
/// move + the "thinking" indicator (the search is heavy and blocks the main thread).
let private scheduleCpu (g: Game) (rerender: unit -> unit) =
    g.thinking <- true
    rerender ()
    Fable.Core.JS.setTimeout (fun () ->
        konevuoro g
        // The computer may have handed the turn back to itself (it never does here), but in
        // any case stop showing the indicator and repaint.
        g.thinking <- false
        rerender ()) 30 |> ignore

/// A human click on (col,row) — mirrors MainPage.i_Click. Owns its own re-render so it can
/// repaint the human move immediately and then compute the reply asynchronously.
let playerClick (g: Game) (col: int) (row: int) (rerender: unit -> unit) =
    let c = g.c
    if g.winner = None && not g.thinking && c.A.[col].[row] = 0 then
        // Only allow a human move when it is actually the human's turn.
        let humanTurn =
            match g.mode with
            | PlayerVsPlayer -> true
            | PlayerVsComputer -> g.toMove = X          // human = X
            | ComputerVsPlayer -> g.toMove = O          // human = O
        if humanTurn then
            c.vuoro <- Player.toInt g.toMove
            c.A.[col].[row] <- c.vuoro
            g.last <- Some(col, row)
            c.kokovuoro <- c.kokovuoro + 1
            let w = c.TarkistaVoitto()
            if w <> 0 then
                g.winner <- Some(Player.ofInt w)
                g.winLine <- Some { X1 = c.suora1x; Y1 = c.suora1y; X2 = c.suora2x; Y2 = c.suora2y }
                rerender ()
            else
                g.toMove <- Player.other g.toMove
                if computersTurn g then scheduleCpu g rerender   // paints move + spinner, then computes
                else rerender ()

// Single game instance for the page.
let mutable game = newGame ComputerVsPlayer

[<ReactComponent>]
let App () =
    // A stable force-render via useReducer. The dispatch identity is constant across renders,
    // so it is safe to capture in the deferred CPU-move callback (unlike a useState setter +
    // captured value, which would go stale and silently skip the repaint of the computer's move).
    let _, rerender = React.useReducer((fun (s: int) (_: unit) -> s + 1), 0)

    let g = game
    let c = g.c

    let statusText =
        match g.winner with
        | Some X -> "X WON!"
        | Some O -> "O WON!"
        | None -> if g.thinking then "Computer is thinking…"
                  elif g.comment.Trim() = "" then ""
                  else g.comment

    Html.div [
        prop.className "thomoku-root"
        prop.children [
            Html.div [
                prop.className "topbar"
                prop.children [
                    Html.span [ prop.className "title"; prop.text "THO-MOKU" ]
                    if g.thinking then
                        Html.span [
                            prop.className "thinking"
                            prop.children [
                                Html.span [ prop.className "spinner" ]
                                Html.text "thinking…"
                            ]
                        ]
                    Html.button [
                        prop.className "restart"
                        prop.text "(Restart)"
                        prop.onClick (fun _ -> game <- newGame g.mode; rerender ())
                    ]
                ]
            ]
            Html.div [
                prop.className "modes"
                prop.children [
                    for (label, m) in [ "Computer vs. Player", ComputerVsPlayer
                                        "Player vs. Player", PlayerVsPlayer
                                        "Player vs. Computer", PlayerVsComputer ] do
                        Html.label [
                            prop.children [
                                Html.input [
                                    prop.type' "radio"
                                    prop.name "mode"
                                    prop.isChecked (g.mode = m)
                                    prop.onChange (fun (_: bool) ->
                                        // New game in the chosen mode. In CvP the centre X is the
                                        // computer's opening move (seeded by newGame); other modes
                                        // also start from that seed, matching the original.
                                        game <- newGame m
                                        rerender ())
                                ]
                                Html.text label
                            ]
                        ]
                ]
            ]
            Html.div [ prop.className "comment"; prop.text statusText ]
            Html.div [
                prop.className "board-wrap"
                prop.children [
                    Html.div [
                        prop.className "board"
                        prop.children [
                            for row in 0 .. boardSize - 1 do
                                for col in 0 .. boardSize - 1 do
                                    let isLast = g.last = Some(col, row)
                                    Html.div [
                                        prop.key (sprintf "%d_%d" col row)
                                        prop.classes [
                                            "cell"
                                            match Cell.ofInt c.A.[col].[row] with
                                            | Taken X -> "x"
                                            | Taken O -> "o"
                                            | EmptyCell -> ""
                                            if isLast then "last"
                                        ]
                                        prop.onClick (fun _ -> playerClick g col row rerender)
                                    ]
                        ]
                    ]
                    // Red line over the 5 winning marks. viewBox is in cell units so it scales
                    // with the board on any screen size.
                    match g.winLine with
                    | Some line ->
                        Svg.svg [
                            svg.className "winline"
                            svg.viewBox (0, 0, boardSize, boardSize)
                            svg.children [
                                Svg.line [
                                    svg.x1 (float line.X1 + 0.5); svg.y1 (float line.Y1 + 0.5)
                                    svg.x2 (float line.X2 + 0.5); svg.y2 (float line.Y2 + 0.5)
                                    svg.stroke "#d11"
                                    svg.strokeWidth 0.15
                                    svg.strokeLineCap "round"
                                ]
                            ]
                        ]
                    | None -> Html.none
                ]
            ]
        ]
    ]

ReactDOM.createRoot(document.getElementById "app").render (App ())
