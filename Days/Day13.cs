using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

internal sealed class Day13 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        return GetMachines()
            .Select(GetButtonPressCount)
            .Where(pressCount => decimal.IsInteger(pressCount.ButtonA) && decimal.IsInteger(pressCount.ButtonB))
            .Sum(pressCount => pressCount.ButtonA * 3 + pressCount.ButtonB);
    }

    public override object SolvePart2()
    {
        return GetMachines()
            .Select(machine => machine with { Prize = new Vector2(machine.Prize.X + 10000000000000, machine.Prize.Y + 10000000000000)})
            .Select(GetButtonPressCount)
            .Where(pressCount => decimal.IsInteger(pressCount.ButtonA) && decimal.IsInteger(pressCount.ButtonB))
            .Sum(pressCount => pressCount.ButtonA * 3 + pressCount.ButtonB);
    }

    private IEnumerable<Machine> GetMachines() =>
        GetData()
            .Split("\n\n")
            .Select(machineData => machineData
                .Split('\n')
                .Select(line => Regex.Match(line, @"X[\+=](?<X>\d+), Y[\+=](?<Y>\d+)"))
                .Select(match => new Vector2(int.Parse(match.Groups["X"].Value), int.Parse(match.Groups["Y"].Value)))
                .ToArray())
            .Select(parsedMachineData => new Machine(parsedMachineData[0], parsedMachineData[1], parsedMachineData[2]))
            .ToArray();

    private static (decimal ButtonA, decimal ButtonB) GetButtonPressCount(Machine machine)
    {
        var d = (decimal)machine.ButtonA.X * machine.ButtonB.Y - machine.ButtonA.Y * machine.ButtonB.X;
        if (d == 0)
        {
            throw new Exception("Maureen is lui");
        }

        var d1 = machine.Prize.X * machine.ButtonB.Y - machine.Prize.Y * machine.ButtonB.X;
        var d2 = machine.ButtonA.X * machine.Prize.Y - machine.ButtonA.Y * machine.Prize.X;

        var pressACount = d1 / d;
        var pressBCount = d2 / d;
        return (pressACount, pressBCount);
    }

    record Vector2(long X, long Y);

    record Machine(Vector2 ButtonA, Vector2 ButtonB, Vector2 Prize) { }
}