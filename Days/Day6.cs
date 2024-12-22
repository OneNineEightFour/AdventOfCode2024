using System.Collections.Immutable;
using System.Numerics;

namespace AdventOfCode2024.Days;

internal sealed class Day6 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var map = GetMap();
        var startPosition = GetStartPosition(map);
        var direction = new Vector2(0, -1);

        return FollowPath(map, startPosition, direction)
            .Distinct()
            .Count();
    }

    public override object SolvePart2()
    {
        var map = GetMap();
        var startPosition = GetStartPosition(map);
        var direction = new Vector2(0, -1);

        return FollowPath(map, startPosition, direction)
            .Skip(1)
            .Distinct()
            .Count(position => !CanEscape(InsertObstacle(map, position), startPosition, direction, [(startPosition, direction)]));
    }

    private ImmutableArray<ImmutableArray<char>> GetMap() =>
        GetDataLines()
            .Select(line => line.ToImmutableArray())
            .ToImmutableArray();

    private static Vector2 GetStartPosition(ImmutableArray<ImmutableArray<char>> map)
    {
        var coords = map
            .SelectMany((line, y) => line.Select((c, x) => (x, y, c)))
            .First(square => square.c == '^');

        return new Vector2(coords.x, coords.y);
    }

    private static ImmutableArray<ImmutableArray<char>> InsertObstacle(ImmutableArray<ImmutableArray<char>> map, Vector2 position) =>
        map
            .Select((line, y) => line.Select((c, x) => position.X == x && position.Y == y ? '#' : c).ToImmutableArray())
            .ToImmutableArray();

    private static IEnumerable<Vector2> FollowPath(ImmutableArray<ImmutableArray<char>> map, Vector2 position, Vector2 direction)
    {
        var newPosition = position + direction;
        if (newPosition.X < 0 || newPosition.Y < 0 || newPosition.X >= map[0].Length || newPosition.Y >= map.Length)
        {
            return [position];
        }

        if (map[(int)newPosition.Y][(int)newPosition.X] == '#')
        {
            return FollowPath(map, position, RotateDirection(direction));
        }

        return FollowPath(map, newPosition, direction).Prepend(position);
    }

    private static bool CanEscape(ImmutableArray<ImmutableArray<char>> map, Vector2 position, Vector2 direction, ImmutableHashSet<(Vector2 position, Vector2 direction)> visitedPositions)
    {
        var newPosition = position + direction;
        if (newPosition.X < 0 || newPosition.Y < 0 || newPosition.X >= map[0].Length || newPosition.Y >= map.Length)
        {
            return true;
        }

        if (visitedPositions.Contains((newPosition, direction)))
        {
            return false;
        }

        if (map[(int)newPosition.Y][(int)newPosition.X] == '#')
        {
            return CanEscape(map, position, RotateDirection(direction), visitedPositions);
        }

        return CanEscape(map, newPosition, direction, visitedPositions.Add((newPosition, direction)));
    }

    private static Vector2 RotateDirection(Vector2 direction) =>
        direction switch
        {
            { X: -1, Y: 0 } => new Vector2(0, -1),
            { X: 0, Y: -1 } => new Vector2(1, 0),
            { X: 1, Y: 0 } => new Vector2(0, 1),
            { X: 0, Y: 1 } => new Vector2(-1, 0),
            _ => throw new NotImplementedException()
        };
}