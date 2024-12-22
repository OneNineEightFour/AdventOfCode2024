namespace AdventOfCode2024.Days;

internal sealed class Day9 : BaseDay
{
    public override bool IsTest => false;

    public override object SolvePart1()
    {
        var fileBlocks = GetFileBlocks().ToArray();
        var emptyBlocks = GetEmptyBlocks().ToArray();

        var blocksToInsert = emptyBlocks
            .Zip(fileBlocks.Reverse())
            .Select(pair => (pair.Second.ID, pair.First.Position));

        return fileBlocks
            .Concat(blocksToInsert)
            .OrderBy(block => block.Position)
            .Take(fileBlocks.Length)
            .Sum(block => (long)block.ID * block.Position);
    }
    
    public override object SolvePart2()
    {
        var fileBlocks = GetFileBlocks().ToArray();
        var emptyBlocks = GetEmptyBlocks()
            .GroupBy(block => block.ID)
            .ToDictionary(item => item.Key, item => item.OrderBy(a => a.Position).ToList());

        var blocksToInsert = fileBlocks.GroupBy(block => block.ID)
            .OrderByDescending(blockGroup => blockGroup.Key)
            .Select(blockGroup => (ID: blockGroup.Key, Size: blockGroup.Count(), Position: blockGroup.First().Position))
            .Aggregate((IEnumerable<(int ID, int Position)>)[], (accum, curr) =>
             {
                 var blockToUse = emptyBlocks.OrderByDescending(item => item.Key).FirstOrDefault(emptyBlockGroup => emptyBlockGroup.Value.Count >= curr.Size && emptyBlockGroup.Value.First().Position < curr.Position);
                 if (blockToUse.Value != null)
                 {
                    emptyBlocks[blockToUse.Key] = blockToUse.Value.Skip(curr.Size).ToList();
                    return accum.Concat(blockToUse.Value.Take(curr.Size).Select(block => (curr.ID, block.Position))).ToArray();
                 }

                 return accum;
             })
            .ToArray();

        var idsToInsert = blocksToInsert.Select(block => block.ID).ToHashSet();

        return fileBlocks
            .Where(block => !idsToInsert.Contains(block.ID))
            .Concat(blocksToInsert)
            .OrderBy(block => block.Position)
            .Sum(block => (long)block.ID * block.Position);
    }

    private IEnumerable<(int ID, int Position)> GetBlocks() =>
        GetData()
            .Select(c => c - '0')
            .SelectMany((value, i) => i % 2 == 0 ? Enumerable.Repeat(i / 2, value) : Enumerable.Repeat((i + 1) / -2, value))
            .Select((id, i) => (id, i));

    private IEnumerable<(int ID, int Position)> GetFileBlocks() =>
        GetBlocks()
            .Where(block => block.ID >= 0);

    private IEnumerable<(int ID, int Position)> GetEmptyBlocks() =>
        GetBlocks()
            .Where(block => block.ID < 0);
}