using SudokuPages.Data.Models;
using SudokuPages.Data.Repos;
using SudokuPages.Data.Test.Fixtures;

namespace SudokuPages.Data.Test.Repos;


public class PuzzleRepoTest : IDisposable
{
    private readonly DbContextFixture _dbContextFixture;
    private readonly SudokuDbContext _dbContext;
    private readonly Puzzle _expectedPuzzleMin;
    private readonly Puzzle _expectedPuzzleMid;
    private readonly Puzzle _expectedPuzzleMax;

    public PuzzleRepoTest()
    {
        _dbContextFixture = new DbContextFixture();
        _dbContext = _dbContextFixture.CreateContext();
        _expectedPuzzleMin = new Puzzle
        {
            Initial = "1................................................................................",
            Solution = "123456789123456789123456789123456789123456789123456789123456789123456789123456789",
        };
        _dbContext.Puzzles.Add(_expectedPuzzleMin);
        _expectedPuzzleMid = new Puzzle
        {
            Initial = "..................................1...............................................",
            Solution = "123456789123456789123456789123456789123456789123456789123456789123456789123456789",
        };
        _dbContext.Puzzles.Add(_expectedPuzzleMid);
        _expectedPuzzleMax = new Puzzle
        {
            Initial = "................................................................................9",
            Solution = "123456789123456789123456789123456789123456789123456789123456789123456789123456789",
        };
        _dbContext.Puzzles.Add(_expectedPuzzleMax);
        _dbContext.SaveChanges();
    }

    public void Dispose() => _dbContextFixture?.Dispose();


    [Fact]
    public void PuzzleRepo_GetById_NotNull()
    {
        // arrange
        var id = _expectedPuzzleMid.Id;
        var repo = new PuzzleRepo(_dbContext);
        // act
        var actual = repo.GetById(id);
        // assert
        Assert.NotNull(actual);
    }

    [Fact]
    public void PuzzleRepo_GetById_Equal()
    {
        // arrange
        var id = _expectedPuzzleMid.Id;
        var repo = new PuzzleRepo(_dbContext);
        // act
        var actual = repo.GetById(id);
        // assert
        Assert.Equal(_expectedPuzzleMid, actual);
    }

    [Fact]
    public void PuzzleRepo_GetMinId_Equal()
    {
        // arrange
        var expectedId = _expectedPuzzleMin.Id;
        var repo = new PuzzleRepo(_dbContext);
        // act
        var actual = repo.GetMinId();
        // assert
        Assert.Equal(expectedId, actual);
    }

    [Fact]
    public void PuzzleRepo_GetMaxId_Equal()
    {
        // arrange
        var expectedId = _expectedPuzzleMax.Id;
        var repo = new PuzzleRepo(_dbContext);
        // act
        var actual = repo.GetMaxId();
        // assert
        Assert.Equal(expectedId, actual);
    }

    [Fact]
    public void PuzzleRepo_GetRandomId_Equal()
    {
        // arrange
        var expectedIds = new int[] { _expectedPuzzleMin.Id, _expectedPuzzleMid.Id, _expectedPuzzleMax.Id };
        var repo = new PuzzleRepo(_dbContext);
        // act
        var actual = repo.GetRandomId();
        // assert
        Assert.True(expectedIds.Where(id => id == actual).Any());
    }

}
