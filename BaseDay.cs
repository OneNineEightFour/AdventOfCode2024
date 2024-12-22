namespace AdventOfCode2024;

internal abstract class BaseDay
{
    private string DataFileName => $"Data/{Name}{(IsTest ? "_Test" : string.Empty)}";

    public string Name => GetType().Name;

    public abstract bool IsTest { get; }

    protected IEnumerable<string> GetDataLines() =>
        File.ReadAllLines(DataFileName);

    protected string GetData() =>
        File.ReadAllText(DataFileName).Trim();

    public abstract object SolvePart1();

    public abstract object SolvePart2();
}