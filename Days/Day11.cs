namespace AdventOfCode2024.Days;

internal sealed class Day11 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var initialState = GetInitialState();

        return Blink(initialState, 25).Count();
    }

    public override object SolvePart2()
    {
        var initialState = GetInitialState();
        
        return Blink(initialState, 75).Count();
    }

    private IEnumerable<long> GetInitialState() =>
        GetData()
            .Split(' ')
            .Select(long.Parse);

    private static IEnumerable<long> Blink(IEnumerable<long> initialState, int count) =>
        Enumerable.Range(0, count)
            .Aggregate(initialState, (state, increment) => state.SelectMany(value => ExecuteRule(value)));

    private static IEnumerable<long> ExecuteRule(long value) =>
        value switch
        {
            0 => [1],
            _ when value.ToString() is var stringValue && stringValue.Length % 2 == 0 => [long.Parse(stringValue[..(stringValue.Length / 2)]), long.Parse(stringValue[(stringValue.Length / 2)..])],
            _ => [value * 2024],
        };
}