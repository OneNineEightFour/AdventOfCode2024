using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode2024;

var lastDayType = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(type => type != typeof(BaseDay) && type.IsAssignableTo(typeof(BaseDay)))
    .OrderBy(type => int.Parse(Regex.Match(type.Name, @"\d+").Value))
    .Last();

var lastDay = (BaseDay)Activator.CreateInstance(lastDayType)!;

try
{
    Console.WriteLine($"{lastDay.Name} - {(lastDay.IsTest ? "Sample data" : "Real data")}");

    var sw = Stopwatch.StartNew();
    Console.WriteLine($"Part1: {lastDay.SolvePart1()} ({(int)sw.Elapsed.TotalMilliseconds}ms)");

    sw.Restart();
    Console.WriteLine($"Part2: {lastDay.SolvePart2()} ({(int)sw.Elapsed.TotalMilliseconds}ms)");
}
catch (NotImplementedException)
{
}