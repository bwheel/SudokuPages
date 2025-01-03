

using Microsoft.EntityFrameworkCore;

public class SudokuDbContext : DbContext
{
  public DbSet<Puzzle> Puzzles { get; set; } = null!;

  public SudokuDbContext(DbContextOptions<SudokuDbContext> options)
  : base(options)
  { }

  // protected override void OnConfiguring(DbContextOptionsBuilder options)
  //       => options.UseSqlite($"Data Source=./sudoku.db");

}