using SudokuPages.Domain;

namespace SudokuPages.Test.Domain;

public class PuzzelServiceTest
{
  [Fact]
  public void PuzzelService_ctor_NotNull()
  {
    var service = new PuzzleService();
    Assert.NotNull(service);
  }

  [Theory]
  [InlineData(".................................................................................")]
  [InlineData("1................................................................................")]
  [InlineData("123456789123456789123456789123456789123456789123456789123456789123456789123456789")]
  public void PuzzelService_ConvertPuzzleTo2DGrid_NotNull(string? dbPuzzle)
  {
    var service = new PuzzleService();

    var result = service.ConvertPuzzleTo2DGrid(dbPuzzle);
    Assert.NotNull(result);
  }

  [Theory]
  [InlineData("too-short")]
  [InlineData("too-long123456789123456789123456789123456789123456789123456789123456789123456789123456789")]
  public void PuzzleService_ConvertPuzzleTo2DGrid_ThrowsArgumentOutOfRangeException(string? dbPuzzle)
  {
    var service = new PuzzleService();
    Assert.Throws<ArgumentOutOfRangeException>(() => service.ConvertPuzzleTo2DGrid(dbPuzzle));
  }


  [Fact]
  public void PuzzleService_ConvertPuzzleTo2DGrid_ThrowsArgumentNullException()
  {
    var service = new PuzzleService();
    Assert.Throws<ArgumentNullException>(() => service.ConvertPuzzleTo2DGrid(null));
  }


  [Theory]
  [InlineData("")]
  [InlineData(" ")]
  [InlineData("\t")]
  [InlineData("\r")]
  [InlineData("\r\n")]
  [InlineData("\n")]
  public void PuzzleService_ConvertPuzzleTo2DGrid_ThrowsArgumentException(string dbPuzzle)
  {
    var service = new PuzzleService();
    Assert.Throws<ArgumentException>(() => service.ConvertPuzzleTo2DGrid(dbPuzzle));
  }


  [Fact]
  public void PuzzleService_ConvertPuzzleTo2DGrid_Equal()
  {
    string dbPuzzle = "123456789123456789123456789123456789123456789123456789123456789123456789123456789";
    int[,] expected = new int[9, 9]{
      {1,2,3,4,5,6,7,8,9},
      {1,2,3,4,5,6,7,8,9},
      {1,2,3,4,5,6,7,8,9},
      {1,2,3,4,5,6,7,8,9},
      {1,2,3,4,5,6,7,8,9},
      {1,2,3,4,5,6,7,8,9},
      {1,2,3,4,5,6,7,8,9},
      {1,2,3,4,5,6,7,8,9},
      {1,2,3,4,5,6,7,8,9},
    };

    var service = new PuzzleService();
    var actual = service.ConvertPuzzleTo2DGrid(dbPuzzle);
    for (int i = 0; i < 9; i++)
    {
      for (int j = 0; j < 9; j++)
      {
        Assert.Equal(expected[i, j], actual[i, j]);
      }
    }
  }
}
