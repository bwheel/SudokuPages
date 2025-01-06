

namespace SudokuPages.Domain;

public class PuzzleService
{
  private const int c_numRows = 9;
  private const int c_numCols = 9;
  private const int c_numElements = c_numCols * c_numRows;
  private const char c_emptyElement = '.';
  private const string c_empty2DPuzzle = "........." + "........." + "........." + "........." + "........." + "........." + "........." + "........." + ".........";

  private static readonly Random s_rand = new Random((int)DateTime.Now.Ticks);

  public int[,] ConvertPuzzleTo2DGrid(string? dbPuzzle = c_empty2DPuzzle)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(dbPuzzle, nameof(dbPuzzle));
    ArgumentOutOfRangeException.ThrowIfNotEqual(dbPuzzle.Length, c_numElements, nameof(dbPuzzle));

    var elements = new Queue<char>(dbPuzzle.ToCharArray());
    int[,] grid = new int[c_numCols, c_numRows];
    for (int i = 0; i < c_numCols; i++)
      for (int j = 0; j < c_numRows; j++)
        grid[i, j] = mapDbPuzzleElementToGridElement(elements.Dequeue());

    return grid;
  }

  private int mapDbPuzzleElementToGridElement(char letter) => letter switch
  {
    c_emptyElement => 0,
    var c when int.TryParse(c.ToString(), out int i) => i,
    _ => throw new ArgumentException(nameof(letter)),
  };

}
