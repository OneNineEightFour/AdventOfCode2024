using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

internal sealed class Day3 : BaseDay
{
    private const string MulOperator = "mul";
    private const string DoOperator = "do";
    private const string DontOperator = "don't";

    public override bool IsTest => false;

    public override object SolvePart1() =>
        GetOperations()
            .Where(operation => operation.Operator == MulOperator)
            .Sum(operation => operation.X * operation.Y);

    public override object SolvePart2() =>
        GetOperations()
            .Aggregate((Result: 0, ExecuteNext: true), (accumelator, operation) => operation switch
            {
                (MulOperator, var x, var y) when accumelator.ExecuteNext => accumelator with { Result = accumelator.Result + operation.X * operation.Y },
                (MulOperator, _, _) => accumelator,
                (DoOperator, _, _) => accumelator with { ExecuteNext = true },
                (DontOperator, _, _) => accumelator with { ExecuteNext = false },
                _ => throw new NotImplementedException(),
            }).Result;

    private IEnumerable<(string Operator, int X, int Y)> GetOperations() =>
        Regex.Matches(GetData(), @$"(?<Op>{DontOperator})\(\)|(?<Op>{DoOperator})\(\)|(?<Op>{MulOperator})\((?<X>\d{{1,3}}),(?<Y>\d{{1,3}})\)")
            .Cast<Match>()
            .Select(match => (Operator: match.Groups["Op"].Value, X: int.Parse(match.Groups["X"].ValueOrDefault("0")), Y: int.Parse(match.Groups["Y"].ValueOrDefault("0"))));
}