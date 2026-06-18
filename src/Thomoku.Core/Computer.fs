// THO-MOKU — Gomoku/five-in-a-row AI.
// Faithful F# port of the Silverlight C# `Computer` class (ThomokuSL5), for use with Fable.
// Verified for exact behavioural equality against the original C# (see /tests).

// Suppress FS0049: the port keeps the original C#'s UPPERCASE parameter names
// (PISTEET, AA, BB, PRIORITY, B, V1/V2, X/Y, C/D) for a faithful 1:1 mapping.
#nowarn "0049"

namespace Thomoku

// Early-return helpers (the C# uses `return` mid-loop; each method catches its OWN raise).
// Used only by the once-per-move methods; the hot recursive search is exception-free.
exception ReturnInt of int
exception ReturnUnit

/// A board position. NOTE the original semantics: when the engine returns a move, place it
/// at A.[p.Y].[p.X] (Y indexes the first dimension, X the second) — preserved from the C#.
type Pelipaikka() =
    member val X = 0 with get, set
    member val Y = 0 with get, set

type Computer() =

    /// Shared RNG (settable so tests can seed deterministically, matching the C# static field).
    static member val random : System.Random = System.Random() with get, set

    // Fields (C# defaults preserved). Exposed as properties so methods can use `this.x`.
    member val laudankoko = 40 with get, set
    member val PRIORITY : int[][] = Array.init 64 (fun _ -> Array.zeroCreate<int> 64) with get, set
    member val maxsyvyys = 5 with get, set
    member val puhe = 1 with get, set
    member val vuoro = 0 with get, set
    member val kokovuoro = 0 with get, set
    member val arpamaara = 0 with get, set
    member val suora1x = 0 with get, set
    member val suora1y = 0 with get, set
    member val suora2x = 0 with get, set
    member val suora2y = 0 with get, set
    member val A : int[][] = Array.init 40 (fun _ -> Array.zeroCreate<int> 40) with get, set
    member val CmpComString = "" with get, set

    // The C# ctor sets `alotyyli = arpa5()` (1 RNG draw; 0 -> 4) then `alotyyli2 = arpa()` (2 draws,
    // first discarded). member val initializers run in declaration order, so this preserves the
    // exact RNG draw sequence. alotyyli MUST be declared before alotyyli2.
    member val alotyyli =
        let a = System.Convert.ToInt32(Computer.random.NextDouble() * 3.0)
        if a = 0 then 4 else a
        with get, set
    member val alotyyli2 =
        System.Convert.ToInt32(Computer.random.NextDouble() * 3.0) |> ignore
        System.Convert.ToInt32(Computer.random.NextDouble() * 3.0) + 1
        with get, set

    // NOTE: the C# `arpa` discards one NextDouble() before returning — that wasted draw
    // MUST be kept so the RNG sequence matches the reference exactly.
    member this.arpa() =
        System.Convert.ToInt32(Computer.random.NextDouble() * 3.0) |> ignore
        System.Convert.ToInt32(Computer.random.NextDouble() * 3.0) + 1

    member this.arpa4() =
        System.Convert.ToInt32(Computer.random.NextDouble() * 3.0) + 1

    member this.arpa5() =
        System.Convert.ToInt32(Computer.random.NextDouble() * 3.0)

    member this.prionollaus() =
        for i in 0 .. 63 do
            for j in 0 .. 63 do
                this.PRIORITY.[j].[i] <- 0


    member this.taulukko1(AA: int[][], V1: int, V2: int) =
        AA.[0].[0] <- V2
        AA.[0].[1] <- V2
        AA.[0].[2] <- 0
        AA.[0].[3] <- V2
        AA.[0].[4] <- V2
        AA.[0].[5] <- 5
        AA.[0].[6] <- 5
        AA.[0].[7] <- 5
        AA.[1].[0] <- V2
        AA.[1].[1] <- V2
        AA.[1].[2] <- V2
        AA.[1].[3] <- 0
        AA.[1].[4] <- V2
        AA.[1].[5] <- 5
        AA.[1].[6] <- 5
        AA.[1].[7] <- 5
        AA.[2].[0] <- V2
        AA.[2].[1] <- V2
        AA.[2].[2] <- V2
        AA.[2].[3] <- V2
        AA.[2].[4] <- 0
        AA.[2].[5] <- 5
        AA.[2].[6] <- 5
        AA.[2].[7] <- 5
        AA.[3].[0] <- V1
        AA.[3].[1] <- V1
        AA.[3].[2] <- 0
        AA.[3].[3] <- V1
        AA.[3].[4] <- V1
        AA.[3].[5] <- 5
        AA.[3].[6] <- 5
        AA.[3].[7] <- 5
        AA.[4].[0] <- V1
        AA.[4].[1] <- V1
        AA.[4].[2] <- V1
        AA.[4].[3] <- 0
        AA.[4].[4] <- V1
        AA.[4].[5] <- 5
        AA.[4].[6] <- 5
        AA.[4].[7] <- 5
        AA.[5].[0] <- V1
        AA.[5].[1] <- V1
        AA.[5].[2] <- V1
        AA.[5].[3] <- V1
        AA.[5].[4] <- 0
        AA.[5].[5] <- 5
        AA.[5].[6] <- 5
        AA.[5].[7] <- 5

    member this.taulukko2(AA: int[][], BB: int[][], V1: int, V2: int) =
        for i in 0 .. 16 - 1 do
            for j in 0 .. 8 - 1 do
                BB.[i].[j] <- 5
        AA.[0].[0] <- V2
        AA.[0].[1] <- V2
        AA.[0].[2] <- V2
        AA.[0].[3] <- V2
        AA.[0].[4] <- 0
        AA.[1].[0] <- V2
        AA.[1].[1] <- V2
        AA.[1].[2] <- V2
        AA.[1].[3] <- 0
        AA.[1].[4] <- V2
        AA.[2].[0] <- V2
        AA.[2].[1] <- V2
        AA.[2].[2] <- 0
        AA.[2].[3] <- V2
        AA.[2].[4] <- V2
        AA.[3].[0] <- V2
        AA.[3].[1] <- V2
        AA.[3].[2] <- 0
        AA.[3].[3] <- V2
        AA.[3].[4] <- 0
        AA.[4].[0] <- V2
        AA.[4].[1] <- V2
        AA.[4].[2] <- 0
        AA.[4].[3] <- V2
        AA.[4].[4] <- 0
        AA.[5].[0] <- V2
        AA.[5].[1] <- 0
        AA.[5].[2] <- V2
        AA.[5].[3] <- 0
        AA.[5].[4] <- V2
        AA.[6].[0] <- V2
        AA.[6].[1] <- V2
        AA.[6].[2] <- 0
        AA.[6].[3] <- 0
        AA.[6].[4] <- V2
        AA.[7].[0] <- V2
        AA.[7].[1] <- V2
        AA.[7].[2] <- 0
        AA.[7].[3] <- 0
        AA.[7].[4] <- V2
        AA.[8].[0] <- V2
        AA.[8].[1] <- V2
        AA.[8].[2] <- V2
        AA.[8].[3] <- 0
        AA.[8].[4] <- 0
        AA.[9].[0] <- V2
        AA.[9].[1] <- V2
        AA.[9].[2] <- V2
        AA.[9].[3] <- 0
        AA.[9].[4] <- 0
        AA.[10].[0] <- V2
        AA.[10].[1] <- 0
        AA.[10].[2] <- V2
        AA.[10].[3] <- V2
        AA.[10].[4] <- 0
        AA.[11].[0] <- V2
        AA.[11].[1] <- 0
        AA.[11].[2] <- V2
        AA.[11].[3] <- V2
        AA.[11].[4] <- 0
        BB.[3].[2] <- V1
        BB.[3].[4] <- V2
        BB.[4].[2] <- V2
        BB.[4].[4] <- V1
        BB.[5].[1] <- V2
        BB.[5].[3] <- V1
        BB.[6].[2] <- V2
        BB.[6].[3] <- V1
        BB.[7].[2] <- V1
        BB.[7].[3] <- V2
        BB.[8].[3] <- V2
        BB.[8].[4] <- V1
        BB.[9].[3] <- V1
        BB.[9].[4] <- V2
        BB.[10].[1] <- V1
        BB.[10].[4] <- V2
        BB.[11].[1] <- V2
        BB.[11].[4] <- V1

    member this.taulukko3(AA: int[][], BB: int[][], V1: int, V2: int) =
        for i in 0 .. 16 - 1 do
            for j in 0 .. 8 - 1 do
                BB.[i].[j] <- 5
        AA.[0].[0] <- 0
        AA.[0].[1] <- V2
        AA.[0].[2] <- V2
        AA.[0].[3] <- V2
        AA.[0].[4] <- 0
        AA.[0].[5] <- 0
        AA.[1].[0] <- 0
        AA.[1].[1] <- V2
        AA.[1].[2] <- V2
        AA.[1].[3] <- 0
        AA.[1].[4] <- V2
        AA.[1].[5] <- 0
        AA.[2].[0] <- 0
        AA.[2].[1] <- V2
        AA.[2].[2] <- V2
        AA.[2].[3] <- 0
        AA.[2].[4] <- 0
        AA.[2].[5] <- 0
        AA.[3].[0] <- 0
        AA.[3].[1] <- V2
        AA.[3].[2] <- V2
        AA.[3].[3] <- 0
        AA.[3].[4] <- 0
        AA.[3].[5] <- 0
        AA.[4].[0] <- 0
        AA.[4].[1] <- V2
        AA.[4].[2] <- 0
        AA.[4].[3] <- V2
        AA.[4].[4] <- 0
        AA.[4].[5] <- 0
        AA.[5].[0] <- 0
        AA.[5].[1] <- V2
        AA.[5].[2] <- 0
        AA.[5].[3] <- V2
        AA.[5].[4] <- 0
        AA.[5].[5] <- 0
        AA.[6].[0] <- 0
        AA.[6].[1] <- V2
        AA.[6].[2] <- 0
        AA.[6].[3] <- 0
        AA.[6].[4] <- V2
        AA.[6].[5] <- 0
        BB.[2].[0] <- V1
        BB.[2].[3] <- V2
        BB.[2].[4] <- V1
        BB.[2].[5] <- V1
        BB.[3].[0] <- V1
        BB.[3].[3] <- V1
        BB.[3].[4] <- V2
        BB.[3].[5] <- V1
        BB.[4].[0] <- V1
        BB.[4].[2] <- V2
        BB.[4].[4] <- V1
        BB.[4].[5] <- V1
        BB.[5].[0] <- V1
        BB.[5].[2] <- V1
        BB.[5].[4] <- V2
        BB.[5].[5] <- V1
        BB.[6].[0] <- V1
        BB.[6].[2] <- V2
        BB.[6].[3] <- V1
        BB.[6].[5] <- V1

    member this.taulukko4(AA: int[][], V1: int, V2: int) =
        AA.[0].[0] <- 3
        AA.[0].[1] <- V2
        AA.[0].[2] <- V2
        AA.[0].[3] <- 0
        AA.[0].[4] <- V2
        AA.[0].[5] <- 5
        AA.[0].[6] <- 5
        AA.[0].[7] <- 5
        AA.[1].[0] <- V2
        AA.[1].[1] <- V2
        AA.[1].[2] <- 0
        AA.[1].[3] <- V2
        AA.[1].[4] <- 3
        AA.[1].[5] <- 5
        AA.[1].[6] <- 5
        AA.[1].[7] <- 5
        AA.[2].[0] <- V2
        AA.[2].[1] <- V2
        AA.[2].[2] <- V2
        AA.[2].[3] <- 0
        AA.[2].[4] <- 3
        AA.[2].[5] <- 5
        AA.[2].[6] <- 5
        AA.[2].[7] <- 5
        AA.[3].[0] <- V1
        AA.[3].[1] <- V2
        AA.[3].[2] <- V2
        AA.[3].[3] <- V2
        AA.[3].[4] <- 3
        AA.[3].[5] <- 0
        AA.[3].[6] <- 5
        AA.[3].[7] <- 5
        AA.[4].[0] <- V2
        AA.[4].[1] <- V2
        AA.[4].[2] <- 3
        AA.[4].[3] <- V2
        AA.[4].[4] <- 0
        AA.[4].[5] <- 5
        AA.[4].[6] <- 5
        AA.[4].[7] <- 5
        AA.[5].[0] <- V2
        AA.[5].[1] <- 3
        AA.[5].[2] <- V2
        AA.[5].[3] <- V2
        AA.[5].[4] <- 0
        AA.[5].[5] <- 5
        AA.[5].[6] <- 5
        AA.[5].[7] <- 5
        AA.[6].[0] <- V2
        AA.[6].[1] <- V2
        AA.[6].[2] <- 0
        AA.[6].[3] <- 0
        AA.[6].[4] <- V2
        AA.[6].[5] <- 5
        AA.[6].[6] <- 5
        AA.[6].[7] <- 5
        AA.[7].[0] <- V2
        AA.[7].[1] <- 0
        AA.[7].[2] <- V2
        AA.[7].[3] <- 0
        AA.[7].[4] <- V2
        AA.[7].[5] <- 5
        AA.[7].[6] <- 5
        AA.[7].[7] <- 5
        AA.[8].[0] <- V2
        AA.[8].[1] <- 0
        AA.[8].[2] <- V1
        AA.[8].[3] <- V1
        AA.[8].[4] <- V1
        AA.[8].[5] <- 0
        AA.[8].[6] <- 0
        AA.[8].[7] <- 5
        AA.[9].[0] <- 3
        AA.[9].[1] <- 0
        AA.[9].[2] <- V1
        AA.[9].[3] <- V1
        AA.[9].[4] <- V1
        AA.[9].[5] <- 0
        AA.[9].[6] <- 3
        AA.[9].[7] <- 5
        AA.[10].[0] <- 0
        AA.[10].[1] <- V1
        AA.[10].[2] <- 0
        AA.[10].[3] <- V1
        AA.[10].[4] <- V1
        AA.[10].[5] <- 0
        AA.[10].[6] <- 5
        AA.[10].[7] <- 5
        AA.[11].[0] <- 3
        AA.[11].[1] <- 3
        AA.[11].[2] <- V1
        AA.[11].[3] <- 0
        AA.[11].[4] <- V1
        AA.[11].[5] <- V1
        AA.[11].[6] <- 3
        AA.[11].[7] <- 3
        AA.[12].[0] <- 3
        AA.[12].[1] <- 3
        AA.[12].[2] <- V1
        AA.[12].[3] <- V1
        AA.[12].[4] <- 0
        AA.[12].[5] <- V1
        AA.[12].[6] <- 3
        AA.[12].[7] <- 3
        AA.[13].[0] <- V2
        AA.[13].[1] <- 3
        AA.[13].[2] <- V1
        AA.[13].[3] <- V1
        AA.[13].[4] <- V1
        AA.[13].[5] <- 0
        AA.[13].[6] <- V2
        AA.[13].[7] <- 3
        AA.[14].[0] <- 3
        AA.[14].[1] <- V2
        AA.[14].[2] <- V2
        AA.[14].[3] <- 0
        AA.[14].[4] <- 3
        AA.[14].[5] <- 5
        AA.[14].[6] <- 5
        AA.[14].[7] <- 5
        AA.[15].[0] <- 3
        AA.[15].[1] <- V2
        AA.[15].[2] <- 0
        AA.[15].[3] <- V2
        AA.[15].[4] <- 3
        AA.[15].[5] <- 5
        AA.[15].[6] <- 5
        AA.[15].[7] <- 5
        AA.[16].[0] <- 3
        AA.[16].[1] <- V2
        AA.[16].[2] <- 0
        AA.[16].[3] <- 0
        AA.[16].[4] <- V2
        AA.[16].[5] <- 3
        AA.[16].[6] <- 5
        AA.[16].[7] <- 5
        AA.[17].[0] <- 3
        AA.[17].[1] <- V2
        AA.[17].[2] <- 3
        AA.[17].[3] <- V2
        AA.[17].[4] <- 0
        AA.[17].[5] <- 3
        AA.[17].[6] <- 5
        AA.[17].[7] <- 5
        AA.[18].[0] <- 3
        AA.[18].[1] <- V2
        AA.[18].[2] <- V2
        AA.[18].[3] <- 3
        AA.[18].[4] <- 0
        AA.[18].[5] <- 3
        AA.[18].[6] <- 5
        AA.[18].[7] <- 5
        AA.[19].[0] <- V2
        AA.[19].[1] <- V1
        AA.[19].[2] <- V1
        AA.[19].[3] <- V1
        AA.[19].[4] <- 0
        AA.[19].[5] <- 0
        AA.[19].[6] <- 5
        AA.[19].[7] <- 5
        AA.[20].[0] <- V2
        AA.[20].[1] <- V1
        AA.[20].[2] <- V1
        AA.[20].[3] <- 0
        AA.[20].[4] <- V1
        AA.[20].[5] <- 0
        AA.[20].[6] <- 5
        AA.[20].[7] <- 5
        AA.[21].[0] <- V2
        AA.[21].[1] <- V1
        AA.[21].[2] <- 0
        AA.[21].[3] <- V1
        AA.[21].[4] <- V1
        AA.[21].[5] <- 0
        AA.[21].[6] <- 5
        AA.[21].[7] <- 5
        AA.[22].[0] <- V2
        AA.[22].[1] <- 0
        AA.[22].[2] <- V1
        AA.[22].[3] <- V1
        AA.[22].[4] <- V1
        AA.[22].[5] <- 0
        AA.[22].[6] <- 5
        AA.[22].[7] <- 5
        AA.[23].[0] <- 3
        AA.[23].[1] <- V1
        AA.[23].[2] <- V1
        AA.[23].[3] <- 0
        AA.[23].[4] <- 3
        AA.[23].[5] <- 5
        AA.[23].[6] <- 5
        AA.[23].[7] <- 5
        AA.[24].[0] <- 3
        AA.[24].[1] <- V1
        AA.[24].[2] <- 0
        AA.[24].[3] <- V1
        AA.[24].[4] <- 3
        AA.[24].[5] <- 5
        AA.[24].[6] <- 5
        AA.[24].[7] <- 5
        AA.[25].[0] <- 3
        AA.[25].[1] <- V1
        AA.[25].[2] <- 0
        AA.[25].[3] <- 0
        AA.[25].[4] <- V1
        AA.[25].[5] <- 3
        AA.[25].[6] <- 5
        AA.[25].[7] <- 5
        AA.[26].[0] <- 3
        AA.[26].[1] <- V1
        AA.[26].[2] <- 3
        AA.[26].[3] <- V1
        AA.[26].[4] <- 0
        AA.[26].[5] <- 3
        AA.[26].[6] <- 5
        AA.[26].[7] <- 5
        AA.[27].[0] <- 3
        AA.[27].[1] <- V1
        AA.[27].[2] <- V1
        AA.[27].[3] <- 3
        AA.[27].[4] <- 0
        AA.[27].[5] <- 3
        AA.[27].[6] <- 5
        AA.[27].[7] <- 5
        AA.[28].[0] <- 3
        AA.[28].[1] <- V2
        AA.[28].[2] <- V2
        AA.[28].[3] <- 3
        AA.[28].[4] <- 3
        AA.[28].[5] <- 0
        AA.[28].[6] <- 3
        AA.[28].[7] <- 5
        AA.[29].[0] <- 3
        AA.[29].[1] <- V2
        AA.[29].[2] <- 0
        AA.[29].[3] <- 3
        AA.[29].[4] <- 5
        AA.[29].[5] <- 5
        AA.[29].[6] <- 5
        AA.[29].[7] <- 5
        AA.[30].[0] <- 3
        AA.[30].[1] <- V2
        AA.[30].[2] <- 3
        AA.[30].[3] <- 0
        AA.[30].[4] <- 3
        AA.[30].[5] <- 5
        AA.[30].[6] <- 5
        AA.[30].[7] <- 5
        AA.[31].[0] <- 3
        AA.[31].[1] <- V1
        AA.[31].[2] <- 0
        AA.[31].[3] <- 3
        AA.[31].[4] <- 5
        AA.[31].[5] <- 5
        AA.[31].[6] <- 5
        AA.[31].[7] <- 5
        AA.[32].[0] <- 3
        AA.[32].[1] <- V1
        AA.[32].[2] <- 3
        AA.[32].[3] <- 0
        AA.[32].[4] <- 3
        AA.[32].[5] <- 5
        AA.[32].[6] <- 5
        AA.[32].[7] <- 5
        AA.[33].[0] <- 3
        AA.[33].[1] <- V2
        AA.[33].[2] <- 3
        AA.[33].[3] <- 0
        AA.[33].[4] <- 3
        AA.[33].[5] <- V2
        AA.[33].[6] <- 3
        AA.[33].[7] <- 5
        AA.[34].[0] <- 3
        AA.[34].[1] <- V2
        AA.[34].[2] <- 3
        AA.[34].[3] <- 3
        AA.[34].[4] <- 0
        AA.[34].[5] <- V2
        AA.[34].[6] <- 3
        AA.[34].[7] <- 5
        AA.[35].[0] <- 3
        AA.[35].[1] <- V2
        AA.[35].[2] <- 3
        AA.[35].[3] <- 3
        AA.[35].[4] <- 0
        AA.[35].[5] <- 3
        AA.[35].[6] <- 5
        AA.[35].[7] <- 5
        AA.[36].[0] <- 3
        AA.[36].[1] <- V1
        AA.[36].[2] <- V1
        AA.[36].[3] <- 3
        AA.[36].[4] <- 3
        AA.[36].[5] <- 0
        AA.[36].[6] <- 3
        AA.[36].[7] <- 5
        AA.[37].[0] <- 3
        AA.[37].[1] <- V1
        AA.[37].[2] <- 3
        AA.[37].[3] <- 0
        AA.[37].[4] <- 3
        AA.[37].[5] <- V1
        AA.[37].[6] <- 3
        AA.[37].[7] <- 5
        AA.[38].[0] <- 3
        AA.[38].[1] <- V1
        AA.[38].[2] <- 3
        AA.[38].[3] <- 3
        AA.[38].[4] <- 0
        AA.[38].[5] <- V1
        AA.[38].[6] <- 3
        AA.[38].[7] <- 5
        AA.[39].[0] <- V1
        AA.[39].[1] <- 0
        AA.[39].[2] <- V1
        AA.[39].[3] <- 0
        AA.[39].[4] <- V1
        AA.[39].[5] <- 0
        AA.[39].[6] <- V1
        AA.[39].[7] <- 5


    member this.fastend(PISTEET: int[]) =
        PISTEET.[0] <- 2000
        PISTEET.[1] <- 4000
        PISTEET.[2] <- 4000
        PISTEET.[3] <- 200
        PISTEET.[4] <- 400
        PISTEET.[5] <- 400

    member this.attacker(PISTEET: int[]) =
        PISTEET.[0] <- 2000
        PISTEET.[1] <- 2000
        PISTEET.[2] <- 2000
        PISTEET.[3] <- 2000
        PISTEET.[4] <- 2000
        PISTEET.[5] <- 2000
        PISTEET.[6] <- 1900
        PISTEET.[7] <- 1900
        PISTEET.[8] <- 4000
        PISTEET.[9] <- 4000
        PISTEET.[10] <- 4000
        PISTEET.[11] <- 30
        PISTEET.[12] <- 30
        PISTEET.[13] <- 30
        PISTEET.[14] <- 500
        PISTEET.[15] <- 270
        PISTEET.[16] <- 490
        PISTEET.[17] <- 490
        PISTEET.[18] <- 490
        PISTEET.[19] <- 350
        PISTEET.[20] <- 350
        PISTEET.[21] <- 350
        PISTEET.[22] <- 350
        PISTEET.[23] <- 200
        PISTEET.[24] <- 200
        PISTEET.[25] <- 190
        PISTEET.[26] <- 190
        PISTEET.[27] <- 80
        PISTEET.[28] <- 80
        PISTEET.[29] <- 60
        PISTEET.[30] <- 50
        PISTEET.[31] <- 40
        PISTEET.[32] <- 25
        PISTEET.[33] <- 9
        PISTEET.[34] <- 10
        PISTEET.[35] <- 10
        PISTEET.[36] <- 10
        PISTEET.[37] <- 4
        PISTEET.[38] <- 2
        PISTEET.[39] <- 2000

    member this.defender(PISTEET: int[]) =
        PISTEET.[0] <- 1500
        PISTEET.[1] <- 1500
        PISTEET.[2] <- 1500
        PISTEET.[3] <- 1450
        PISTEET.[4] <- 1400
        PISTEET.[5] <- 1400
        PISTEET.[6] <- 1000
        PISTEET.[7] <- 1000
        PISTEET.[8] <- 2200
        PISTEET.[9] <- 2200
        PISTEET.[10] <- 2200
        PISTEET.[11] <- 80
        PISTEET.[12] <- 80
        PISTEET.[13] <- 80
        PISTEET.[14] <- 400
        PISTEET.[15] <- 220
        PISTEET.[16] <- 385
        PISTEET.[17] <- 385
        PISTEET.[18] <- 385
        PISTEET.[19] <- 410
        PISTEET.[20] <- 400
        PISTEET.[21] <- 400
        PISTEET.[22] <- 400
        PISTEET.[23] <- 385
        PISTEET.[24] <- 198
        PISTEET.[25] <- 380
        PISTEET.[26] <- 380
        PISTEET.[27] <- 90
        PISTEET.[28] <- 80
        if this.kokovuoro > 10 then
            (
                PISTEET.[31] <- 45
                PISTEET.[32] <- 35
                PISTEET.[29] <- 30
                PISTEET.[30] <- 15
            )
        else
            (
                PISTEET.[31] <- 30
                PISTEET.[32] <- 15
                PISTEET.[29] <- 45
                PISTEET.[30] <- 35
            )
        PISTEET.[33] <- 8
        PISTEET.[34] <- 10
        PISTEET.[35] <- 10
        PISTEET.[36] <- 10
        PISTEET.[37] <- 4
        PISTEET.[38] <- 4
        PISTEET.[39] <- 2000

    member this.preasure(PISTEET: int[]) =
        PISTEET.[0] <- 1700
        PISTEET.[1] <- 1700
        PISTEET.[2] <- 1700
        PISTEET.[3] <- 1600
        PISTEET.[4] <- 1700
        PISTEET.[5] <- 1700
        PISTEET.[6] <- 1700
        PISTEET.[7] <- 1700
        PISTEET.[8] <- 2000
        PISTEET.[9] <- 2000
        PISTEET.[10] <- 2000
        PISTEET.[11] <- 50
        PISTEET.[12] <- 50
        PISTEET.[13] <- 50
        PISTEET.[14] <- 500
        PISTEET.[15] <- 270
        PISTEET.[16] <- 490
        PISTEET.[17] <- 490
        PISTEET.[18] <- 490
        PISTEET.[19] <- 480
        PISTEET.[20] <- 380
        PISTEET.[21] <- 380
        PISTEET.[22] <- 380
        PISTEET.[23] <- 240
        PISTEET.[24] <- 130
        PISTEET.[25] <- 220
        PISTEET.[26] <- 210
        PISTEET.[27] <- 90
        PISTEET.[28] <- 100
        PISTEET.[29] <- 80
        PISTEET.[30] <- 50
        PISTEET.[31] <- 30
        PISTEET.[32] <- 25
        PISTEET.[33] <- 9
        PISTEET.[34] <- 10
        PISTEET.[35] <- 10
        PISTEET.[36] <- 10
        PISTEET.[37] <- 3
        PISTEET.[38] <- 2
        PISTEET.[39] <- 2000

    member this.tbuilder(PISTEET: int[]) =
        PISTEET.[0] <- 1000
        PISTEET.[1] <- 1000
        PISTEET.[2] <- 950
        PISTEET.[3] <- 1000
        PISTEET.[4] <- 1000
        PISTEET.[5] <- 1000
        PISTEET.[6] <- 900
        PISTEET.[7] <- 900
        PISTEET.[8] <- 2020
        PISTEET.[9] <- 2000
        PISTEET.[10] <- 2000
        PISTEET.[11] <- 10
        PISTEET.[12] <- 10
        PISTEET.[13] <- 10
        PISTEET.[14] <- 420
        PISTEET.[15] <- 211
        PISTEET.[16] <- 490
        PISTEET.[17] <- 490
        PISTEET.[18] <- 490
        PISTEET.[19] <- 350
        PISTEET.[20] <- 350
        PISTEET.[21] <- 350
        PISTEET.[22] <- 350
        PISTEET.[23] <- 200
        PISTEET.[24] <- 102
        PISTEET.[25] <- 110
        PISTEET.[26] <- 140
        PISTEET.[27] <- 80
        PISTEET.[28] <- 90
        PISTEET.[29] <- 50
        PISTEET.[30] <- 35
        PISTEET.[31] <- 34
        PISTEET.[32] <- 26
        PISTEET.[33] <- 14
        PISTEET.[34] <- 23
        PISTEET.[35] <- 23
        PISTEET.[36] <- 12
        PISTEET.[37] <- 4
        PISTEET.[38] <- 3
        PISTEET.[39] <- 2000


    member this.haepriority(syvyys: int, rivi0: int, suunta0: int, X: int, Y: int, PRIORITY: int[][]) =
        if suunta0 = 0 then
            if rivi0 = 3 && PRIORITY.[X + 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X + 2].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X + 1].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 1].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X + 2].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 7 && PRIORITY.[X + 3].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 8 && PRIORITY.[X + 3].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 9 && PRIORITY.[X + 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 10 && PRIORITY.[X + 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 11 && PRIORITY.[X + 1].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 1].[Y] <- 9500 - syvyys * 1000
        if suunta0 = 1 then
            if rivi0 = 3 && PRIORITY.[X].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X].[Y + 1] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 1] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 7 && PRIORITY.[X].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 8 && PRIORITY.[X].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 9 && PRIORITY.[X].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 10 && PRIORITY.[X].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 11 && PRIORITY.[X].[Y + 1] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 1] <- 9500 - syvyys * 1000
        if suunta0 = 2 then
            if rivi0 = 3 && PRIORITY.[X + 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X + 2].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X + 1].[Y + 1] < 9500 - syvyys * 1000 then PRIORITY.[X + 1].[Y + 1] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X + 2].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 7 && PRIORITY.[X + 3].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 8 && PRIORITY.[X + 3].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 9 && PRIORITY.[X + 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 10 && PRIORITY.[X + 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 11 && PRIORITY.[X + 1].[Y + 1] < 9500 - syvyys * 1000 then PRIORITY.[X + 1].[Y + 1] <- 9500 - syvyys * 1000
        if suunta0 = 3 then
            if rivi0 = 3 && PRIORITY.[X + 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X + 2].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X + 1].[Y - 1] < 9500 - syvyys * 1000 then PRIORITY.[X + 1].[Y - 1] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X + 2].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 7 && PRIORITY.[X + 3].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 8 && PRIORITY.[X + 3].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 9 && PRIORITY.[X + 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 10 && PRIORITY.[X + 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 11 && PRIORITY.[X + 1].[Y - 1] < 9500 - syvyys * 1000 then PRIORITY.[X + 1].[Y - 1] <- 9500 - syvyys * 1000
        if suunta0 = 4 then
            if rivi0 = 3 && PRIORITY.[X - 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X - 2].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X - 1].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 1].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X - 2].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 7 && PRIORITY.[X - 3].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 8 && PRIORITY.[X - 3].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 9 && PRIORITY.[X - 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 10 && PRIORITY.[X - 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 11 && PRIORITY.[X - 1].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 1].[Y] <- 9500 - syvyys * 1000
        if suunta0 = 5 then
            if rivi0 = 3 && PRIORITY.[X].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X].[Y - 1] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 1] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 7 && PRIORITY.[X].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 8 && PRIORITY.[X].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 9 && PRIORITY.[X].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 10 && PRIORITY.[X].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 11 && PRIORITY.[X].[Y - 1] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 1] <- 9500 - syvyys * 1000
        if suunta0 = 6 then
            if rivi0 = 3 && PRIORITY.[X - 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X - 2].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X - 1].[Y - 1] < 9500 - syvyys * 1000 then PRIORITY.[X - 1].[Y - 1] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X - 2].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 7 && PRIORITY.[X - 3].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 8 && PRIORITY.[X - 3].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 9 && PRIORITY.[X - 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 10 && PRIORITY.[X - 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 11 && PRIORITY.[X - 1].[Y - 1] < 9500 - syvyys * 1000 then PRIORITY.[X - 1].[Y - 1] <- 9500 - syvyys * 1000
        if suunta0 = 7 then
            if rivi0 = 3 && PRIORITY.[X - 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X - 2].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X - 1].[Y + 1] < 9500 - syvyys * 1000 then PRIORITY.[X - 1].[Y + 1] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X - 2].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 7 && PRIORITY.[X - 3].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 8 && PRIORITY.[X - 3].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 9 && PRIORITY.[X - 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 10 && PRIORITY.[X - 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 11 && PRIORITY.[X - 1].[Y + 1] < 9500 - syvyys * 1000 then PRIORITY.[X - 1].[Y + 1] <- 9500 - syvyys * 1000
    member this.haepriority2(syvyys: int, rivi0: int, suunta0: int, X: int, Y: int, PRIORITY: int[][]) =
        if suunta0 = 0 then
            if rivi0 = 2 && PRIORITY.[X + 3].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 3 && PRIORITY.[X + 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X + 2].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X + 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X + 2].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y] <- 9500 - syvyys * 1000
        if suunta0 = 1 then
            if rivi0 = 2 && PRIORITY.[X].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 3 && PRIORITY.[X].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y + 2] <- 9500 - syvyys * 1000
        if suunta0 = 2 then
            if rivi0 = 2 && PRIORITY.[X + 3].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 3 && PRIORITY.[X + 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X + 2].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X + 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X + 2].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y + 2] <- 9500 - syvyys * 1000
        if suunta0 = 3 then
            if rivi0 = 2 && PRIORITY.[X + 3].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X + 3].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 3 && PRIORITY.[X + 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X + 2].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X + 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X + 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X + 2].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X + 2].[Y - 2] <- 9500 - syvyys * 1000
        if suunta0 = 4 then
            if rivi0 = 2 && PRIORITY.[X - 3].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 3 && PRIORITY.[X - 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X - 2].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X - 4].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X - 2].[Y] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y] <- 9500 - syvyys * 1000
        if suunta0 = 5 then
            if rivi0 = 2 && PRIORITY.[X].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 3 && PRIORITY.[X].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X].[Y - 2] <- 9500 - syvyys * 1000
        if suunta0 = 6 then
            if rivi0 = 2 && PRIORITY.[X - 3].[Y - 3] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y - 3] <- 9500 - syvyys * 1000
            if rivi0 = 3 && PRIORITY.[X - 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X - 2].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y - 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X - 4].[Y - 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y - 4] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X - 2].[Y - 2] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y - 2] <- 9500 - syvyys * 1000
        if suunta0 = 7 then
            if rivi0 = 2 && PRIORITY.[X - 3].[Y + 3] < 9500 - syvyys * 1000 then PRIORITY.[X - 3].[Y + 3] <- 9500 - syvyys * 1000
            if rivi0 = 3 && PRIORITY.[X - 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 4 && PRIORITY.[X - 2].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y + 2] <- 9500 - syvyys * 1000
            if rivi0 = 5 && PRIORITY.[X - 4].[Y + 4] < 9500 - syvyys * 1000 then PRIORITY.[X - 4].[Y + 4] <- 9500 - syvyys * 1000
            if rivi0 = 6 && PRIORITY.[X - 2].[Y + 2] < 9500 - syvyys * 1000 then PRIORITY.[X - 2].[Y + 2] <- 9500 - syvyys * 1000


    member this.pikahaku(B: int[][], V2: int, haettava: int) : int =
        try
            let mutable V2 = V2
            V2 <- (if V2 <> 1 then 1 else 2)
            let array = Array.init 4 (fun _ -> Array.zeroCreate<int> 8)
            let mutable num = 0
            let mutable num2 = 5
            if haettava = 1 then
                num <- 3
                num2 <- 5
                array.[0].[0] <- V2
                array.[0].[1] <- V2
                array.[0].[2] <- V2
                array.[0].[3] <- V2
                array.[0].[4] <- 0
                array.[1].[0] <- V2
                array.[1].[1] <- V2
                array.[1].[2] <- V2
                array.[1].[3] <- 0
                array.[1].[4] <- V2
                array.[2].[0] <- V2
                array.[2].[1] <- V2
                array.[2].[2] <- 0
                array.[2].[3] <- V2
                array.[2].[4] <- V2
            if haettava = 2 then
                num <- 2
                num2 <- 6
                array.[0].[0] <- 0
                array.[0].[1] <- V2
                array.[0].[2] <- V2
                array.[0].[3] <- V2
                array.[0].[4] <- 0
                array.[0].[5] <- 0
                array.[1].[0] <- 0
                array.[1].[1] <- V2
                array.[1].[2] <- V2
                array.[1].[3] <- 0
                array.[1].[4] <- V2
                array.[1].[5] <- 0
            for i in 0 .. num - 1 do
                for j in 8 .. this.laudankoko + 8 - 1 do
                    for k in 8 .. this.laudankoko + 8 - 1 do
                        if B.[k].[j] = array.[i].[0] && B.[k + 1].[j] = array.[i].[1] && B.[k + 2].[j] = array.[i].[2] && B.[k + 3].[j] = array.[i].[3] && B.[k + 4].[j] = array.[i].[4] then
                            if num2 = 5 then
                                raise (ReturnInt 9)
                            if B.[k + 5].[j] = array.[i].[5] then
                                raise (ReturnInt 9)
                        if B.[k].[j] = array.[i].[0] && B.[k].[j + 1] = array.[i].[1] && B.[k].[j + 2] = array.[i].[2] && B.[k].[j + 3] = array.[i].[3] && B.[k].[j + 4] = array.[i].[4] then
                            if num2 = 5 then
                                raise (ReturnInt 9)
                            if B.[k].[j + 5] = array.[i].[5] then
                                raise (ReturnInt 9)
                        if B.[k].[j] = array.[i].[0] && B.[k + 1].[j + 1] = array.[i].[1] && B.[k + 2].[j + 2] = array.[i].[2] && B.[k + 3].[j + 3] = array.[i].[3] && B.[k + 4].[j + 4] = array.[i].[4] then
                            if num2 = 5 then
                                raise (ReturnInt 9)
                            if B.[k + 5].[j + 5] = array.[i].[5] then
                                raise (ReturnInt 9)
                        if B.[k].[j] = array.[i].[0] && B.[k + 1].[j - 1] = array.[i].[1] && B.[k + 2].[j - 2] = array.[i].[2] && B.[k + 3].[j - 3] = array.[i].[3] && B.[k + 4].[j - 4] = array.[i].[4] then
                            if num2 = 5 then
                                raise (ReturnInt 9)
                            if B.[k + 5].[j - 5] = array.[i].[5] then
                                raise (ReturnInt 9)
                        if B.[k].[j] = array.[i].[0] && B.[k - 1].[j] = array.[i].[1] && B.[k - 2].[j] = array.[i].[2] && B.[k - 3].[j] = array.[i].[3] && B.[k - 4].[j] = array.[i].[4] then
                            if num2 = 5 then
                                raise (ReturnInt 9)
                            if B.[k - 5].[j] = array.[i].[5] then
                                raise (ReturnInt 9)
                        if B.[k].[j] = array.[i].[0] && B.[k].[j - 1] = array.[i].[1] && B.[k].[j - 2] = array.[i].[2] && B.[k].[j - 3] = array.[i].[3] && B.[k].[j - 4] = array.[i].[4] then
                            if num2 = 5 then
                                raise (ReturnInt 9)
                            if B.[k].[j - 5] = array.[i].[5] then
                                raise (ReturnInt 9)
                        if B.[k].[j] = array.[i].[0] && B.[k - 1].[j - 1] = array.[i].[1] && B.[k - 2].[j - 2] = array.[i].[2] && B.[k - 3].[j - 3] = array.[i].[3] && B.[k - 4].[j - 4] = array.[i].[4] then
                            if num2 = 5 then
                                raise (ReturnInt 9)
                            if B.[k - 5].[j - 5] = array.[i].[5] then
                                raise (ReturnInt 9)
                        if B.[k].[j] = array.[i].[0] && B.[k - 1].[j + 1] = array.[i].[1] && B.[k - 2].[j + 2] = array.[i].[2] && B.[k - 3].[j + 3] = array.[i].[3] && B.[k - 4].[j + 4] = array.[i].[4] then
                            if num2 = 5 then
                                raise (ReturnInt 9)
                            if B.[k - 5].[j + 5] = array.[i].[5] then
                                raise (ReturnInt 9)
            0
        with ReturnInt v -> v

    member this.torjuntahaku(B: int[][], V2: int, PRIORITY: int[][]) : int =
        let array = Array.init 4 (fun _ -> Array.zeroCreate<int> 8)
        let num = 1
        array.[0].[0] <- 0
        array.[0].[1] <- V2
        array.[0].[2] <- V2
        array.[0].[3] <- V2
        array.[0].[4] <- 0
        array.[0].[5] <- 0
        for i in 0 .. num - 1 do
            for j in 8 .. this.laudankoko + 8 - 1 do
                for k in 8 .. this.laudankoko + 8 - 1 do
                    if B.[k].[j] = array.[i].[0] && B.[k + 1].[j] = array.[i].[1] && B.[k + 2].[j] = array.[i].[2] && B.[k + 3].[j] = array.[i].[3] && B.[k + 4].[j] = array.[i].[4] && B.[k + 5].[j] = array.[i].[5] then
                        B.[k + 4].[j] <- V2
                        B.[k + 5].[j] <- V2
                        let num2 = this.pikahaku(B, V2, 2)
                        B.[k + 4].[j] <- 0
                        B.[k + 5].[j] <- 0
                        if num2 = 9 then
                            PRIORITY.[k + 4].[j] <- 1000
                    if B.[k].[j] = array.[i].[0] && B.[k].[j + 1] = array.[i].[1] && B.[k].[j + 2] = array.[i].[2] && B.[k].[j + 3] = array.[i].[3] && B.[k].[j + 4] = array.[i].[4] && B.[k].[j + 5] = array.[i].[5] then
                        B.[k].[j + 4] <- V2
                        B.[k].[j + 5] <- V2
                        let num3 = this.pikahaku(B, V2, 2)
                        B.[k].[j + 4] <- 0
                        B.[k].[j + 5] <- 0
                        if num3 = 9 then
                            PRIORITY.[k].[j + 4] <- 1000
                    if B.[k].[j] = array.[i].[0] && B.[k + 1].[j + 1] = array.[i].[1] && B.[k + 2].[j + 2] = array.[i].[2] && B.[k + 3].[j + 3] = array.[i].[3] && B.[k + 4].[j + 4] = array.[i].[4] && B.[k + 5].[j + 5] = array.[i].[5] then
                        B.[k + 4].[j + 4] <- V2
                        B.[k + 5].[j + 5] <- V2
                        let num4 = this.pikahaku(B, V2, 2)
                        B.[k + 4].[j + 4] <- 0
                        B.[k + 5].[j + 5] <- 0
                        if num4 = 9 then
                            PRIORITY.[k + 4].[j + 4] <- 1000
                    if B.[k].[j] = array.[i].[0] && B.[k + 1].[j - 1] = array.[i].[1] && B.[k + 2].[j - 2] = array.[i].[2] && B.[k + 3].[j - 3] = array.[i].[3] && B.[k + 4].[j - 4] = array.[i].[4] && B.[k + 5].[j - 5] = array.[i].[5] then
                        B.[k + 4].[j - 4] <- V2
                        B.[k + 5].[j - 5] <- V2
                        let num5 = this.pikahaku(B, V2, 2)
                        B.[k + 4].[j - 4] <- 0
                        B.[k + 5].[j - 5] <- 0
                        if num5 = 9 then
                            PRIORITY.[k + 4].[j - 4] <- 1000
                    if B.[k].[j] = array.[i].[0] && B.[k - 1].[j] = array.[i].[1] && B.[k - 2].[j] = array.[i].[2] && B.[k - 3].[j] = array.[i].[3] && B.[k - 4].[j] = array.[i].[4] && B.[k - 5].[j] = array.[i].[5] then
                        B.[k - 4].[j] <- V2
                        B.[k - 5].[j] <- V2
                        let num6 = this.pikahaku(B, V2, 2)
                        B.[k - 4].[j] <- 0
                        B.[k - 5].[j] <- 0
                        if num6 = 9 then
                            PRIORITY.[k - 4].[j] <- 1000
                    if B.[k].[j] = array.[i].[0] && B.[k].[j - 1] = array.[i].[1] && B.[k].[j - 2] = array.[i].[2] && B.[k].[j - 3] = array.[i].[3] && B.[k].[j - 4] = array.[i].[4] && B.[k].[j - 5] = array.[i].[5] then
                        B.[k].[j - 4] <- V2
                        B.[k].[j - 5] <- V2
                        let num7 = this.pikahaku(B, V2, 2)
                        B.[k].[j - 4] <- 0
                        B.[k].[j - 5] <- 0
                        if num7 = 9 then
                            PRIORITY.[k].[j - 4] <- 1000
                    if B.[k].[j] = array.[i].[0] && B.[k - 1].[j - 1] = array.[i].[1] && B.[k - 2].[j - 2] = array.[i].[2] && B.[k - 3].[j - 3] = array.[i].[3] && B.[k - 4].[j - 4] = array.[i].[4] && B.[k - 5].[j - 5] = array.[i].[5] then
                        B.[k - 4].[j - 4] <- V2
                        B.[k - 5].[j - 5] <- V2
                        let num8 = this.pikahaku(B, V2, 2)
                        B.[k - 4].[j - 4] <- 0
                        B.[k - 5].[j - 5] <- 0
                        if num8 = 9 then
                            PRIORITY.[k - 4].[j - 4] <- 1000
                    if B.[k].[j] = array.[i].[0] && B.[k - 1].[j + 1] = array.[i].[1] && B.[k - 2].[j + 2] = array.[i].[2] && B.[k - 3].[j + 3] = array.[i].[3] && B.[k - 4].[j + 4] = array.[i].[4] && B.[k - 5].[j + 5] = array.[i].[5] then
                        B.[k - 4].[j + 4] <- V2
                        B.[k - 5].[j + 5] <- V2
                        let num9 = this.pikahaku(B, V2, 2)
                        B.[k - 4].[j + 4] <- 0
                        B.[k - 5].[j + 5] <- 0
                        if num9 = 9 then
                            PRIORITY.[k - 4].[j + 4] <- 1000
        0


    member this.TarkistaVoitto() : int =
        try
            let array = Array.init 64 (fun _ -> Array.zeroCreate<int> 64)
            let num = 1
            let num2 = 40
            let num3 = this.vuoro
            for i in 0 .. 63 do
                for j in 0 .. 63 do
                    array.[j].[i] <- 0
            for k in 0 .. 39 do
                for l in 0 .. 39 do
                    array.[l + 8].[k + 8] <- this.A.[l].[k]
            for m in 8 .. num2 + 8 - 1 do
                for n in 8 .. num2 + 8 - 1 do
                    let mutable num4 = 0
                    while array.[n + num4].[m] = num3 do num4 <- num4 + 1
                    if num4 > 4 && (num = 1 || (num4 = 5 && array.[n - 1].[m] <> num3)) then
                        this.suora1x <- n - 8
                        this.suora1y <- m - 8
                        this.suora2x <- n + num4 - 9
                        this.suora2y <- m - 8
                        raise (ReturnInt num3)
            for num5 in 8 .. num2 + 8 - 1 do
                for num6 in 8 .. num2 + 8 - 1 do
                    let mutable num7 = 0
                    while array.[num6].[num5 + num7] = num3 do num7 <- num7 + 1
                    if num7 > 4 && (num = 1 || (num7 = 5 && array.[num6].[num5 - 1] <> num3)) then
                        this.suora1x <- num6 - 8
                        this.suora1y <- num5 - 8
                        this.suora2x <- num6 - 8
                        this.suora2y <- num5 + num7 - 9
                        raise (ReturnInt num3)
            for num8 in 8 .. num2 + 8 - 1 do
                for num9 in 8 .. num2 + 8 - 1 do
                    let mutable num10 = 0
                    while array.[num9 + num10].[num8 + num10] = num3 do num10 <- num10 + 1
                    if num10 > 4 && (num = 1 || (num10 = 5 && array.[num9 - 1].[num8 - 1] <> num3)) then
                        this.suora1x <- num9 - 8
                        this.suora1y <- num8 - 8
                        this.suora2x <- num9 + num10 - 9
                        this.suora2y <- num8 + num10 - 9
                        raise (ReturnInt num3)
            for num11 in 8 .. num2 + 8 - 1 do
                for num12 in 8 .. num2 + 8 - 1 do
                    let mutable num13 = 0
                    while array.[num12 - num13].[num11 + num13] = num3 do num13 <- num13 + 1
                    if num13 > 4 && (num = 1 || (num13 = 5 && array.[num12 + 1].[num11 - 1] <> num3)) then
                        this.suora1x <- num12 - 8
                        this.suora1y <- num11 - 8
                        this.suora2x <- num12 - num13 - 7
                        this.suora2y <- num11 + num13 - 9
                        raise (ReturnInt num3)
            0
        with ReturnInt v -> v


    member this.lisaapriority(PRIORITY: int[][], AA: int[][], C: int, D: int, suunta: int, B: int[][], rivi: int, PISTEET: int[]) =
        try
            if ((AA.[rivi].[0] = 3 && B.[C].[D] = 0) || AA.[rivi].[0] = 5 || B.[C].[D] = AA.[rivi].[0]) && ((AA.[rivi].[1] = 3 && B.[C + 1].[D] = 0) || AA.[rivi].[1] = 5 || B.[C + 1].[D] = AA.[rivi].[1]) && ((AA.[rivi].[2] = 3 && B.[C + 2].[D] = 0) || AA.[rivi].[2] = 5 || B.[C + 2].[D] = AA.[rivi].[2]) && ((AA.[rivi].[3] = 3 && B.[C + 3].[D] = 0) || AA.[rivi].[3] = 5 || B.[C + 3].[D] = AA.[rivi].[3]) && ((AA.[rivi].[4] = 3 && B.[C + 4].[D] = 0) || AA.[rivi].[4] = 5 || B.[C + 4].[D] = AA.[rivi].[4]) && ((AA.[rivi].[5] = 3 && B.[C + 5].[D] = 0) || AA.[rivi].[5] = 5 || B.[C + 5].[D] = AA.[rivi].[5]) && ((AA.[rivi].[6] = 3 && B.[C + 6].[D] = 0) || AA.[rivi].[6] = 5 || B.[C + 6].[D] = AA.[rivi].[6]) && ((AA.[rivi].[7] = 3 && B.[C + 7].[D] = 0) || AA.[rivi].[7] = 5 || B.[C + 7].[D] = AA.[rivi].[7]) then
                for i in 0 .. 7 do
                    if AA.[rivi].[i] = 0 && B.[C + i].[D] = 0 && C + i > -1 && C + i < this.laudankoko + 8 && D > -1 && D < this.laudankoko + 8 then
                        PRIORITY.[C + i].[D] <- PRIORITY.[C + i].[D] + PISTEET.[rivi]
            if ((AA.[rivi].[0] = 3 && B.[C].[D] = 0) || AA.[rivi].[0] = 5 || B.[C].[D] = AA.[rivi].[0]) && ((AA.[rivi].[1] = 3 && B.[C].[D + 1] = 0) || AA.[rivi].[1] = 5 || B.[C].[D + 1] = AA.[rivi].[1]) && ((AA.[rivi].[2] = 3 && B.[C].[D + 2] = 0) || AA.[rivi].[2] = 5 || B.[C].[D + 2] = AA.[rivi].[2]) && ((AA.[rivi].[3] = 3 && B.[C].[D + 3] = 0) || AA.[rivi].[3] = 5 || B.[C].[D + 3] = AA.[rivi].[3]) && ((AA.[rivi].[4] = 3 && B.[C].[D + 4] = 0) || AA.[rivi].[4] = 5 || B.[C].[D + 4] = AA.[rivi].[4]) && ((AA.[rivi].[5] = 3 && B.[C].[D + 5] = 0) || AA.[rivi].[5] = 5 || B.[C].[D + 5] = AA.[rivi].[5]) && ((AA.[rivi].[6] = 3 && B.[C].[D + 6] = 0) || AA.[rivi].[6] = 5 || B.[C].[D + 6] = AA.[rivi].[6]) && ((AA.[rivi].[7] = 3 && B.[C].[D + 7] = 0) || AA.[rivi].[7] = 5 || B.[C].[D + 7] = AA.[rivi].[7]) then
                for j in 0 .. 7 do
                    if AA.[rivi].[j] = 0 && B.[C].[D + j] = 0 && C > -1 && C < this.laudankoko + 8 && D + j > -1 && D + j < this.laudankoko + 8 then
                        PRIORITY.[C].[D + j] <- PRIORITY.[C].[D + j] + PISTEET.[rivi]
            if ((AA.[rivi].[0] = 3 && B.[C].[D] = 0) || AA.[rivi].[0] = 5 || B.[C].[D] = AA.[rivi].[0]) && ((AA.[rivi].[1] = 3 && B.[C + 1].[D + 1] = 0) || AA.[rivi].[1] = 5 || B.[C + 1].[D + 1] = AA.[rivi].[1]) && ((AA.[rivi].[2] = 3 && B.[C + 2].[D + 2] = 0) || AA.[rivi].[2] = 5 || B.[C + 2].[D + 2] = AA.[rivi].[2]) && ((AA.[rivi].[3] = 3 && B.[C + 3].[D + 3] = 0) || AA.[rivi].[3] = 5 || B.[C + 3].[D + 3] = AA.[rivi].[3]) && ((AA.[rivi].[4] = 3 && B.[C + 4].[D + 4] = 0) || AA.[rivi].[4] = 5 || B.[C + 4].[D + 4] = AA.[rivi].[4]) && ((AA.[rivi].[5] = 3 && B.[C + 5].[D + 5] = 0) || AA.[rivi].[5] = 5 || B.[C + 5].[D + 5] = AA.[rivi].[5]) && ((AA.[rivi].[6] = 3 && B.[C + 6].[D + 6] = 0) || AA.[rivi].[6] = 5 || B.[C + 6].[D + 6] = AA.[rivi].[6]) && ((AA.[rivi].[7] = 3 && B.[C + 7].[D + 7] = 0) || AA.[rivi].[7] = 5 || B.[C + 7].[D + 7] = AA.[rivi].[7]) then
                for k in 0 .. 7 do
                    if AA.[rivi].[k] = 0 && B.[C + k].[D + k] = 0 && C + k > -1 && C + k < this.laudankoko + 8 && D + k > -1 && D + k < this.laudankoko + 8 then
                        PRIORITY.[C + k].[D + k] <- PRIORITY.[C + k].[D + k] + PISTEET.[rivi]
            if ((AA.[rivi].[0] = 3 && B.[C].[D] = 0) || AA.[rivi].[0] = 5 || B.[C].[D] = AA.[rivi].[0]) && ((AA.[rivi].[1] = 3 && B.[C + 1].[D - 1] = 0) || AA.[rivi].[1] = 5 || B.[C + 1].[D - 1] = AA.[rivi].[1]) && ((AA.[rivi].[2] = 3 && B.[C + 2].[D - 2] = 0) || AA.[rivi].[2] = 5 || B.[C + 2].[D - 2] = AA.[rivi].[2]) && ((AA.[rivi].[3] = 3 && B.[C + 3].[D - 3] = 0) || AA.[rivi].[3] = 5 || B.[C + 3].[D - 3] = AA.[rivi].[3]) && ((AA.[rivi].[4] = 3 && B.[C + 4].[D - 4] = 0) || AA.[rivi].[4] = 5 || B.[C + 4].[D - 4] = AA.[rivi].[4]) && ((AA.[rivi].[5] = 3 && B.[C + 5].[D - 5] = 0) || AA.[rivi].[5] = 5 || B.[C + 5].[D - 5] = AA.[rivi].[5]) && ((AA.[rivi].[6] = 3 && B.[C + 6].[D - 6] = 0) || AA.[rivi].[6] = 5 || B.[C + 6].[D - 6] = AA.[rivi].[6]) && ((AA.[rivi].[7] = 3 && B.[C + 7].[D - 7] = 0) || AA.[rivi].[7] = 5 || B.[C + 7].[D - 7] = AA.[rivi].[7]) then
                for l in 0 .. 7 do
                    if AA.[rivi].[l] = 0 && B.[C + l].[D - l] = 0 && C + l > -1 && C + l < this.laudankoko + 8 && D - l > -1 && D - l < this.laudankoko + 8 then
                        PRIORITY.[C + l].[D - l] <- PRIORITY.[C + l].[D - l] + PISTEET.[rivi]
            if ((AA.[rivi].[0] = 3 && B.[C].[D] = 0) || AA.[rivi].[0] = 5 || B.[C].[D] = AA.[rivi].[0]) && ((AA.[rivi].[1] = 3 && B.[C - 1].[D] = 0) || AA.[rivi].[1] = 5 || B.[C - 1].[D] = AA.[rivi].[1]) && ((AA.[rivi].[2] = 3 && B.[C - 2].[D] = 0) || AA.[rivi].[2] = 5 || B.[C - 2].[D] = AA.[rivi].[2]) && ((AA.[rivi].[3] = 3 && B.[C - 3].[D] = 0) || AA.[rivi].[3] = 5 || B.[C - 3].[D] = AA.[rivi].[3]) && ((AA.[rivi].[4] = 3 && B.[C - 4].[D] = 0) || AA.[rivi].[4] = 5 || B.[C - 4].[D] = AA.[rivi].[4]) && ((AA.[rivi].[5] = 3 && B.[C - 5].[D] = 0) || AA.[rivi].[5] = 5 || B.[C - 5].[D] = AA.[rivi].[5]) && ((AA.[rivi].[6] = 3 && B.[C - 6].[D] = 0) || AA.[rivi].[6] = 5 || B.[C - 6].[D] = AA.[rivi].[6]) && ((AA.[rivi].[7] = 3 && B.[C - 7].[D] = 0) || AA.[rivi].[7] = 5 || B.[C - 7].[D] = AA.[rivi].[7]) then
                for m in 0 .. 7 do
                    if AA.[rivi].[m] = 0 && B.[C - m].[D] = 0 && C - m > -1 && C - m < this.laudankoko + 8 && D > -1 && D < this.laudankoko + 8 then
                        PRIORITY.[C - m].[D] <- PRIORITY.[C - m].[D] + PISTEET.[rivi]
            if ((AA.[rivi].[0] = 3 && B.[C].[D] = 0) || AA.[rivi].[0] = 5 || B.[C].[D] = AA.[rivi].[0]) && ((AA.[rivi].[1] = 3 && B.[C].[D - 1] = 0) || AA.[rivi].[1] = 5 || B.[C].[D - 1] = AA.[rivi].[1]) && ((AA.[rivi].[2] = 3 && B.[C].[D - 2] = 0) || AA.[rivi].[2] = 5 || B.[C].[D - 2] = AA.[rivi].[2]) && ((AA.[rivi].[3] = 3 && B.[C].[D - 3] = 0) || AA.[rivi].[3] = 5 || B.[C].[D - 3] = AA.[rivi].[3]) && ((AA.[rivi].[4] = 3 && B.[C].[D - 4] = 0) || AA.[rivi].[4] = 5 || B.[C].[D - 4] = AA.[rivi].[4]) && ((AA.[rivi].[5] = 3 && B.[C].[D - 5] = 0) || AA.[rivi].[5] = 5 || B.[C].[D - 5] = AA.[rivi].[5]) && ((AA.[rivi].[6] = 3 && B.[C].[D - 6] = 0) || AA.[rivi].[6] = 5 || B.[C].[D - 6] = AA.[rivi].[6]) && ((AA.[rivi].[7] = 3 && B.[C].[D - 7] = 0) || AA.[rivi].[7] = 5 || B.[C].[D - 7] = AA.[rivi].[7]) then
                for n in 0 .. 7 do
                    if AA.[rivi].[n] = 0 && B.[C].[D - n] = 0 && C > -1 && C < this.laudankoko + 8 && D - n > -1 && D - n < this.laudankoko + 8 then
                        PRIORITY.[C].[D - n] <- PRIORITY.[C].[D - n] + PISTEET.[rivi]
            if ((AA.[rivi].[0] = 3 && B.[C].[D] = 0) || AA.[rivi].[0] = 5 || B.[C].[D] = AA.[rivi].[0]) && ((AA.[rivi].[1] = 3 && B.[C - 1].[D - 1] = 0) || AA.[rivi].[1] = 5 || B.[C - 1].[D - 1] = AA.[rivi].[1]) && ((AA.[rivi].[2] = 3 && B.[C - 2].[D - 2] = 0) || AA.[rivi].[2] = 5 || B.[C - 2].[D - 2] = AA.[rivi].[2]) && ((AA.[rivi].[3] = 3 && B.[C - 3].[D - 3] = 0) || AA.[rivi].[3] = 5 || B.[C - 3].[D - 3] = AA.[rivi].[3]) && ((AA.[rivi].[4] = 3 && B.[C - 4].[D - 4] = 0) || AA.[rivi].[4] = 5 || B.[C - 4].[D - 4] = AA.[rivi].[4]) && ((AA.[rivi].[5] = 3 && B.[C - 5].[D - 5] = 0) || AA.[rivi].[5] = 5 || B.[C - 5].[D - 5] = AA.[rivi].[5]) && ((AA.[rivi].[6] = 3 && B.[C - 6].[D - 6] = 0) || AA.[rivi].[6] = 5 || B.[C - 6].[D - 6] = AA.[rivi].[6]) && ((AA.[rivi].[7] = 3 && B.[C - 7].[D - 7] = 0) || AA.[rivi].[7] = 5 || B.[C - 7].[D - 7] = AA.[rivi].[7]) then
                for num in 0 .. 7 do
                    if AA.[rivi].[num] = 0 && B.[C - num].[D - num] = 0 && C - num > -1 && C - num < this.laudankoko + 8 && D - num > -1 && D - num < this.laudankoko + 8 then
                        PRIORITY.[C - num].[D - num] <- PRIORITY.[C - num].[D - num] + PISTEET.[rivi]
            if ((AA.[rivi].[0] <> 3 || B.[C].[D] <> 0) && AA.[rivi].[0] <> 5 && B.[C].[D] <> AA.[rivi].[0]) || ((AA.[rivi].[1] <> 3 || B.[C - 1].[D + 1] <> 0) && AA.[rivi].[1] <> 5 && B.[C - 1].[D + 1] <> AA.[rivi].[1]) || ((AA.[rivi].[2] <> 3 || B.[C - 2].[D + 2] <> 0) && AA.[rivi].[2] <> 5 && B.[C - 2].[D + 2] <> AA.[rivi].[2]) || ((AA.[rivi].[3] <> 3 || B.[C - 3].[D + 3] <> 0) && AA.[rivi].[3] <> 5 && B.[C - 3].[D + 3] <> AA.[rivi].[3]) || ((AA.[rivi].[4] <> 3 || B.[C - 4].[D + 4] <> 0) && AA.[rivi].[4] <> 5 && B.[C - 4].[D + 4] <> AA.[rivi].[4]) || ((AA.[rivi].[5] <> 3 || B.[C - 5].[D + 5] <> 0) && AA.[rivi].[5] <> 5 && B.[C - 5].[D + 5] <> AA.[rivi].[5]) || ((AA.[rivi].[6] <> 3 || B.[C - 6].[D + 6] <> 0) && AA.[rivi].[6] <> 5 && B.[C - 6].[D + 6] <> AA.[rivi].[6]) || ((AA.[rivi].[7] <> 3 || B.[C - 7].[D + 7] <> 0) && AA.[rivi].[7] <> 5 && B.[C - 7].[D + 7] <> AA.[rivi].[7]) then
                raise ReturnUnit
            for num2 in 0 .. 7 do
                if AA.[rivi].[num2] = 0 && B.[C - num2].[D + num2] = 0 && C - num2 > -1 && C - num2 < this.laudankoko + 8 && D + num2 > -1 && D + num2 < this.laudankoko + 8 then
                    PRIORITY.[C - num2].[D + num2] <- PRIORITY.[C - num2].[D + num2] + PISTEET.[rivi]
            ()
        with ReturnUnit -> ()


    member this.rekursiohaku(syvyys: int, syvyysmax: int, AA: int[][], BB: int[][], B: int[][], PRIORITY: int[][], rivi0: int, suunta0: int, X: int, Y: int, puoli2: int) : int =
        try
            if syvyys > syvyysmax then raise (ReturnInt 0)
            let mutable rivi0 = rivi0
            let mutable suunta0 = suunta0
            let mutable X = X
            let mutable Y = Y
            let mutable syvyysmax = syvyysmax
            for i in 0 .. 11 do
                if syvyys = 0 then rivi0 <- i
                for j in 8 .. this.laudankoko + 8 - 1 do
                    if syvyys = 0 then Y <- j
                    for k in 8 .. this.laudankoko + 8 - 1 do
                        if syvyys = 0 then X <- k
                        if B.[k].[j] = AA.[i].[0] && B.[k + 1].[j] = AA.[i].[1] && B.[k + 2].[j] = AA.[i].[2] && B.[k + 3].[j] = AA.[i].[3] && B.[k + 4].[j] = AA.[i].[4] then
                            if i < 3 && syvyys > 0 then
                                let num = this.pikahaku(B, puoli2, 1)
                                if num = 9 then raise (ReturnInt 0)
                                syvyysmax <- syvyys - 1
                                this.haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 0
                            for l in 0 .. 4 do
                                if BB.[i].[l] <> 5 then
                                    B.[k + l].[j] <- BB.[i].[l]
                            this.rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for m in 0 .. 4 do
                                if BB.[i].[m] <> 5 then
                                    B.[k + m].[j] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k].[j + 1] = AA.[i].[1] && B.[k].[j + 2] = AA.[i].[2] && B.[k].[j + 3] = AA.[i].[3] && B.[k].[j + 4] = AA.[i].[4] then
                            if i < 3 && syvyys > 0 then
                                let num2 = this.pikahaku(B, puoli2, 1)
                                if num2 = 9 then raise (ReturnInt 0)
                                syvyysmax <- syvyys - 1
                                this.haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 1
                            for n in 0 .. 4 do
                                if BB.[i].[n] <> 5 then
                                    B.[k].[j + n] <- BB.[i].[n]
                            this.rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num3 in 0 .. 4 do
                                if BB.[i].[num3] <> 5 then
                                    B.[k].[j + num3] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k + 1].[j + 1] = AA.[i].[1] && B.[k + 2].[j + 2] = AA.[i].[2] && B.[k + 3].[j + 3] = AA.[i].[3] && B.[k + 4].[j + 4] = AA.[i].[4] then
                            if i < 3 && syvyys > 0 then
                                let num4 = this.pikahaku(B, puoli2, 1)
                                if num4 = 9 then raise (ReturnInt 0)
                                syvyysmax <- syvyys - 1
                                this.haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 2
                            for num5 in 0 .. 4 do
                                if BB.[i].[num5] <> 5 then
                                    B.[k + num5].[j + num5] <- BB.[i].[num5]
                            this.rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num6 in 0 .. 4 do
                                if BB.[i].[num6] <> 5 then
                                    B.[k + num6].[j + num6] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k + 1].[j - 1] = AA.[i].[1] && B.[k + 2].[j - 2] = AA.[i].[2] && B.[k + 3].[j - 3] = AA.[i].[3] && B.[k + 4].[j - 4] = AA.[i].[4] then
                            if i < 3 && syvyys > 0 then
                                let num7 = this.pikahaku(B, puoli2, 1)
                                if num7 = 9 then raise (ReturnInt 0)
                                syvyysmax <- syvyys - 1
                                this.haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 3
                            for num8 in 0 .. 4 do
                                if BB.[i].[num8] <> 5 then
                                    B.[k + num8].[j - num8] <- BB.[i].[num8]
                            this.rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num9 in 0 .. 4 do
                                if BB.[i].[num9] <> 5 then
                                    B.[k + num9].[j - num9] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k - 1].[j] = AA.[i].[1] && B.[k - 2].[j] = AA.[i].[2] && B.[k - 3].[j] = AA.[i].[3] && B.[k - 4].[j] = AA.[i].[4] then
                            if i < 3 && syvyys > 0 then
                                let num10 = this.pikahaku(B, puoli2, 1)
                                if num10 = 9 then raise (ReturnInt 0)
                                syvyysmax <- syvyys - 1
                                this.haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 4
                            for num11 in 0 .. 4 do
                                if BB.[i].[num11] <> 5 then
                                    B.[k - num11].[j] <- BB.[i].[num11]
                            this.rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num12 in 0 .. 4 do
                                if BB.[i].[num12] <> 5 then
                                    B.[k - num12].[j] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k].[j - 1] = AA.[i].[1] && B.[k].[j - 2] = AA.[i].[2] && B.[k].[j - 3] = AA.[i].[3] && B.[k].[j - 4] = AA.[i].[4] then
                            if i < 3 && syvyys > 0 then
                                let num13 = this.pikahaku(B, puoli2, 1)
                                if num13 = 9 then raise (ReturnInt 0)
                                syvyysmax <- syvyys - 1
                                this.haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 5
                            for num14 in 0 .. 4 do
                                if BB.[i].[num14] <> 5 then
                                    B.[k].[j - num14] <- BB.[i].[num14]
                            this.rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num15 in 0 .. 4 do
                                if BB.[i].[num15] <> 5 then
                                    B.[k].[j - num15] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k - 1].[j - 1] = AA.[i].[1] && B.[k - 2].[j - 2] = AA.[i].[2] && B.[k - 3].[j - 3] = AA.[i].[3] && B.[k - 4].[j - 4] = AA.[i].[4] then
                            if i < 3 && syvyys > 0 then
                                let num16 = this.pikahaku(B, puoli2, 1)
                                if num16 = 9 then raise (ReturnInt 0)
                                syvyysmax <- syvyys - 1
                                this.haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 6
                            for num17 in 0 .. 4 do
                                if BB.[i].[num17] <> 5 then
                                    B.[k - num17].[j - num17] <- BB.[i].[num17]
                            this.rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num18 in 0 .. 4 do
                                if BB.[i].[num18] <> 5 then
                                    B.[k - num18].[j - num18] <- 0
                        if not (B.[k].[j] <> AA.[i].[0] || B.[k - 1].[j + 1] <> AA.[i].[1] || B.[k - 2].[j + 2] <> AA.[i].[2] || B.[k - 3].[j + 3] <> AA.[i].[3] || B.[k - 4].[j + 4] <> AA.[i].[4]) then (
                            if i < 3 && syvyys > 0 then
                                let num19 = this.pikahaku(B, puoli2, 1)
                                if num19 = 9 then raise (ReturnInt 0)
                                syvyysmax <- syvyys - 1
                                this.haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 7
                            for num20 in 0 .. 4 do
                                if BB.[i].[num20] <> 5 then
                                    B.[k - num20].[j + num20] <- BB.[i].[num20]
                            this.rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num21 in 0 .. 4 do
                                if BB.[i].[num21] <> 5 then
                                    B.[k - num21].[j + num21] <- 0
                        )
            0
        with ReturnInt v -> v
    member this.rekursiohaku2(syvyys: int, syvyysmax: int, AA: int[][], BB: int[][], B: int[][], PRIORITY: int[][], rivi0: int, suunta0: int, X: int, Y: int, puoli2: int) : int =
        try
            if syvyys > syvyysmax then raise (ReturnInt 0)
            let mutable rivi0 = rivi0
            let mutable suunta0 = suunta0
            let mutable X = X
            let mutable Y = Y
            let mutable syvyysmax = syvyysmax
            for i in 0 .. 6 do
                if syvyys = 0 then rivi0 <- i
                for j in 8 .. this.laudankoko + 8 - 1 do
                    if syvyys = 0 then Y <- j
                    for k in 8 .. this.laudankoko + 8 - 1 do
                        if syvyys = 0 then X <- k
                        if B.[k].[j] = AA.[i].[0] && B.[k + 1].[j] = AA.[i].[1] && B.[k + 2].[j] = AA.[i].[2] && B.[k + 3].[j] = AA.[i].[3] && B.[k + 4].[j] = AA.[i].[4] && B.[k + 5].[j] = AA.[i].[5] then
                            if i < 2 && syvyys > 0 then
                                syvyysmax <- syvyys - 1
                                this.haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 0
                            for l in 0 .. 5 do
                                if BB.[i].[l] <> 5 then
                                    B.[k + l].[j] <- BB.[i].[l]
                            this.rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for m in 0 .. 5 do
                                if BB.[i].[m] <> 5 then
                                    B.[k + m].[j] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k].[j + 1] = AA.[i].[1] && B.[k].[j + 2] = AA.[i].[2] && B.[k].[j + 3] = AA.[i].[3] && B.[k].[j + 4] = AA.[i].[4] && B.[k].[j + 5] = AA.[i].[5] then
                            if i < 2 && syvyys > 0 then
                                syvyysmax <- syvyys - 1
                                this.haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 1
                            for n in 0 .. 5 do
                                if BB.[i].[n] <> 5 then
                                    B.[k].[j + n] <- BB.[i].[n]
                            this.rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num in 0 .. 5 do
                                if BB.[i].[num] <> 5 then
                                    B.[k].[j + num] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k + 1].[j + 1] = AA.[i].[1] && B.[k + 2].[j + 2] = AA.[i].[2] && B.[k + 3].[j + 3] = AA.[i].[3] && B.[k + 4].[j + 4] = AA.[i].[4] && B.[k + 5].[j + 5] = AA.[i].[5] then
                            if i < 2 && syvyys > 0 then
                                syvyysmax <- syvyys - 1
                                this.haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 2
                            for num2 in 0 .. 5 do
                                if BB.[i].[num2] <> 5 then
                                    B.[k + num2].[j + num2] <- BB.[i].[num2]
                            this.rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num3 in 0 .. 5 do
                                if BB.[i].[num3] <> 5 then
                                    B.[k + num3].[j + num3] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k + 1].[j - 1] = AA.[i].[1] && B.[k + 2].[j - 2] = AA.[i].[2] && B.[k + 3].[j - 3] = AA.[i].[3] && B.[k + 4].[j - 4] = AA.[i].[4] && B.[k + 5].[j - 5] = AA.[i].[5] then
                            if i < 2 && syvyys > 0 then
                                syvyysmax <- syvyys - 1
                                this.haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 3
                            for num4 in 0 .. 5 do
                                if BB.[i].[num4] <> 5 then
                                    B.[k + num4].[j - num4] <- BB.[i].[num4]
                            this.rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num5 in 0 .. 5 do
                                if BB.[i].[num5] <> 5 then
                                    B.[k + num5].[j - num5] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k - 1].[j] = AA.[i].[1] && B.[k - 2].[j] = AA.[i].[2] && B.[k - 3].[j] = AA.[i].[3] && B.[k - 4].[j] = AA.[i].[4] && B.[k - 5].[j] = AA.[i].[5] then
                            if i < 2 && syvyys > 0 then
                                syvyysmax <- syvyys - 1
                                this.haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 4
                            for num6 in 0 .. 5 do
                                if BB.[i].[num6] <> 5 then
                                    B.[k - num6].[j] <- BB.[i].[num6]
                            this.rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num7 in 0 .. 5 do
                                if BB.[i].[num7] <> 5 then
                                    B.[k - num7].[j] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k].[j - 1] = AA.[i].[1] && B.[k].[j - 2] = AA.[i].[2] && B.[k].[j - 3] = AA.[i].[3] && B.[k].[j - 4] = AA.[i].[4] && B.[k].[j - 5] = AA.[i].[5] then
                            if i < 2 && syvyys > 0 then
                                syvyysmax <- syvyys - 1
                                this.haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 5
                            for num8 in 0 .. 5 do
                                if BB.[i].[num8] <> 5 then
                                    B.[k].[j - num8] <- BB.[i].[num8]
                            this.rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num9 in 0 .. 5 do
                                if BB.[i].[num9] <> 5 then
                                    B.[k].[j - num9] <- 0
                        if B.[k].[j] = AA.[i].[0] && B.[k - 1].[j - 1] = AA.[i].[1] && B.[k - 2].[j - 2] = AA.[i].[2] && B.[k - 3].[j - 3] = AA.[i].[3] && B.[k - 4].[j - 4] = AA.[i].[4] && B.[k - 5].[j - 5] = AA.[i].[5] then
                            if i < 2 && syvyys > 0 then
                                syvyysmax <- syvyys - 1
                                this.haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 6
                            for num10 in 0 .. 5 do
                                if BB.[i].[num10] <> 5 then
                                    B.[k - num10].[j - num10] <- BB.[i].[num10]
                            this.rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num11 in 0 .. 5 do
                                if BB.[i].[num11] <> 5 then
                                    B.[k - num11].[j - num11] <- 0
                        if not (B.[k].[j] <> AA.[i].[0] || B.[k - 1].[j + 1] <> AA.[i].[1] || B.[k - 2].[j + 2] <> AA.[i].[2] || B.[k - 3].[j + 3] <> AA.[i].[3] || B.[k - 4].[j + 4] <> AA.[i].[4] || B.[k - 5].[j + 5] <> AA.[i].[5]) then (
                            if i < 2 && syvyys > 0 then
                                syvyysmax <- syvyys - 1
                                this.haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY)
                                raise (ReturnInt syvyys)
                            if syvyys = 0 then suunta0 <- 7
                            for num12 in 0 .. 5 do
                                if BB.[i].[num12] <> 5 then
                                    B.[k - num12].[j + num12] <- BB.[i].[num12]
                            this.rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2) |> ignore
                            for num13 in 0 .. 5 do
                                if BB.[i].[num13] <> 5 then
                                    B.[k - num13].[j + num13] <- 0
                        )
            0
        with ReturnInt v -> v


    member this.alotus() : int =
        try
            let array = Array.init 64 (fun _ -> Array.zeroCreate<int> 64)
            for k in 0 .. this.laudankoko - 1 do
                for l in 0 .. this.laudankoko - 1 do
                    array.[l + 8].[k + 8] <- this.A.[l].[k]
            if this.kokovuoro = 0 then
                this.PRIORITY.[this.laudankoko / 2 + 8].[this.laudankoko / 2 + 8] <- 500
                raise (ReturnInt 1)
            elif this.kokovuoro = 1 then
                if this.alotyyli = 2 || this.alotyyli = 3 then
                    for num29 in 8 .. this.laudankoko + 8 - 1 do
                        for num30 in 8 .. this.laudankoko + 8 - 1 do
                            if array.[num30].[num29] = 1 then
                                if num30 < this.laudankoko / 2 + 7 then
                                    if num29 < this.laudankoko / 2 + 7 then
                                        this.PRIORITY.[num30 + 1].[num29 + 1] <- 500
                                    else
                                        this.PRIORITY.[num30 + 1].[num29 - 1] <- 500
                                elif num29 < this.laudankoko / 2 + 7 then
                                    this.PRIORITY.[num30 - 1].[num29 + 1] <- 500
                                else
                                    this.PRIORITY.[num30 - 1].[num29 - 1] <- 500
                    raise (ReturnInt 1)
                if this.alotyyli = 4 then
                    for num31 in 8 .. this.laudankoko + 8 - 1 do
                        for num32 in 8 .. this.laudankoko + 8 - 1 do
                            if array.[num32].[num31] = 1 then
                                if num32 < this.laudankoko / 2 + 7 then
                                    if num31 < this.laudankoko / 2 + 7 then
                                        this.PRIORITY.[num32 + 2].[num31 + 1] <- 500
                                        this.PRIORITY.[num32 + 1].[num31 + 2] <- 500
                                    else
                                        this.PRIORITY.[num32 + 2].[num31 - 1] <- 500
                                        this.PRIORITY.[num32 + 1].[num31 - 2] <- 500
                                elif num31 < this.laudankoko / 2 + 7 then
                                    this.PRIORITY.[num32 - 2].[num31 + 1] <- 500
                                    this.PRIORITY.[num32 - 1].[num31 + 2] <- 500
                                else
                                    this.PRIORITY.[num32 - 2].[num31 - 1] <- 500
                                    this.PRIORITY.[num32 - 1].[num31 - 2] <- 500
                    raise (ReturnInt 1)
                raise (ReturnInt 0)
            elif this.kokovuoro = 2 then
                for num33 in 8 .. this.laudankoko + 8 - 1 do
                    for num34 in 8 .. this.laudankoko + 8 - 1 do
                        if array.[num34].[num33] = 1 then
                            if this.alotyyli = 2 then
                                if array.[num34 - 1].[num33 - 1] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34].[num33 - 1] = 2 then
                                    this.PRIORITY.[num34].[num33 - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 1].[num33 - 1] = 2 then
                                    this.PRIORITY.[num34 + 2].[num33 - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 - 1].[num33] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 1].[num33] = 2 then
                                    this.PRIORITY.[num34 + 2].[num33] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 - 1].[num33 + 1] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34].[num33 + 1] = 2 then
                                    this.PRIORITY.[num34].[num33 + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 1].[num33 + 1] = 2 then
                                    this.PRIORITY.[num34 + 2].[num33 + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 - 2].[num33 - 2] = 2 then
                                    this.PRIORITY.[num34 - 3].[num33 - 3] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 2].[num33 - 2] = 2 then
                                    this.PRIORITY.[num34 + 3].[num33 - 3] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 2].[num33 + 2] = 2 then
                                    this.PRIORITY.[num34 + 3].[num33 + 3] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 - 2].[num33 + 2] = 2 then
                                    this.PRIORITY.[num34 - 3].[num33 + 3] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34].[num33 - 2] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 - 2] <- 500
                                    this.PRIORITY.[num34 + 2].[num33 - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 - 2].[num33] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 - 2] <- 500
                                    this.PRIORITY.[num34 - 2].[num33 + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 2].[num33] = 2 then
                                    this.PRIORITY.[num34 + 2].[num33 - 2] <- 500
                                    this.PRIORITY.[num34 + 2].[num33 + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34].[num33 + 2] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 + 2] <- 500
                                    this.PRIORITY.[num34 + 2].[num33 + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 - 1].[num33 - 2] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 - 2] <- 500
                                    this.PRIORITY.[num34 + 1].[num33 - 1] <- 500
                                    this.PRIORITY.[num34 - 2].[num33] <- 100
                                    raise (ReturnInt 1)
                                if array.[num34 - 1].[num33 + 2] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 + 2] <- 500
                                    this.PRIORITY.[num34 + 1].[num33 + 1] <- 500
                                    this.PRIORITY.[num34].[num33 + 2] <- 100
                                    raise (ReturnInt 1)
                                if array.[num34 + 1].[num33 - 2] = 2 then
                                    this.PRIORITY.[num34 + 2].[num33 - 2] <- 500
                                    this.PRIORITY.[num34 - 1].[num33 - 1] <- 500
                                    this.PRIORITY.[num34 + 2].[num33] <- 100
                                    raise (ReturnInt 1)
                                if array.[num34 + 1].[num33 + 2] = 2 then
                                    this.PRIORITY.[num34 + 2].[num33 + 2] <- 500
                                    this.PRIORITY.[num34 - 1].[num33 + 1] <- 500
                                    this.PRIORITY.[num34].[num33 + 2] <- 100
                                    raise (ReturnInt 1)
                                if array.[num34 + 2].[num33 + 1] = 2 then
                                    this.PRIORITY.[num34 + 2].[num33 + 2] <- 500
                                    this.PRIORITY.[num34 + 1].[num33 - 1] <- 500
                                    this.PRIORITY.[num34].[num33 + 2] <- 100
                                    raise (ReturnInt 1)
                                if array.[num34 - 2].[num33 - 1] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 - 2] <- 500
                                    this.PRIORITY.[num34 - 1].[num33 + 1] <- 500
                                    this.PRIORITY.[num34].[num33 - 2] <- 100
                                    raise (ReturnInt 1)
                                if array.[num34 + 2].[num33 - 1] = 2 then
                                    this.PRIORITY.[num34 + 2].[num33 - 2] <- 500
                                    this.PRIORITY.[num34 + 1].[num33 + 1] <- 500
                                    this.PRIORITY.[num34].[num33 - 2] <- 100
                                    raise (ReturnInt 1)
                                if array.[num34 - 2].[num33 + 1] = 2 then
                                    this.PRIORITY.[num34 - 2].[num33 + 2] <- 500
                                    this.PRIORITY.[num34 - 1].[num33 - 1] <- 500
                                    this.PRIORITY.[num34].[num33 + 2] <- 100
                                    raise (ReturnInt 1)
                                raise (ReturnInt 0)
                            elif this.alotyyli = 3 then
                                if array.[num34 - 1].[num33 - 1] = 2 then
                                    this.PRIORITY.[num34 + 1].[num33 + 1] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 1].[num33 - 1] = 2 then
                                    this.PRIORITY.[num34 - 1].[num33 + 1] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 - 1].[num33 + 1] = 2 then
                                    this.PRIORITY.[num34 + 1].[num33 - 1] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 1].[num33 + 1] = 2 then
                                    this.PRIORITY.[num34 - 1].[num33 - 1] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34].[num33 - 1] = 2 then
                                    this.PRIORITY.[num34 - 1].[num33 + 1] <- 500
                                    this.PRIORITY.[num34 + 1].[num33 + 1] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 - 1].[num33] = 2 then
                                    this.PRIORITY.[num34 + 1].[num33 + 1] <- 500
                                    this.PRIORITY.[num34 + 1].[num33 - 1] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34 + 1].[num33] = 2 then
                                    this.PRIORITY.[num34 - 1].[num33 + 1] <- 500
                                    this.PRIORITY.[num34 - 1].[num33 - 1] <- 500
                                    raise (ReturnInt 1)
                                if array.[num34].[num33 + 1] = 2 then
                                    this.PRIORITY.[num34 + 1].[num33 - 1] <- 500
                                    this.PRIORITY.[num34 - 1].[num33 - 1] <- 500
                                    raise (ReturnInt 1)
                                for num37 in 8 .. this.laudankoko + 8 - 1 do
                                    for num38 in 8 .. this.laudankoko + 8 - 1 do
                                        if array.[num38].[num37] = 2 then
                                            if num38 > num34 then
                                                if num37 > num33 then
                                                    this.PRIORITY.[num34 - 1].[num33 - 1] <- 500
                                                    raise (ReturnInt 1)
                                                this.PRIORITY.[num34 - 1].[num33 + 1] <- 500
                                                raise (ReturnInt 1)
                                            if num37 > num33 then
                                                this.PRIORITY.[num34 + 1].[num33 - 1] <- 500
                                                raise (ReturnInt 1)
                                            this.PRIORITY.[num34 + 1].[num33 + 1] <- 500
                                            raise (ReturnInt 1)
                                raise (ReturnInt 0)
                            elif this.alotyyli = 4 then
                                for num35 in 8 .. this.laudankoko + 8 - 1 do
                                    for num36 in 8 .. this.laudankoko + 8 - 1 do
                                        if array.[num36].[num35] = 2 then
                                            if num36 > num34 then
                                                if num35 > num33 then
                                                    if num34 + 1 = num36 && num33 + 1 = num35 then
                                                        this.PRIORITY.[num34].[num33 - 2] <- 500
                                                        this.PRIORITY.[num34 - 2].[num33] <- 500
                                                        raise (ReturnInt 1)
                                                    this.PRIORITY.[num34 - 2].[num33 - 2] <- 500
                                                    raise (ReturnInt 1)
                                                if num34 + 1 = num36 && num33 - 1 = num35 then
                                                    this.PRIORITY.[num34].[num33 + 2] <- 500
                                                    this.PRIORITY.[num34 - 2].[num33] <- 500
                                                    raise (ReturnInt 1)
                                                this.PRIORITY.[num34 - 2].[num33 + 2] <- 500
                                                raise (ReturnInt 1)
                                            if num35 > num33 then
                                                if num34 - 1 = num36 && num33 + 1 = num35 then
                                                    this.PRIORITY.[num34].[num33 - 2] <- 500
                                                    this.PRIORITY.[num34 + 2].[num33] <- 500
                                                    raise (ReturnInt 1)
                                                this.PRIORITY.[num34 + 2].[num33 - 2] <- 500
                                                raise (ReturnInt 1)
                                            if num34 - 1 = num36 && num33 - 1 = num35 then
                                                this.PRIORITY.[num34].[num33 + 2] <- 500
                                                this.PRIORITY.[num34 + 2].[num33] <- 500
                                                raise (ReturnInt 1)
                                            this.PRIORITY.[num34 + 2].[num33 + 2] <- 500
                                            raise (ReturnInt 1)
                                raise (ReturnInt 0)
                            else
                                raise (ReturnInt 0)
                0
            elif this.kokovuoro = 3 then
                if this.alotyyli = 2 then
                    for num25 in 8 .. this.laudankoko + 8 - 1 do
                        for num26 in 8 .. this.laudankoko + 8 - 1 do
                            if array.[num26].[num25] = 2 && ((array.[num26 - 1].[num25 - 1] = 1 && array.[num26 + 1].[num25 + 1] = 1) || (array.[num26 + 1].[num25 - 1] = 1 && array.[num26 - 1].[num25 + 1] = 1)) then
                                this.PRIORITY.[num26].[num25 + 1] <- 500
                                this.PRIORITY.[num26].[num25 - 1] <- 500
                                this.PRIORITY.[num26 + 1].[num25] <- 500
                                this.PRIORITY.[num26 - 1].[num25] <- 500
                                raise (ReturnInt 1)
                if this.alotyyli <> 3 then
                    0
                else
                    for num27 in 8 .. this.laudankoko + 8 - 1 do
                        for num28 in 8 .. this.laudankoko + 8 - 1 do
                            if array.[num28].[num27] = 2 then
                                if (array.[num28 - 1].[num27 - 1] = 1 || array.[num28 - 1].[num27 - 1] = 0) && array.[num28 + 1].[num27 + 1] = 0 then
                                    this.PRIORITY.[num28 + 1].[num27 + 1] <- 500
                                    raise (ReturnInt 1)
                                if (array.[num28 - 1].[num27 + 1] = 1 || array.[num28 - 1].[num27 + 1] = 0) && array.[num28 + 1].[num27 - 1] = 0 then
                                    this.PRIORITY.[num28 + 1].[num27 - 1] <- 500
                                    raise (ReturnInt 1)
                                if (array.[num28 + 1].[num27 - 1] = 1 || array.[num28 + 1].[num27 - 1] = 0) && array.[num28 - 1].[num27 + 1] = 0 then
                                    this.PRIORITY.[num28 - 1].[num27 + 1] <- 500
                                    raise (ReturnInt 1)
                                if (array.[num28 + 1].[num27 + 1] = 1 || array.[num28 + 1].[num27 + 1] = 0) && array.[num28 - 1].[num27 - 1] = 0 then
                                    this.PRIORITY.[num28 - 1].[num27 - 1] <- 500
                                    raise (ReturnInt 1)
                    0
            elif this.kokovuoro = 4 then
                let num18 = this.pikahaku(array, 1, 2)
                if num18 = 9 then
                    raise (ReturnInt 0)
                for num19 in 8 .. this.laudankoko + 8 - 1 do
                    for num20 in 8 .. this.laudankoko + 8 - 1 do
                        if array.[num20].[num19] = 1 then
                            if this.alotyyli = 2 then
                                for num23 in 8 .. this.laudankoko + 8 - 1 do
                                    for num24 in 8 .. this.laudankoko + 8 - 1 do
                                        if array.[num24].[num23] = 2 then
                                            if array.[num20 + 2].[num19 + 2] = 1 && (num24 <> num20 + 1 || num23 <> num19 + 1) then
                                                if num24 > num23 then
                                                    if array.[num20].[num19 + 2] = 0 then
                                                        this.PRIORITY.[num20].[num19 + 2] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num20 + 2].[num19] = 0 then
                                                    this.PRIORITY.[num20 + 2].[num19] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num20 - 2].[num19 + 2] = 1 && (num24 <> num20 - 1 || num23 <> num19 + 1) then
                                                if num24 > this.laudankoko + 12 - num23 then
                                                    if array.[num20 - 2].[num19] = 0 then
                                                        this.PRIORITY.[num20 - 2].[num19] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num20].[num19 + 2] = 0 then
                                                    this.PRIORITY.[num20].[num19 + 2] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num20].[num19 + 2] = 1 && (num24 <> num20 || num23 <> num19 + 1) then
                                                if num24 > num20 then
                                                    if array.[num20 - 1].[num19 + 1] = 0 then
                                                        this.PRIORITY.[num20 - 1].[num19 + 1] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num20 + 1].[num19 + 1] = 0 then
                                                    this.PRIORITY.[num20 + 1].[num19 + 1] <- 500
                                                    raise (ReturnInt 1)
                                            if not (array.[num20 + 2].[num19] <> 1 || (num24 = num20 + 1 && num23 = num19)) then
                                                if num23 > num19 then
                                                    if array.[num20 + 1].[num19 - 1] = 0 then
                                                        this.PRIORITY.[num20 + 1].[num19 - 1] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num20 + 1].[num19 + 1] = 0 then
                                                    this.PRIORITY.[num20 + 1].[num19 + 1] <- 500
                                                    raise (ReturnInt 1)
                                raise (ReturnInt 0)
                            elif this.alotyyli = 3 then
                                if array.[num20 - 1].[num19 - 1] = 1 then
                                    if array.[num20 - 2].[num19 - 2] = 2 && array.[num20 + 1].[num19 + 1] = 0 then
                                        this.PRIORITY.[num20 + 1].[num19 + 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num20 + 1].[num19 + 1] = 2 && array.[num20 - 2].[num19 - 2] = 0 then
                                        this.PRIORITY.[num20 - 2].[num19 - 2] <- 500
                                        raise (ReturnInt 1)
                                if array.[num20 - 1].[num19 + 1] = 1 then
                                    if array.[num20 - 2].[num19 + 2] = 2 && array.[num20 + 1].[num19 - 1] = 0 then
                                        this.PRIORITY.[num20 + 1].[num19 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num20 + 1].[num19 - 1] = 2 && array.[num20 - 2].[num19 + 2] = 0 then
                                        this.PRIORITY.[num20 - 2].[num19 + 2] <- 500
                                        raise (ReturnInt 1)
                                if array.[num20 + 1].[num19 - 1] = 1 then
                                    if array.[num20 + 2].[num19 - 2] = 2 && array.[num20 - 1].[num19 + 1] = 0 then
                                        this.PRIORITY.[num20 - 1].[num19 + 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num20 - 1].[num19 + 1] = 2 && array.[num20 + 2].[num19 - 2] = 0 then
                                        this.PRIORITY.[num20 + 2].[num19 - 2] <- 500
                                        raise (ReturnInt 1)
                                if array.[num20 + 1].[num19 + 1] = 1 then
                                    if array.[num20 + 2].[num19 + 2] = 2 && array.[num20 - 1].[num19 - 1] = 0 then
                                        this.PRIORITY.[num20 - 1].[num19 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num20 - 1].[num19 - 1] = 2 && array.[num20 + 2].[num19 + 2] = 0 then
                                        this.PRIORITY.[num20 + 2].[num19 + 2] <- 500
                                        raise (ReturnInt 1)
                                raise (ReturnInt 0)
                            elif this.alotyyli = 4 then
                                if array.[num20 + 2].[num19] = 1 then
                                    if array.[num20 - 1].[num19 + 1] = 2 then
                                        if array.[num20].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20].[num19 - 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20].[num19 - 2] <> 0 && array.[num20].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20].[num19 + 2] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 - 1].[num19 - 1] = 2 then
                                        if array.[num20].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20].[num19 + 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20].[num19 + 2] <> 0 && array.[num20].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20].[num19 - 2] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 + 3].[num19 + 1] = 2 then
                                        if array.[num20 + 2].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19 - 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 + 2].[num19 - 2] <> 0 && array.[num20 + 2].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19 + 2] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 + 3].[num19 - 1] = 2 then
                                        if array.[num20 + 2].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19 + 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 + 2].[num19 + 2] <> 0 && array.[num20 + 2].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19 - 2] <- 400
                                            raise (ReturnInt 1)
                                if array.[num20 - 2].[num19] = 1 then
                                    if array.[num20 + 1].[num19 + 1] = 2 then
                                        if array.[num20].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20].[num19 - 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20].[num19 - 2] <> 0 && array.[num20].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20].[num19 + 2] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 + 1].[num19 - 1] = 2 then
                                        if array.[num20].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20].[num19 + 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20].[num19 + 2] <> 0 && array.[num20].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20].[num19 - 2] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 - 3].[num19 + 1] = 2 then
                                        if array.[num20 - 2].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19 - 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 - 2].[num19 - 2] <> 0 && array.[num20 - 2].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19 + 2] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 - 3].[num19 - 1] = 2 then
                                        if array.[num20 - 2].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19 + 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 - 2].[num19 + 2] <> 0 && array.[num20 - 2].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19 - 2] <- 400
                                            raise (ReturnInt 1)
                                if array.[num20].[num19 + 2] = 1 then
                                    if array.[num20 + 1].[num19 - 1] = 2 then
                                        if array.[num20 - 2].[num19] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 - 2].[num19] <> 0 && array.[num20 + 2].[num19] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 - 1].[num19 - 1] = 2 then
                                        if array.[num20 + 2].[num19] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 + 2].[num19] <> 0 && array.[num20 - 2].[num19] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 + 1].[num19 + 3] = 2 then
                                        if array.[num20 - 2].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 - 2].[num19 + 2] <> 0 && array.[num20 + 2].[num19] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19 + 2] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 - 1].[num19 + 3] = 2 then
                                        if array.[num20 + 2].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19 + 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 + 2].[num19 + 2] <> 0 && array.[num20 - 2].[num19 + 2] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19 + 2] <- 400
                                            raise (ReturnInt 1)
                                if array.[num20].[num19 - 2] = 1 then
                                    if array.[num20 + 1].[num19 + 1] = 2 then
                                        if array.[num20 - 2].[num19] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 - 2].[num19] <> 0 && array.[num20 + 2].[num19] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 - 1].[num19 + 1] = 2 then
                                        if array.[num20 + 2].[num19] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 + 2].[num19] <> 0 && array.[num20 - 2].[num19] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 + 1].[num19 - 3] = 2 then
                                        if array.[num20 - 2].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19 - 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 - 2].[num19 - 2] <> 0 && array.[num20 + 2].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19 - 2] <- 400
                                            raise (ReturnInt 1)
                                    if array.[num20 - 1].[num19 - 3] = 2 then
                                        if array.[num20 + 2].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20 + 2].[num19 - 2] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num20 + 2].[num19 - 2] <> 0 && array.[num20 - 2].[num19 - 2] = 0 then
                                            this.PRIORITY.[num20 - 2].[num19 - 2] <- 400
                                            raise (ReturnInt 1)
                                for num21 in 8 .. this.laudankoko + 8 - 1 do
                                    for num22 in 8 .. this.laudankoko + 8 - 1 do
                                        if array.[num22].[num21] = 2 then
                                            if array.[num20 - 2].[num19 - 2] = 1 then
                                                if num22 > num21 then
                                                    if array.[num20 - 2].[num19] = 0 then
                                                        this.PRIORITY.[num20 - 2].[num19] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num20].[num19 - 2] = 0 then
                                                    this.PRIORITY.[num20].[num19 - 2] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num20 - 2].[num19 + 2] = 1 then
                                                if num22 > this.laudankoko + 14 - num21 then
                                                    if array.[num20 - 2].[num19] = 0 then
                                                        this.PRIORITY.[num20 - 2].[num19] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num20].[num19 + 2] = 0 then
                                                    this.PRIORITY.[num20].[num19 + 2] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num20 + 2].[num19 - 2] = 1 then
                                                if num22 > this.laudankoko + 14 - num21 then
                                                    if array.[num20].[num19 - 2] = 0 then
                                                        this.PRIORITY.[num20].[num19 - 2] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num20 + 2].[num19] = 0 then
                                                    this.PRIORITY.[num20 + 2].[num19] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num20 + 2].[num19 + 2] = 1 then
                                                if num22 > num21 then
                                                    if array.[num20].[num19 + 2] = 0 then
                                                        this.PRIORITY.[num20].[num19 + 2] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num20 + 2].[num19] = 0 then
                                                    this.PRIORITY.[num20 + 2].[num19] <- 500
                                                    raise (ReturnInt 1)
                                raise (ReturnInt 0)
                            else
                                raise (ReturnInt 0)
                raise (ReturnInt 0)
            elif this.kokovuoro = 5 then
                if this.alotyyli = 2 then
                    let num10 = this.pikahaku(array, 2, 2)
                    if num10 = 9 then
                        raise (ReturnInt 0)
                    for num11 in 8 .. this.laudankoko + 8 - 1 do
                        for num12 in 8 .. this.laudankoko + 8 - 1 do
                            if array.[num12].[num11] = 2 then
                                if array.[num12 - 1].[num11 - 1] = 1 && array.[num12 + 1].[num11 - 1] = 1 && array.[num12 + 1].[num11 + 1] = 1 then
                                    if array.[num12 - 1].[num11] = 0 then
                                        this.PRIORITY.[num12 - 1].[num11] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num12].[num11 + 1] = 0 then
                                        this.PRIORITY.[num12].[num11 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num12 - 1].[num11 - 1] = 1 && array.[num12 - 1].[num11 + 1] = 1 && array.[num12 + 1].[num11 + 1] = 1 then
                                    if array.[num12 + 1].[num11] = 0 then
                                        this.PRIORITY.[num12 + 1].[num11] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num12].[num11 - 1] = 0 then
                                        this.PRIORITY.[num12].[num11 - 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num12 - 1].[num11 - 1] = 1 && array.[num12 + 1].[num11 - 1] = 1 && array.[num12 - 1].[num11 + 1] = 1 then
                                    if array.[num12 + 1].[num11] = 0 then
                                        this.PRIORITY.[num12 + 1].[num11] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num12].[num11 + 1] = 0 then
                                        this.PRIORITY.[num12].[num11 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num12 + 1].[num11 - 1] = 1 && array.[num12 - 1].[num11 + 1] = 1 && array.[num12 + 1].[num11 + 1] = 1 then
                                    if array.[num12 - 1].[num11] = 0 then
                                        this.PRIORITY.[num12 - 1].[num11] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num12].[num11 - 1] = 0 then
                                        this.PRIORITY.[num12].[num11 - 1] <- 500
                                        raise (ReturnInt 1)
                    raise (ReturnInt 0)
                if this.alotyyli <> 3 then
                    raise (ReturnInt 0)
                let num13 = this.pikahaku(array, 2, 2)
                if num13 = 9 then
                    raise (ReturnInt 0)
                for num14 in 8 .. this.laudankoko + 8 - 1 do
                    for num15 in 8 .. this.laudankoko + 8 - 1 do
                        if array.[num15].[num14] = 2 then
                            for num16 in 8 .. this.laudankoko + 8 - 1 do
                                for num17 in 8 .. this.laudankoko + 8 - 1 do
                                    if array.[num17].[num16] = 1 then
                                        if array.[num15 - 1].[num14 - 1] = 2 && (num17 <> num15 - 2 || num16 <> num14 - 2) && (num17 <> num15 + 1 || num16 <> num14 + 1) then
                                            if array.[num15 - 2].[num14 - 2] = 1 then
                                                if num17 > num16 then
                                                    if array.[num15].[num14 + 1] = 0 then
                                                        this.PRIORITY.[num15].[num14 + 1] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num15 + 1].[num14] = 0 then
                                                    this.PRIORITY.[num15 + 1].[num14] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num15 + 1].[num14 + 1] = 1 then
                                                if num17 > num16 then
                                                    if array.[num15 - 2].[num14 - 1] = 0 then
                                                        this.PRIORITY.[num15 - 2].[num14 - 1] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num15 - 1].[num14 - 2] = 0 then
                                                    this.PRIORITY.[num15 - 1].[num14 - 2] <- 500
                                                    raise (ReturnInt 1)
                                        if array.[num15 - 1].[num14 + 1] = 2 && (num17 <> num15 - 2 || num16 <> num14 + 2) && (num17 <> num15 + 1 || num16 <> num14 - 1) then
                                            if array.[num15 - 2].[num14 + 2] = 1 then
                                                if num17 > this.laudankoko + 14 - num16 then
                                                    if array.[num15].[num14 - 1] = 0 then
                                                        this.PRIORITY.[num15].[num14 - 1] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num15 + 1].[num14] = 0 then
                                                    this.PRIORITY.[num15 + 1].[num14] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num15 + 1].[num14 - 1] = 1 then
                                                if num17 > this.laudankoko + 14 - num16 then
                                                    if array.[num15 - 2].[num14 + 1] = 0 then
                                                        this.PRIORITY.[num15 - 2].[num14 + 1] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num15 - 1].[num14 + 2] = 0 then
                                                    this.PRIORITY.[num15 - 1].[num14 + 2] <- 500
                                                    raise (ReturnInt 1)
                                        if array.[num15 + 1].[num14 - 1] = 2 && (num17 <> num15 + 2 || num16 <> num14 - 2) && (num17 <> num15 - 1 || num16 <> num14 + 1) then
                                            if array.[num15 + 2].[num14 - 2] = 1 then
                                                if num17 > this.laudankoko + 14 - num16 then
                                                    if array.[num15 - 1].[num14] = 0 then
                                                        this.PRIORITY.[num15 - 1].[num14] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num15].[num14 + 1] = 0 then
                                                    this.PRIORITY.[num15].[num14 + 1] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num15 - 1].[num14 + 1] = 1 then
                                                if num17 > this.laudankoko + 14 - num16 then
                                                    if array.[num15 + 1].[num14 - 2] = 0 then
                                                        this.PRIORITY.[num15 + 1].[num14 - 2] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num15 + 2].[num14 - 1] = 0 then
                                                    this.PRIORITY.[num15 + 2].[num14 - 1] <- 500
                                                    raise (ReturnInt 1)
                                        if not (array.[num15 + 1].[num14 + 1] <> 2 || (num17 = num15 + 2 && num16 = num14 + 2) || (num17 = num15 - 1 && num16 = num14 - 1)) then
                                            if array.[num15 + 2].[num14 + 2] = 1 then
                                                if num17 > num16 then
                                                    if array.[num15 - 1].[num14] = 0 then
                                                        this.PRIORITY.[num15 - 1].[num14] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num15].[num14 - 1] = 0 then
                                                    this.PRIORITY.[num15].[num14 - 1] <- 500
                                                    raise (ReturnInt 1)
                                            if array.[num15 - 1].[num14 - 1] = 1 then
                                                if num17 > num16 then
                                                    if array.[num15 + 1].[num14 + 1] = 0 then
                                                        this.PRIORITY.[num15 + 1].[num14 + 2] <- 500
                                                        raise (ReturnInt 1)
                                                elif array.[num15 + 2].[num14 + 1] = 0 then
                                                    this.PRIORITY.[num15 + 2].[num14 + 1] <- 500
                                                    raise (ReturnInt 1)
                raise (ReturnInt 0)
            elif this.kokovuoro = 6 then
                if this.alotyyli = 2 then
                    let num7 = this.pikahaku(array, 1, 2)
                    if num7 = 9 then
                        raise (ReturnInt 0)
                    for num8 in 8 .. this.laudankoko + 8 - 1 do
                        for num9 in 8 .. this.laudankoko + 8 - 1 do
                            if array.[num9].[num8] = 1 then
                                if array.[num9 - 2].[num8] = 1 && array.[num9].[num8 - 2] = 1 && array.[num9 - 1].[num8 - 1] = 2 then
                                    if array.[num9].[num8 - 1] = 2 && array.[num9 + 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 - 1].[num8] = 2 && array.[num9 - 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 + 2].[num8] = 1 && array.[num9].[num8 - 2] = 1 && array.[num9 + 1].[num8 - 1] = 2 then
                                    if array.[num9 + 1].[num8] = 2 && array.[num9 + 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9].[num8 - 1] = 2 && array.[num9 - 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 + 2].[num8] = 1 && array.[num9].[num8 + 2] = 1 && array.[num9 + 1].[num8 + 1] = 2 then
                                    if array.[num9 + 1].[num8] = 2 && array.[num9 + 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9].[num8 + 1] = 2 && array.[num9 - 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 - 2].[num8] = 1 && array.[num9].[num8 + 2] = 1 && array.[num9 - 1].[num8 + 1] = 2 then
                                    if array.[num9 - 1].[num8] = 2 && array.[num9 - 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9].[num8 + 1] = 2 && array.[num9 + 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 - 2].[num8] = 1 && array.[num9].[num8 - 2] = 1 && array.[num9 - 1].[num8 - 1] = 2 then
                                    if array.[num9 - 3].[num8 - 1] = 2 && array.[num9 + 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 - 1].[num8 - 3] = 2 && array.[num9 - 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 + 2].[num8] = 1 && array.[num9].[num8 - 2] = 1 && array.[num9 + 1].[num8 - 1] = 2 then
                                    if array.[num9 + 1].[num8 - 3] = 2 && array.[num9 + 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 + 3].[num8 - 1] = 2 && array.[num9 - 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 + 2].[num8] = 1 && array.[num9].[num8 + 2] = 1 && array.[num9 + 1].[num8 + 1] = 2 then
                                    if array.[num9 + 1].[num8 - 3] = 2 && array.[num9 + 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 + 3].[num8 + 1] = 2 && array.[num9 - 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 - 2].[num8] = 1 && array.[num9].[num8 + 2] = 1 && array.[num9 - 1].[num8 + 1] = 2 then
                                    if array.[num9 - 1].[num8 + 3] = 2 && array.[num9 - 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 - 3].[num8 + 1] = 2 && array.[num9 + 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 - 2].[num8] = 1 && array.[num9].[num8 - 2] = 1 && array.[num9 - 1].[num8 - 1] = 2 then
                                    if array.[num9 - 2].[num8 - 1] = 2 && array.[num9 + 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 - 1].[num8 - 2] = 2 && array.[num9 - 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 + 2].[num8] = 1 && array.[num9].[num8 - 2] = 1 && array.[num9 + 1].[num8 - 1] = 2 then
                                    if array.[num9 + 1].[num8 - 2] = 2 && array.[num9 + 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 + 2].[num8 - 1] = 2 && array.[num9 - 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 + 2].[num8] = 1 && array.[num9].[num8 + 2] = 1 && array.[num9 + 1].[num8 + 1] = 2 then
                                    if array.[num9 + 1].[num8 - 2] = 2 && array.[num9 + 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 + 2].[num8 + 1] = 2 && array.[num9 - 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 - 2].[num8] = 1 && array.[num9].[num8 + 2] = 1 && array.[num9 - 1].[num8 + 1] = 2 then
                                    if array.[num9 - 1].[num8 + 2] = 2 && array.[num9 - 1].[num8 - 1] = 0 then
                                        this.PRIORITY.[num9 - 1].[num8 - 1] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 - 2].[num8 + 1] = 2 && array.[num9 + 1].[num8 + 1] = 0 then
                                        this.PRIORITY.[num9 + 1].[num8 + 1] <- 500
                                        raise (ReturnInt 1)
                                if array.[num9 - 1].[num8 + 1] = 1 && array.[num9 + 1].[num8 + 1] = 1 && array.[num9].[num8 + 1] = 2 then
                                    if array.[num9].[num8 + 2] = 0 then
                                        this.PRIORITY.[num9].[num8 + 2] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9].[num8 + 2] = 2 then
                                        if array.[num9 + 2].[num8 + 2] = 0 && array.[num9 - 1].[num8 - 1] = 0 then
                                            this.PRIORITY.[num9 - 1].[num8 - 1] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num9 - 2].[num8 + 2] = 0 && array.[num9 + 1].[num8 - 1] = 0 then
                                            this.PRIORITY.[num9 + 1].[num8 - 1] <- 500
                                            raise (ReturnInt 1)
                                if array.[num9 + 1].[num8 - 1] = 1 && array.[num9 + 1].[num8 + 1] = 1 && array.[num9 + 1].[num8] = 2 then
                                    if array.[num9 + 2].[num8] = 0 then
                                        this.PRIORITY.[num9 + 2].[num8] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 + 2].[num8] = 2 then
                                        if array.[num9 + 2].[num8 - 2] = 0 && array.[num9 - 1].[num8 + 1] = 0 then
                                            this.PRIORITY.[num9 - 1].[num8 + 1] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num9 + 2].[num8 + 2] = 0 && array.[num9 - 1].[num8 - 1] = 0 then
                                            this.PRIORITY.[num9 - 1].[num8 - 1] <- 500
                                            raise (ReturnInt 1)
                                if array.[num9 + 1].[num8 - 1] = 1 && array.[num9 - 1].[num8 - 1] = 1 && array.[num9].[num8 - 1] = 2 then
                                    if array.[num9].[num8 - 2] = 0 then
                                        this.PRIORITY.[num9].[num8 - 2] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9].[num8 - 2] = 2 then
                                        if array.[num9 + 2].[num8 - 2] = 0 && array.[num9 - 1].[num8 + 1] = 0 then
                                            this.PRIORITY.[num9 - 1].[num8 + 1] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num9 - 2].[num8 - 2] = 0 && array.[num9 + 1].[num8 + 1] = 0 then
                                            this.PRIORITY.[num9 + 1].[num8 + 1] <- 500
                                            raise (ReturnInt 1)
                                if not (array.[num9 - 1].[num8 + 1] <> 1 || array.[num9 - 1].[num8 - 1] <> 1 || array.[num9 - 1].[num8] <> 2) then
                                    if array.[num9 - 2].[num8] = 0 then
                                        this.PRIORITY.[num9 - 2].[num8] <- 500
                                        raise (ReturnInt 1)
                                    if array.[num9 - 2].[num8] = 2 then
                                        if array.[num9 + 2].[num8 - 2] = 0 && array.[num9 + 1].[num8 - 1] = 0 then
                                            this.PRIORITY.[num9 + 1].[num8 - 1] <- 500
                                            raise (ReturnInt 1)
                                        if array.[num9 - 2].[num8 - 2] = 0 && array.[num9 + 1].[num8 + 1] = 0 then
                                            this.PRIORITY.[num9 + 1].[num8 + 1] <- 500
                                            raise (ReturnInt 1)
                    raise (ReturnInt 0)
                elif this.alotyyli = 3 then
                    let num2 = this.pikahaku(array, 1, 2)
                    if num2 = 9 then
                        raise (ReturnInt 0)
                    for num3 in 8 .. this.laudankoko + 8 - 1 do
                        for num4 in 8 .. this.laudankoko + 8 - 1 do
                            if array.[num4].[num3] = 1 then
                                for num5 in 8 .. this.laudankoko + 8 - 1 do
                                    for num6 in 8 .. this.laudankoko + 8 - 1 do
                                        if array.[num6].[num5] = 2 then
                                            if array.[num4 - 1].[num3 - 1] = 1 && (num6 <> num4 - 2 || num5 <> num3 - 2) && (num6 <> num4 + 1 || num5 <> num3 + 1) then
                                                if array.[num4 - 2].[num3 - 2] = 2 then
                                                    if num6 > num5 then
                                                        if array.[num4].[num3 + 1] = 0 then
                                                            this.PRIORITY.[num4].[num3 + 1] <- 500
                                                            raise (ReturnInt 1)
                                                    elif array.[num4 + 1].[num3] = 0 then
                                                        this.PRIORITY.[num4 + 1].[num3] <- 500
                                                        raise (ReturnInt 1)
                                                if array.[num4 + 1].[num3 + 1] = 2 then
                                                    if num6 > num5 then
                                                        if array.[num4 - 2].[num3 - 1] = 0 then
                                                            this.PRIORITY.[num4 - 2].[num3 - 1] <- 500
                                                            raise (ReturnInt 1)
                                                    elif array.[num4 - 1].[num3 - 2] = 0 then
                                                        this.PRIORITY.[num4 - 1].[num3 - 2] <- 500
                                                        raise (ReturnInt 1)
                                            if array.[num4 - 1].[num3 + 1] = 1 && (num6 <> num4 - 2 || num5 <> num3 + 2) && (num6 <> num4 + 1 || num5 <> num3 - 1) then
                                                if array.[num4 - 2].[num3 + 2] = 2 then
                                                    if num6 > this.laudankoko + 14 - num5 then
                                                        if array.[num4].[num3 - 1] = 0 then
                                                            this.PRIORITY.[num4].[num3 - 1] <- 500
                                                            raise (ReturnInt 1)
                                                    elif array.[num4 + 1].[num3] = 0 then
                                                        this.PRIORITY.[num4 + 1].[num3] <- 500
                                                        raise (ReturnInt 1)
                                                if array.[num4 + 1].[num3 - 1] = 2 then
                                                    if num6 > this.laudankoko + 14 - num5 then
                                                        if array.[num4 - 2].[num3 + 1] = 0 then
                                                            this.PRIORITY.[num4 - 2].[num3 + 1] <- 500
                                                            raise (ReturnInt 1)
                                                    elif array.[num4 - 1].[num3 + 2] = 0 then
                                                        this.PRIORITY.[num4 - 1].[num3 + 2] <- 500
                                                        raise (ReturnInt 1)
                                            if array.[num4 + 1].[num3 - 1] = 1 && (num6 <> num4 + 2 || num5 <> num3 - 2) && (num6 <> num4 - 1 || num5 <> num3 + 1) then
                                                if array.[num4 + 2].[num3 - 2] = 2 then
                                                    if num6 > this.laudankoko + 14 - num5 then
                                                        if array.[num4 - 1].[num3] = 0 then
                                                            this.PRIORITY.[num4 - 1].[num3] <- 500
                                                            raise (ReturnInt 1)
                                                    elif array.[num4].[num3 + 1] = 0 then
                                                        this.PRIORITY.[num4].[num3 + 1] <- 500
                                                        raise (ReturnInt 1)
                                                if array.[num4 - 1].[num3 + 1] = 2 then
                                                    if num6 > this.laudankoko + 14 - num5 then
                                                        if array.[num4 + 1].[num3 - 2] = 0 then
                                                            this.PRIORITY.[num4 + 1].[num3 - 2] <- 500
                                                            raise (ReturnInt 1)
                                                    elif array.[num4 + 2].[num3 - 1] = 0 then
                                                        this.PRIORITY.[num4 + 2].[num3 - 1] <- 500
                                                        raise (ReturnInt 1)
                                            if not (array.[num4 + 1].[num3 + 1] <> 1 || (num6 = num4 + 2 && num5 = num3 + 2) || (num6 = num4 - 1 && num5 = num3 - 1)) then
                                                if array.[num4 + 2].[num3 + 2] = 2 then
                                                    if num6 > num5 then
                                                        if array.[num4 - 1].[num3] = 0 then
                                                            this.PRIORITY.[num4 - 1].[num3] <- 500
                                                            raise (ReturnInt 1)
                                                    elif array.[num4].[num3 - 1] = 0 then
                                                        this.PRIORITY.[num4].[num3 - 1] <- 500
                                                        raise (ReturnInt 1)
                                                if array.[num4 - 1].[num3 - 1] = 2 then
                                                    if num6 > num5 then
                                                        if array.[num4 + 1].[num3 + 2] = 0 then
                                                            this.PRIORITY.[num4 + 1].[num3 + 2] <- 500
                                                            raise (ReturnInt 1)
                                                    elif array.[num4 + 2].[num3 + 1] = 0 then
                                                        this.PRIORITY.[num4 + 2].[num3 + 1] <- 500
                                                        raise (ReturnInt 1)
                    raise (ReturnInt 0)
                elif this.alotyyli = 4 then
                    let num = this.pikahaku(array, 1, 2)
                    if num = 9 then
                        raise (ReturnInt 0)
                    for m in 8 .. this.laudankoko + 8 - 1 do
                        for n in 8 .. this.laudankoko + 8 - 1 do
                            if array.[n].[m] = 1 then
                                if array.[n - 2].[m - 2] = 0 && array.[n].[m - 2] = 1 && array.[n - 2].[m] = 1 then
                                    this.PRIORITY.[n - 2].[m - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[n - 2].[m - 2] = 1 && array.[n].[m - 2] = 0 && array.[n - 2].[m] = 1 then
                                    this.PRIORITY.[n].[m - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[n - 2].[m - 2] = 1 && array.[n].[m - 2] = 1 && array.[n - 2].[m] = 0 then
                                    this.PRIORITY.[n - 2].[m] <- 500
                                    raise (ReturnInt 1)
                                if array.[n].[m - 2] = 0 && array.[n + 2].[m - 2] = 1 && array.[n + 2].[m] = 1 then
                                    this.PRIORITY.[n].[m - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[n].[m - 2] = 1 && array.[n + 2].[m - 2] = 0 && array.[n + 2].[m] = 1 then
                                    this.PRIORITY.[n + 2].[m - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[n].[m - 2] = 1 && array.[n + 2].[m - 2] = 1 && array.[n + 2].[m] = 0 then
                                    this.PRIORITY.[n + 2].[m - 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[n + 2].[m] = 0 && array.[n + 2].[m + 2] = 1 && array.[n].[m + 2] = 1 then
                                    this.PRIORITY.[n + 2].[m] <- 500
                                    raise (ReturnInt 1)
                                if array.[n + 2].[m] = 1 && array.[n + 2].[m + 2] = 0 && array.[n].[m + 2] = 1 then
                                    this.PRIORITY.[n + 2].[m + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[n + 2].[m] = 1 && array.[n + 2].[m + 2] = 1 && array.[n].[m + 2] = 0 then
                                    this.PRIORITY.[n].[m + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[n - 2].[m] = 0 && array.[n - 2].[m + 2] = 1 && array.[n + 2].[m + 2] = 1 then
                                    this.PRIORITY.[n - 2].[m] <- 500
                                    raise (ReturnInt 1)
                                if array.[n - 2].[m] = 1 && array.[n - 2].[m + 2] = 0 && array.[n + 2].[m + 2] = 1 then
                                    this.PRIORITY.[n - 2].[m + 2] <- 500
                                    raise (ReturnInt 1)
                                if array.[n - 2].[m] = 1 && array.[n - 2].[m + 2] = 1 && array.[n + 2].[m + 2] = 0 then
                                    this.PRIORITY.[n + 2].[m + 2] <- 500
                                    raise (ReturnInt 1)
                    raise (ReturnInt 0)
                else
                    raise (ReturnInt 0)
            else
                raise (ReturnInt 0)
        with ReturnInt v -> v


    member this.haeparas() : Pelipaikka =
        let mutable num = 0
        let mutable num2 = 0
        let mutable y = 0
        let mutable x = 0
        for i in 0 .. this.laudankoko + 8 - 1 do
            for j in 0 .. this.laudankoko + 8 - 1 do
                let num3 = this.arpa()
                if this.arpamaara = 0 then num2 <- 0
                if this.arpamaara = 1 then num2 <- 7 + num3
                if this.arpamaara = 2 then num2 <- System.Convert.ToInt32(0.1 * float num)
                if this.PRIORITY.[j].[i] < -1 then this.PRIORITY.[j].[i] <- 65536 + this.PRIORITY.[j].[i]
                if this.PRIORITY.[j].[i] + num3 + num2 > num && j >= 8 && i >= 8 && this.A.[j - 8].[i - 8] = 0 then
                    num <- this.PRIORITY.[j].[i] + num3
                    y <- j - 8
                    x <- i - 8
        Pelipaikka(X = x, Y = y)

    member this.mietipaikka() : int =
        try
            let suunta = 0
            let mutable puoli = 2
            let pISTEET = Array.zeroCreate<int> 64
            let mutable num = 0
            let mutable suunta2 = 0
            let mutable rivi = 0
            let array = Array.init 64 (fun _ -> Array.zeroCreate<int> 64)
            let array2 = Array.init 64 (fun _ -> Array.zeroCreate<int> 64)
            let aA = Array.init 64 (fun _ -> Array.zeroCreate<int> 8)
            let aA2 = Array.init 16 (fun _ -> Array.zeroCreate<int> 8)
            let bB = Array.init 16 (fun _ -> Array.zeroCreate<int> 8)
            let num2 = this.vuoro
            let mutable num3 = 2
            if this.vuoro = 1 then
                num3 <- 2
                let mutable num4 = 0.85
                if this.kokovuoro < 16 then num4 <- float this.kokovuoro / 17.0
                if Computer.random.NextDouble() > num4 then num3 <- 1
            else
                num3 <- 2
                let mutable num5 = 0.85
                if this.kokovuoro < 16 then num5 <- 0.92
                if Computer.random.NextDouble() > num5 then num3 <- 1
            if Computer.random.NextDouble() > 0.96 then num3 <- 3
            if Computer.random.NextDouble() > 0.98 then num3 <- 4
            let mutable syvyysmax = this.maxsyvyys
            let num6 = this.arpa4()
            for i in 0 .. 63 do
                for j in 0 .. 63 do
                    array.[j].[i] <- 0
            for k in 0 .. this.laudankoko - 1 do
                for l in 0 .. this.laudankoko - 1 do
                    array.[l + 8].[k + 8] <- this.A.[l].[k]
            this.fastend(pISTEET)
            if num2 = 1 then this.taulukko1(aA, 2, 1)
            if num2 = 2 then this.taulukko1(aA, 1, 2)
            for m in 0 .. 5 do
                for n in 8 .. this.laudankoko + 8 - 1 do
                    for num7 in 8 .. this.laudankoko + 8 - 1 do
                        this.lisaapriority(this.PRIORITY, aA, num7, n, suunta, array, m, pISTEET)
            for num8 in 8 .. this.laudankoko + 8 - 1 do
                for num9 in 8 .. this.laudankoko + 8 - 1 do
                    if this.PRIORITY.[num9].[num8] > 1 then
                        if this.PRIORITY.[num9].[num8] < 1000 && this.puhe = 1 then
                            if num6 <= 1 then this.CmpComString <- "Computer: I have no options..."
                            if num6 = 2 then this.CmpComString <- "Computer: Now you had the straight four. :("
                            if num6 = 3 then this.CmpComString <- "Computer: I must place the mark here."
                            if num6 >= 4 then this.CmpComString <- "Computer: There are no question marks."
                        raise (ReturnInt 2)
            if this.maxsyvyys <> 0 then
                if num2 = 1 then
                    this.taulukko2(aA2, bB, 2, 1)
                    puoli <- 2
                if num2 = 2 then
                    this.taulukko2(aA2, bB, 1, 2)
                    puoli <- 1
                for num10 in 0 .. 63 do
                    for num11 in 0 .. 63 do
                        array2.[num10].[num11] <- array.[num10].[num11]
                let mutable x = 0
                let mutable y = 0
                this.rekursiohaku(0, syvyysmax, aA2, bB, array2, this.PRIORITY, rivi, suunta2, x, y, num2) |> ignore
                for num12 in 8 .. this.laudankoko + 8 - 1 do
                    for num13 in 8 .. this.laudankoko + 8 - 1 do
                        if this.PRIORITY.[num13].[num12] > 1 && this.PRIORITY.[num13].[num12] > num then
                            num <- this.PRIORITY.[num13].[num12]
                for num14 in 8 .. this.laudankoko + 8 - 1 do
                    for num15 in 8 .. this.laudankoko + 8 - 1 do
                        if this.PRIORITY.[num15].[num14] > 1 && this.PRIORITY.[num15].[num14] < num then
                            this.PRIORITY.[num15].[num14] <- 0
                for num16 in 8 .. this.laudankoko + 8 - 1 do
                    for num17 in 8 .. this.laudankoko + 8 - 1 do
                        if this.PRIORITY.[num17].[num16] > 1 then
                            if this.puhe = 1 then
                                if num6 <= 1 then this.CmpComString <- "Computer: GG!"
                                if num6 = 2 then this.CmpComString <- "Computer: I won!!! :D"
                                if num6 = 3 then this.CmpComString <- "Computer: Almost too easy..."
                                if num6 >= 4 then this.CmpComString <- "Computer: You must be sleeping."
                            raise (ReturnInt 1)
                let num18 = this.pikahaku(array, num2, 2)
                if num18 = 9 then
                    this.torjuntahaku(array, num2, this.PRIORITY) |> ignore
                    for num19 in 8 .. this.laudankoko + 8 - 1 do
                        for num20 in 8 .. this.laudankoko + 8 - 1 do
                            if this.PRIORITY.[num20].[num19] > 1 then
                                if this.puhe = 1 then this.CmpComString <- "Computer: Hmm... Hmm..."
                                raise (ReturnInt 1)
                    if this.puhe = 1 then
                        if num6 <= 1 then this.CmpComString <- "Computer: I'm under a threat."
                        if num6 = 2 then this.CmpComString <- "Computer: Ohh... Help me! :)"
                        if num6 = 3 then this.CmpComString <- "Computer: Tough game..."
                        if num6 >= 4 then this.CmpComString <- "Computer: You got the straight three."
                if num18 <> 9 then
                    rivi <- 0
                    suunta2 <- 0
                    x <- 0
                    y <- 0
                    syvyysmax <- this.maxsyvyys
                    for num21 in 0 .. 63 do
                        for num22 in 0 .. 63 do
                            array2.[num21].[num22] <- array.[num21].[num22]
                    if num2 = 1 then this.taulukko2(aA2, bB, 1, 2)
                    if num2 = 2 then this.taulukko2(aA2, bB, 2, 1)
                    this.rekursiohaku(0, syvyysmax, aA2, bB, array2, this.PRIORITY, rivi, suunta2, x, y, puoli) |> ignore
                    for num23 in 8 .. this.laudankoko + 8 - 1 do
                        for num24 in 8 .. this.laudankoko + 8 - 1 do
                            if this.PRIORITY.[num24].[num23] > 1 && this.PRIORITY.[num24].[num23] > num then
                                num <- this.PRIORITY.[num24].[num23]
                    for num25 in 8 .. this.laudankoko + 8 - 1 do
                        for num26 in 8 .. this.laudankoko + 8 - 1 do
                            if this.PRIORITY.[num26].[num25] > 1 && this.PRIORITY.[num26].[num25] < num then
                                this.PRIORITY.[num26].[num25] <- 0
                    for num27 in 8 .. this.laudankoko + 8 - 1 do
                        for num28 in 8 .. this.laudankoko + 8 - 1 do
                            if this.PRIORITY.[num28].[num27] > 1 then
                                if this.puhe = 1 then
                                    if num6 <= 1 then this.CmpComString <- "Computer: Let's ruin your game here..."
                                    if num6 = 2 then this.CmpComString <- "Computer: Hmm... I see your point."
                                    if num6 = 3 then this.CmpComString <- "Computer: I can read your mind. ;)"
                                    if num6 >= 4 then this.CmpComString <- "Computer: You have a little trap here."
                                raise (ReturnInt 1)
                    rivi <- 0
                    suunta2 <- 0
                    x <- 0
                    y <- 0
                    syvyysmax <- this.maxsyvyys
                    for num29 in 0 .. 63 do
                        for num30 in 0 .. 63 do
                            array2.[num29].[num30] <- array.[num29].[num30]
                    if num2 = 1 then this.taulukko3(aA2, bB, 2, 1)
                    if num2 = 2 then this.taulukko3(aA2, bB, 1, 2)
                    this.rekursiohaku2(0, syvyysmax - 1, aA2, bB, array2, this.PRIORITY, rivi, suunta2, x, y, num2) |> ignore
                    for num31 in 8 .. this.laudankoko + 8 - 1 do
                        for num32 in 8 .. this.laudankoko + 8 - 1 do
                            if this.PRIORITY.[num32].[num31] > 1 && this.PRIORITY.[num32].[num31] > num then
                                num <- this.PRIORITY.[num32].[num31]
                    for num33 in 8 .. this.laudankoko + 8 - 1 do
                        for num34 in 8 .. this.laudankoko + 8 - 1 do
                            if this.PRIORITY.[num34].[num33] > 1 && this.PRIORITY.[num34].[num33] < num then
                                this.PRIORITY.[num34].[num33] <- 0
                    for num35 in 8 .. this.laudankoko + 8 - 1 do
                        for num36 in 8 .. this.laudankoko + 8 - 1 do
                            if this.PRIORITY.[num36].[num35] > 1 then
                                if this.puhe = 1 then
                                    if num6 <= 1 then this.CmpComString <- "Computer: Now be careful!"
                                    if num6 = 2 then this.CmpComString <- "Computer: Hehehee..."
                                    if num6 = 3 then this.CmpComString <- "Computer: Hmmmmm... Let's see..."
                                    if num6 >= 4 then this.CmpComString <- "Computer: I recommend to concentrate."
                                raise (ReturnInt 1)
                    let mutable flag = true
                    if num2 = 1 then this.taulukko3(aA2, bB, 1, 2)
                    if num2 = 2 then this.taulukko3(aA2, bB, 2, 1)
                    for num37 in 8 .. this.laudankoko + 8 - 1 do
                        for num38 in 8 .. this.laudankoko + 8 - 1 do
                            if this.PRIORITY.[num38].[num37] > 1 then flag <- false
                    if flag then
                        this.rekursiohaku2(0, syvyysmax - 2, aA2, bB, array2, this.PRIORITY, rivi, suunta2, x, y, puoli) |> ignore
                        for num39 in 8 .. this.laudankoko + 8 - 1 do
                            for num40 in 8 .. this.laudankoko + 8 - 1 do
                                if this.PRIORITY.[num40].[num39] > 1 then
                                    this.PRIORITY.[num40].[num39] <- this.PRIORITY.[num40].[num39] / 2
            if num3 = 1 then this.attacker(pISTEET)
            if num3 = 2 then this.defender(pISTEET)
            if num3 = 3 then this.preasure(pISTEET)
            if num3 = 4 then this.tbuilder(pISTEET)
            if num2 = 1 then this.taulukko4(aA, 2, 1)
            if num2 = 2 then this.taulukko4(aA, 1, 2)
            for num41 in 0 .. 39 do
                for num42 in 8 .. this.laudankoko + 8 - 1 do
                    for num43 in 8 .. this.laudankoko + 8 - 1 do
                        this.lisaapriority(this.PRIORITY, aA, num43, num42, suunta, array, num41, pISTEET)
            for num44 in 8 .. this.laudankoko + 8 - 1 do
                for num45 in 8 .. this.laudankoko + 8 - 1 do
                    if this.PRIORITY.[num45].[num44] > 1 then raise (ReturnInt 1)
            for num46 in 9 .. this.laudankoko + 7 - 1 do
                for num47 in 9 .. this.laudankoko + 7 - 1 do
                    if array.[num47].[num46] = 0 then
                        this.PRIORITY.[num47].[num46] <- 50
                        if array.[num47 + 1].[num46] <> 0 then this.PRIORITY.[num47].[num46] <- this.PRIORITY.[num47].[num46] + 100
                        if array.[num47 - 1].[num46] <> 0 then this.PRIORITY.[num47].[num46] <- this.PRIORITY.[num47].[num46] + 100
                        if array.[num47].[num46 - 1] <> 0 then this.PRIORITY.[num47].[num46] <- this.PRIORITY.[num47].[num46] + 100
                        if array.[num47].[num46 + 1] <> 0 then this.PRIORITY.[num47].[num46] <- this.PRIORITY.[num47].[num46] + 100
                        if array.[num47 + 1].[num46 + 1] <> 0 then this.PRIORITY.[num47].[num46] <- this.PRIORITY.[num47].[num46] + 100
                        if array.[num47 - 1].[num46 + 1] <> 0 then this.PRIORITY.[num47].[num46] <- this.PRIORITY.[num47].[num46] + 100
                        if array.[num47 + 1].[num46 - 1] <> 0 then this.PRIORITY.[num47].[num46] <- this.PRIORITY.[num47].[num46] + 100
                        if array.[num47 - 1].[num46 - 1] <> 0 then this.PRIORITY.[num47].[num46] <- this.PRIORITY.[num47].[num46] + 100
            if this.puhe = 1 then
                if num6 <= 1 then this.CmpComString <- "Computer: You have no good places!"
                if num6 = 2 then this.CmpComString <- "Computer: Try to create something, please."
                if num6 = 3 then this.CmpComString <- "Computer: Not going very well..."
                if num6 >= 4 then this.CmpComString <- "Computer: Boring..."
            0
        with ReturnInt v -> v
