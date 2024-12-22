namespace AdventOfCode2024.Days;

internal sealed class Day2 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1() =>
        GetReports()
            .Count(IsValidReport);

    public override object SolvePart2() =>
        GetReports()
            .Count(report => IsValidReport(report) || report.Select((_, x) => report.Where((_, y) => x != y)).Any(IsValidReport));

    private IEnumerable<int>[] GetReports() =>
        GetDataLines()
            .Select(line => line.Split(' ').Select(int.Parse))
            .ToArray();

    private static bool IsValidReport(IEnumerable<int> report)
    {
        var reportDiff = report.Zip(report.Skip(1));

        return reportDiff.All(item => Math.Abs(item.First - item.Second) <= 3) &&
            (reportDiff.All(item => item.First > item.Second) || reportDiff.All(item => item.First < item.Second));
    }
}