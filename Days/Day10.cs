using AdventOfCode2024.Model;

namespace AdventOfCode2024.Days;

internal sealed class Day10 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var map = GetMap();

        return map
            .Where(cell => cell.Value == '0')
            .Sum(startPoint => GetTrailEndPoints(map, startPoint).Distinct().Count());
    }

    public override object SolvePart2()
    {
        var map = GetMap();

        return map
            .Where(cell => cell.Value == '0')
            .Sum(startPoint => GetTrailEndPoints(map, startPoint).Count());
    }

    private IEnumerable<Cell> GetTrailEndPoints(IEnumerable<Cell> map, Cell startPoint) =>
        map
            .Where(cell => Math.Abs(cell.X - startPoint.X) + Math.Abs(cell.Y - startPoint.Y) == 1)
            .Where(cell => cell.Value - startPoint.Value == 1)
            .SelectMany(cell => GetTrailEndPoints(map, cell).Append(cell))
            .Where(cell => cell.Value == '9');

    private IEnumerable<Cell> GetMap() =>
        GetDataLines()
            .SelectMany((line, y) => line.Select((c, x) => new Cell(x, y, c)));
}