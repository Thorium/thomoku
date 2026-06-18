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

/// All mutable game state. The engine (Computer) is itself stateful, so we keep one instance
/// and mirror just enough for rendering. A React tick forces re-render after each mutation.
type Game =
    { mutable c: Computer
      mutable vuororasti: bool          // true => X (1) to move, false => O (2)
      mutable mode: Mode
      mutable winner: int               // 0 none, 1 X, 2 O
      mutable last: (int * int) option  // last placed (col,row) for the active marker
      mutable comment: string
      mutable thinking: bool             // computer is computing its move
      mutable winLine: (int * int * int * int) option }  // (x1,y1,x2,y2) of the winning row

let newGame (mode: Mode) =
    let c = Computer()
    c.A <- Array.init boardSize (fun _ -> Array.zeroCreate<int> boardSize)
    c.kokovuoro <- 0
    c.arpamaara <- 0
    match mode with
    | ComputerVsPlayer ->
        // Computer is X and opens at a random-ish centre (the original's auto first move).
        let cx = 17 + c.arpa()
        let cy = 17 + c.arpa4()
        c.A.[cx].[cy] <- 1
        c.vuoro <- 2
        c.kokovuoro <- 1
        { c = c; vuororasti = false; mode = mode; winner = 0; last = Some(cx, cy); comment = ""; thinking = false; winLine = None }
    | PlayerVsPlayer | PlayerVsComputer ->
        // X moves first on an empty board (the human in PvC; the first player in PvP).
        { c = c; vuororasti = true; mode = mode; winner = 0; last = None; comment = ""; thinking = false; winLine = None }

/// The computer's turn — mirrors MainPage.Konevuoro.
let konevuoro (g: Game) =
    let c = g.c
    c.vuoro <- if g.vuororasti then 1 else 2
    c.prionollaus()
    if c.kokovuoro < 10 then c.alotus() |> ignore
    c.CmpComString <- " "
    c.mietipaikka() |> ignore
    g.comment <- c.CmpComString
    let p = c.haeparas()
    if p.Y >= 0 && p.Y < boardSize && p.X >= 0 && p.X < boardSize && c.A.[p.Y].[p.X] = 0 then
        c.A.[p.Y].[p.X] <- c.vuoro
        g.last <- Some(p.Y, p.X)
    let w = c.TarkistaVoitto()
    if w <> 0 then
        g.winner <- w
        g.winLine <- Some(c.suora1x, c.suora1y, c.suora2x, c.suora2y)
    else g.vuororasti <- not g.vuororasti
    c.kokovuoro <- c.kokovuoro + 1

/// Is it the computer's turn to move in the current mode?
let private computersTurn (g: Game) =
    match g.mode with
    | PlayerVsComputer -> not g.vuororasti      // computer = O
    | ComputerVsPlayer -> g.vuororasti          // computer = X
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
    if g.winner = 0 && not g.thinking && c.A.[col].[row] = 0 then
        // Only allow a human move when it is actually the human's turn.
        let humanTurn =
            match g.mode with
            | PlayerVsPlayer -> true
            | PlayerVsComputer -> g.vuororasti          // human = X
            | ComputerVsPlayer -> not g.vuororasti      // human = O
        if humanTurn then
            c.vuoro <- if g.vuororasti then 1 else 2
            c.A.[col].[row] <- c.vuoro
            g.last <- Some(col, row)
            c.kokovuoro <- c.kokovuoro + 1
            let w = c.TarkistaVoitto()
            if w <> 0 then
                g.winner <- w
                g.winLine <- Some(c.suora1x, c.suora1y, c.suora2x, c.suora2y)
                rerender ()
            else
                g.vuororasti <- not g.vuororasti
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
        | 1 -> "X WON!"
        | 2 -> "O WON!"
        | _ -> if g.thinking then "Computer is thinking…"
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
                                    let v = c.A.[col].[row]
                                    let isLast = g.last = Some(col, row)
                                    Html.div [
                                        prop.key (sprintf "%d_%d" col row)
                                        prop.className (
                                            "cell"
                                            + (if v = 1 then " x" elif v = 2 then " o" else "")
                                            + (if isLast then " last" else ""))
                                        prop.onClick (fun _ -> playerClick g col row rerender)
                                    ]
                        ]
                    ]
                    // Red line over the 5 winning marks. viewBox is in cell units so it scales
                    // with the board on any screen size.
                    match g.winLine with
                    | Some (x1, y1, x2, y2) ->
                        Svg.svg [
                            svg.className "winline"
                            svg.viewBox (0, 0, boardSize, boardSize)
                            svg.children [
                                Svg.line [
                                    svg.x1 (float x1 + 0.5); svg.y1 (float y1 + 0.5)
                                    svg.x2 (float x2 + 0.5); svg.y2 (float y2 + 0.5)
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
