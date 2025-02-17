
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

using SudokuPages.Data.Models;

namespace SudokuPages.Data.Repos;

public class PuzzleRepo
{
    private readonly SudokuDbContext _dbContext;
    private static readonly Random s_rand = new Random((int)DateTime.Now.Ticks);
    private static Dictionary<string, object> _simpleCache = new Dictionary<string, object>();
    private class CacheKeys
    {
        public static string MaxIdKey = "Puzzle.Id.Max";
        public static string MinIdKey = "Puzzle.Id.Min";
    }
    public PuzzleRepo(SudokuDbContext dbContext) => _dbContext = dbContext;

    public Puzzle? GetById(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0, nameof(id));
        var result = _dbContext
            .Puzzles
            .Where(p => p.Id == id)
            .FirstOrDefault();
        return result;
    }

    public int GetMinId()
    {
        if (_simpleCache.TryGetValue(CacheKeys.MinIdKey, out object? minId))
            return (int)minId;
        var puzzle = _dbContext.Puzzles.OrderBy(x => x.Id).First();
        _simpleCache[CacheKeys.MinIdKey] = puzzle.Id;
        return puzzle.Id;
    }

    public int GetMaxId()
    {
        if (_simpleCache.TryGetValue(CacheKeys.MaxIdKey, out object? maxId))
            return (int)maxId;

        var puzzle = _dbContext.Puzzles.OrderByDescending(x => x.Id).First();
        _simpleCache[CacheKeys.MaxIdKey] = puzzle.Id;

        return puzzle.Id;
    }

    public int GetRandomId()
    {
        var min = GetMinId();
        var max = GetMaxId();
        return s_rand.Next(min, max);
    }
}
