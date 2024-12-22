namespace AdventOfCode2024.Days;

internal sealed class Day1 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var firstList = GetList(0).Order();
        var secondList = GetList(1).Order();

        return firstList.Zip(secondList)
            .Sum(item => Math.Abs(item.First - item.Second));
    }

    public override object SolvePart2()
    {
        var firstList = GetList(0);
        var secondList = GetList(1);

        var secondListLookup = secondList
            .GroupBy(value => value)
            .ToDictionary(group => group.Key, group => group.Count());

        return firstList.Sum(value => secondListLookup.TryGetValue(value, out var count) ? value * count : 0);
    }

    private IEnumerable<int> GetList(int n) =>
        GetDataLines()
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(numbers => int.Parse(numbers[n]));
}