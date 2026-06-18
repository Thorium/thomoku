using System;
using System.Text;
using ThomokuSL5;

// Self-play harness for the C# reference oracle.
// Faithfully reproduces MainPage.Konevuoro's computer-move pipeline for BOTH sides:
//   set vuoro -> prionollaus() -> alotus() (if kokovuoro<10) -> mietipaikka() -> haeparas()
//   -> place A[p.Y,p.X] -> TarkistaVoitto() -> kokovuoro++ -> swap side.
// The board starts with one seeded X near the centre (as MainPage's ctor does).

class Program
{
    // Returns: total stones placed (incl. seed), winner (0/1/2), and a signature of the move sequence.
    static (int moves, int winner, string sig) PlayGame(int seed)
    {
        Computer.random = new Random(seed);   // seed BEFORE constructing (ctor consumes random)
        var c = new Computer();
        c.A = new int[40, 40];
        c.kokovuoro = 0;
        c.arpamaara = 1;

        var sb = new StringBuilder();

        // Seed mark: X near centre, exactly like MainPage ctor.
        int cx = 17 + c.arpa();
        int cy = 17 + c.arpa4();
        c.A[cx, cy] = 1;
        c.vuoro = 2;
        c.kokovuoro++;          // kokovuoro = 1
        int placed = 1;
        sb.Append($"X{cx},{cy};");

        bool vuororasti = false; // false => O (2) to move next, true => X (1)
        int winner = 0;

        for (int turn = 0; turn < 1600; turn++)
        {
            c.vuoro = vuororasti ? 1 : 2;
            c.prionollaus();
            if (c.kokovuoro < 10) c.alotus();
            c.CmpComString = " ";
            c.mietipaikka();
            Pelipaikka p = c.haeparas();

            // haeparas returns board coords; placement is A[p.Y, p.X] (see Konevuoro).
            if (p.Y < 0 || p.Y >= 40 || p.X < 0 || p.X >= 40 || c.A[p.Y, p.X] != 0)
            {
                sb.Append("STUCK;");
                return (placed, -1, sb.ToString());  // could not find a legal move
            }

            c.A[p.Y, p.X] = c.vuoro;
            placed++;
            sb.Append($"{(c.vuoro == 1 ? 'X' : 'O')}{p.Y},{p.X};");

            winner = c.TarkistaVoitto();
            c.kokovuoro++;
            if (winner != 0) break;

            vuororasti = !vuororasti;
        }

        return (placed, winner, sb.ToString());
    }

    static void Main(string[] args)
    {
        int games = args.Length > 0 ? int.Parse(args[0]) : 30;
        bool printSig = args.Length > 1 && args[1] == "--sig";

        long total = 0; int min = int.MaxValue, max = 0;
        int w1 = 0, w2 = 0, draw = 0, stuck = 0;

        for (int seed = 1; seed <= games; seed++)
        {
            var (moves, winner, sig) = PlayGame(seed);
            total += moves;
            if (moves < min) min = moves;
            if (moves > max) max = moves;
            if (winner == 1) w1++;
            else if (winner == 2) w2++;
            else if (winner == -1) stuck++;
            else draw++;

            if (printSig)
                Console.WriteLine($"seed={seed} moves={moves} winner={winner} sig={sig}");
            else
                Console.WriteLine($"seed={seed,3} moves={moves,4} winner={winner}");
        }

        Console.WriteLine($"--- games={games} avg={(double)total / games:F1} min={min} max={max} | X={w1} O={w2} draw={draw} stuck={stuck}");
    }
}
