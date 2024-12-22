namespace AdventOfCode2024.Days;

internal sealed class Day8 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var (width, height) = GetMapBounds();
        var map = GetMap()
            .Where(node => node.C != '.')
            .ToArray();

        return map.GroupBy(node => node.C)
            .SelectMany(group => group.SelectMany((firstNode, i) => group.Where(secondNode => firstNode != secondNode).Select(secondNode => (firstNode, secondNode))))
            .Select(pair => (X: pair.firstNode.X - (pair.secondNode.X - pair.firstNode.X), Y: pair.firstNode.Y - (pair.secondNode.Y - pair.firstNode.Y)))
            .Where(antiNode => antiNode.X >= 0 && antiNode.Y >= 0 && antiNode.X < width && antiNode.Y < height)
            .Distinct()
            .Count();
    }

    public override object SolvePart2()
    {
        var (width, height) = GetMapBounds();
        var map = GetMap()
            .Where(node => node.C != '.')
            .ToArray();

        return map.GroupBy(node => node.C)
            .SelectMany(group => group.SelectMany((firstNode, i) => group.Where(secondNode => firstNode != secondNode).Select(secondNode => (firstNode, secondNode))))
            .Select(pair => (StartX: pair.firstNode.X, StartY: pair.firstNode.Y, DX: pair.secondNode.X - pair.firstNode.X, DY: pair.secondNode.Y - pair.firstNode.Y))
            .SelectMany(nodeHarmonics => Enumerable.Range(0, Math.Max(width / Math.Abs(nodeHarmonics.DX), height / Math.Abs(nodeHarmonics.DY))).Select(n => (X: nodeHarmonics.StartX - nodeHarmonics.DX * n, Y: nodeHarmonics.StartY - nodeHarmonics.DY * n)))
            .Where(antiNode => antiNode.X >= 0 && antiNode.Y >= 0 && antiNode.X < width && antiNode.Y < height)
            .Distinct()
            .Count();
    }

    private IEnumerable<(int X, int Y, char C)> GetMap() =>
        GetDataLines()
            .SelectMany((line, y) => line.Select((c, x) => (x, y, c)));

    private (int Width, int Height) GetMapBounds() =>
        (GetDataLines().First().Length, GetDataLines().Count());
}