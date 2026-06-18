# THO-MOKU — F# Fable web port

A browser version of the classic **THO-MOKU** gomoku/five-in-a-row game, deployed to GitHub
Pages. The strong computer AI is the original Silverlight C# engine, ported faithfully to F# and
compiled to JavaScript with [Fable](https://fable.io/).

> The historical DOS/OpenGL/Silverlight versions and the AI write-up live in the repository root
> (`Readme.md`, `THOMOKU/`, `thomokugl/`). This document covers the web port.

## How it fits together

```
reference/            C# reference oracle — the decompiled Silverlight `Computer` class,
                      made runnable + seedable. Used ONLY to verify the F# port.
src/Thomoku.Core/     Computer.fs — the AI engine, a faithful F# port of the C# logic.
                      Fable-compatible (no .NET-only APIs). Also compiles for .NET (tests).
src/Thomoku.App/      App.fs — the Feliz (React) UI: 40x40 board, original x.jpg/o.jpg markers,
                      PvC / PvP / CvP modes, restart, win highlight, engine "comments".
tests/                selfplay.fsx — computer-vs-computer self-play sanity + oracle-equivalence.
                      Verify/      — fast compiled equivalence/envelope check.
                      golden_signatures.txt — move-by-move output captured from the C# oracle.
```

### Why a port (and not "just reference the DLL")?

Fable transpiles **F# → JavaScript**; it cannot load a compiled .NET DLL in the browser. So the
C# `Computer` logic was ported to F# (`Computer.fs`). Because F# also compiles for .NET, the port
is unit-tested for **exact behavioural equality** against the original C# engine: with the same
RNG seed both produce identical games, move for move (see `tests/golden_signatures.txt`).

## Develop locally

Prerequisites: **.NET SDK 10**, **Node.js 18+**.

```bash
dotnet tool restore        # installs the Fable compiler (once)
npm install                # react, react-dom, vite
npm start                  # Fable watch + Vite dev server  ->  http://localhost:5173/thomoku/
```

Production build (what CI runs):

```bash
npm run build              # dotnet fable src/Thomoku.App  +  vite build  ->  dist/
```

## Tests

```bash
# Fast compiled check (envelope + exact match vs the C# oracle), N games:
dotnet run -c Release --project tests/Verify 5

# Self-play script (the engine plays itself); default 10 games, or pass a count:
dotnet fsi tests/selfplay.fsx 5
```

A healthy game ends decisively in ~30–50 moves. The tests fail if any game is shorter than 15
moves (instant/false end → wiring bug) or fills the 40×40 board (1600 moves → win never detected),
and if any game diverges from the C# reference. **Note:** the faithful depth-5 search is heavy
(~15s/game), so large runs take a few minutes — this is expected, and the UI only ever computes
one move at a time.

## Deployment

`.github/workflows/deploy.yml` builds on every push to `master` and publishes `dist/` to GitHub
Pages. Enable **Settings → Pages → Source: GitHub Actions**. The site is served from
`/thomoku/` (configured via `base` in `vite.config.mjs`).
