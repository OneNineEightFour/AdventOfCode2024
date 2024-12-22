using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2024.Days;

internal sealed class Day5 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var rules = GetRules();

        return GetUpdates()
            .Where(update => !update.Select((number, i) => rules.GetValueOrDefault(number, Array.Empty<string>()).Any(largerNumber => update.Take(i).Contains(largerNumber))).Any(val => val))
            .Sum(update => int.Parse(update.ElementAt(update.Length / 2)));
    }

    public override object SolvePart2()
    {
        var rules = GetRules();

        var comparer = new UpdateComparer(rules);
        return GetUpdates()
            .Where(update => update.Select((number, i) => rules.GetValueOrDefault(number, Array.Empty<string>()).Any(largerNumber => update.Take(i).Contains(largerNumber))).Any(val => val))
            .Select(update => update.Order(comparer))
            .Sum(update => int.Parse(update.ElementAt(update.Count() / 2)));
    }

    private Dictionary<string, string[]> GetRules() =>
        GetDataLines()
            .TakeWhile(line => !string.IsNullOrEmpty(line))
            .Select(line => line.Split('|'))
            .GroupBy(line => line[0])
            .ToDictionary(group => group.Key, group => group.Select(line => line[1]).ToArray());

    private IEnumerable<string[]> GetUpdates() =>
        GetDataLines()
            .SkipWhile(line => !string.IsNullOrEmpty(line))
            .Skip(1)
            .Select(line => line.Split(','));

    private class UpdateComparer(Dictionary<string, string[]> rules) : IComparer<string>
    {
        private readonly Dictionary<string, string[]> _rules = rules;

        public int Compare(string? x, string? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            if (_rules.TryGetValue(x, out var xRules))
            {
                if (xRules.Contains(y))
                {
                    return -1;
                }
            }

            if (_rules.TryGetValue(y, out var yRules))
            {
                if (yRules.Contains(x))
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}