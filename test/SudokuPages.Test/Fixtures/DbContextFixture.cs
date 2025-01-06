using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SudokuPages.Fixtures;

public class DbContextFixture : IDisposable
{
  private readonly SqliteConnection _connection;
  public DbContextFixture()
  {
    _connection = new SqliteConnection("DataSource=:memory:");
    _connection.Open();
  }
  public SudokuDbContext CreateContext()
  {
    var options = new DbContextOptionsBuilder<SudokuDbContext>()
      .UseSqlite(_connection)
      .Options;
    var context = new SudokuDbContext(options);
    context.Database.EnsureCreated();
    return context;
  }
  public void Dispose() => _connection.Close();
}
