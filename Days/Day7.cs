namespace AdventOfCode2024.Days;

internal sealed class Day7 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var data = GetDataLines()
            .Select(line => line.Split(':', StringSplitOptions.TrimEntries))
            .Select(line => (answer: long.Parse(line[0]), calculation: line[1].Split(' ').Select(int.Parse).ToList()));

        return data
            .Where(row => IsValidCalculation(row, GenerateMulAddOperations))
            .Sum(row => row.answer);
    }

    public override object SolvePart2()
    {

        var data = GetDataLines()
            .Select(line => line.Split(':', StringSplitOptions.TrimEntries))
            .Select(line => (answer: long.Parse(line[0]), calculation: line[1].Split(' ').Select(int.Parse).ToList()));

        return data
            .Where(row => IsValidCalculation(row, GenerateMulAddConcOperations))
            .Sum(row => row.answer);
    }

    private static bool IsValidCalculation((long answer, List<int> numbers) row, Func<int, IEnumerable<List<char>>> operationsGenerator)
    {
        var allOperationCombinations = operationsGenerator(row.numbers.Count);
        foreach (var operations in allOperationCombinations)
        {
            if (PerformCalculation(row.numbers, operations) == row.answer)
            {
                return true;
            }
        }
        return false;
    }

    private static long PerformCalculation(List<int> numbers, List<char> operations)
    {
        long total = numbers[0];
        for (int i = 0; i < operations.Count; i++)
        {
            total = operations[i] switch
            {
                '+' => total + numbers[i + 1],
                '*' => total * numbers[i + 1],
                '|' => long.Parse($"{total}{numbers[i + 1]}"),
                _ => throw new NotImplementedException()
            };
        }
        return total;
    }

    private static IEnumerable<List<char>> GenerateMulAddOperations(int count) =>
        GeneratePermutations(['+', '*'], count - 1);

    private static IEnumerable<List<char>> GenerateMulAddConcOperations(int count) =>
        GeneratePermutations(['+', '*', '|'], count - 1);

    static IEnumerable<List<char>> GeneratePermutations(char[] chars, int length)
    {
        var n = chars.Length;
        var result = new char[length];

        foreach (var perm in Permute(0))
        {
            yield return perm;
        }

        IEnumerable<List<char>> Permute(int pos)
        {
            if (pos == length)
            {
                yield return result.ToList();
                yield break;
            }

            for (int i = 0; i < n; i++)
            {
                result[pos] = chars[i];
                foreach (var perm in Permute(pos + 1))
                {
                    yield return perm;
                }
            }
        }
    }
}
