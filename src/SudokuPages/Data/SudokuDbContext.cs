

using Microsoft.EntityFrameworkCore;

using SudokuPages.Data.Models;

public class SudokuDbContext : DbContext
{
    public DbSet<Puzzle> Puzzles { get; set; } = null!;

    public SudokuDbContext(DbContextOptions<SudokuDbContext> options)
    : base(options)
    { }
}