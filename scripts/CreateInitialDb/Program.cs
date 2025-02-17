
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

using CsvHelper;
using CsvHelper.Configuration.Attributes;

using Microsoft.EntityFrameworkCore;

using SudokuPages.Data.Models;

namespace CreateInitialDb;

class Program
{
    private static void Log(string message)
        => Console.WriteLine($"CreateInitialDb:\t{message}");
    class CsvRecordRow
    {
        [Name("id")]
        public int Id { get; set; }
        [Name("puzzle")]
        public string Puzzle { get; set; }
        [Name("solution")]
        public string Solution { get; set; }
        [Name("clues")]
        public string Clues { get; set; }
        [Name("difficulty")]
        public decimal Difficulty { get; set; }
    }

    static void Main(string[] args)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(args.Length, 2, "csvFileIn = arg[0] AND = sqliteFileOut");
        string csvFileIn = args[0];
        string sqliteFileOut = args[1];

        if (!File.Exists(csvFileIn))
            throw new ArgumentException(nameof(csvFileIn));
        if (File.Exists(sqliteFileOut))
        {
            Log($"Removing previous to create new. sqliteFileOut: {sqliteFileOut}");
            File.Delete(sqliteFileOut);
        }
        Log($"Creating sqlite file. {sqliteFileOut}");
        File.Create(sqliteFileOut).Close();

        Log($"Opening csvFileIn: {csvFileIn}");
        using var streamReader = new StreamReader(csvFileIn);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        var csvRecords = csvReader.GetRecords<CsvRecordRow>();
        var puzzles = csvRecords.Select(x => new Puzzle
        {
            Initial = x.Puzzle,
            Solution = x.Solution,
            Difficulty = x.Difficulty,
        });
        var builder = new DbContextOptionsBuilder<SudokuDbContext>();
        builder.UseSqlite($"Data Source={sqliteFileOut}");
        Log($"Opening sqliteFileOut: {sqliteFileOut}");
        using var dbContext = new SudokuDbContext(builder.Options);
        dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        Log("Migrating DB");
        dbContext.Database.Migrate();
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Log("Inserting Puzzles");
        dbContext.Puzzles.AddRange(puzzles);
        Log("Saving Changes");
        dbContext.SaveChanges();
        sw.Stop();

        Log($"Saved puzzles({dbContext.Puzzles.Count()})\tellapsed{sw.Elapsed.ToString()}\tsqliteFileOut: {sqliteFileOut}");
    }
}
