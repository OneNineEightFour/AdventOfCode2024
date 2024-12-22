namespace AdventOfCode2024.Days;

internal sealed class Day4 : BaseDay
{
    private const double Deg45InRad = Math.PI / 4;

    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var puzzle = GetPuzzle();
        var center = CalculateCenter(puzzle);

        return Enumerable.Range(0, 8)
            .Select(n => n * Deg45InRad)
            .Sum(theta => puzzle
                .Then(puzzle => RotateAroundCenter(puzzle, center, theta))
                .Then(puzzle => FindSequences(puzzle, "XMAS"))
                .Count());
    }

    public override object SolvePart2()
    {
        var puzzle = GetPuzzle();
        var center = CalculateCenter(puzzle);

        return Enumerable.Range(0, 4)
            .Select(n => (n * 2 + 1) * Deg45InRad)
            .SelectMany(theta => puzzle
                .Then(puzzle => RotateAroundCenter(puzzle, center, theta))
                .Then(puzzle => FindSequences(puzzle, "MAS").Select(sequence => sequence.ElementAt(1)))
                .Then(puzzle => RotateAroundCenter(puzzle, center, -theta)))
            .GroupBy(square => square)
            .Count(group => group.Count() == 2);
    }

    private IEnumerable<(double X, double Y, char C)> GetPuzzle() =>
        GetDataLines()
            .SelectMany((line, y) => line.Select((c, x) => ((double)x, (double)y, c)))
            .ToArray();

    private static (double X, double Y) CalculateCenter(IEnumerable<(double X, double Y, char C)> puzzle) =>
        (
            X: (puzzle.Max(square => square.X) + puzzle.Min(square => square.X)) / 2,
            Y: (puzzle.Max(square => square.Y) + puzzle.Min(square => square.Y)) / 2
        );

    private static IEnumerable<(double X, double Y, char C)> RotateAroundCenter(IEnumerable<(double X, double Y, char C)> puzzle, (double x, double y) center, double theta)
    {
        (var sin, var cos) = Math.SinCos(theta);
        return puzzle.Select(square => square with
        {
            X = Math.Round(cos * (square.X - center.x) - sin * (square.Y - center.y) + center.x, 1),
            Y = Math.Round(sin * (square.X - center.x) + cos * (square.Y - center.y) + center.y, 1),
        });
    }

    private static IEnumerable<IEnumerable<(double X, double Y, char C)>> FindSequences(IEnumerable<(double X, double Y, char C)> puzzle, string needle) =>
        puzzle
            .GroupBy(square => square.Y)
            .Select(group => group.OrderBy(square => square.X))
            .Select(squareLine => squareLine.Select((_, i) => squareLine.Skip(i).Take(needle.Length)))
            .SelectMany(sequences => sequences.Where(sequence => needle.SequenceEqual(sequence.Select(square => square.C))));
}