using System.Text.Json;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

internal static class ObjectExtensions
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
    };

    public static T Dump<T>(this T self)
    {
        Console.WriteLine(JsonSerializer.Serialize(self, _serializerOptions));
        return self;
    }

    public static string ValueOrDefault(this Group self, string defaultValue) =>
        self.Success ?
            self.Value :
            defaultValue;

    public static IEnumerable<R> Then<T, R>(this IEnumerable<T> self, Func<IEnumerable<T>, IEnumerable<R>> transformer) =>
        transformer(self);
}