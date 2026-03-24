
using SudokuPages.Data.Repos;

namespace SudokuPages.Domain;

public class PuzzleService
{
  private const int c_numRows = 9;
  private const int c_numCols = 9;
  private const int c_numElements = c_numCols * c_numRows;
  private const char c_emptyElement = '.';
  private const string c_empty2DPuzzle = "........." + "........." + "........." + "........." + "........." + "........." + "........." + "........." + ".........";

  private static readonly Random s_rand = new Random((int)DateTime.Now.Ticks);

  private readonly PuzzleRepo _puzzleRepo;

  public PuzzleService(PuzzleRepo puzzleRepo)
  {
    _puzzleRepo = puzzleRepo;
  }

  /// <summary>
  /// Fetches a puzzle by ID and returns a view model containing its initial (unsolved) grid.
  /// </summary>
  /// <param name="id">The puzzle ID.</param>
  /// <returns>A <see cref="PuzzleViewModel"/> with the puzzle's initial grid.</returns>
  /// <exception cref="ArgumentNullException">Thrown when no puzzle exists for <paramref name="id"/>.</exception>
  public PuzzleViewModel GetPuzzleViewModelById(int id)
  {
    var puzzle = _puzzleRepo.GetById(id);
    ArgumentNullException.ThrowIfNull(puzzle);
    return new PuzzleViewModel { Id = puzzle.Id, Grid = ConvertPuzzleTo2DGrid(puzzle.Initial) };
  }

  /// <summary>
  /// Fetches a puzzle by ID and returns a view model containing its solution grid.
  /// </summary>
  /// <param name="id">The puzzle ID.</param>
  /// <returns>A <see cref="PuzzleViewModel"/> with the puzzle's solution grid.</returns>
  /// <exception cref="ArgumentNullException">Thrown when no puzzle exists for <paramref name="id"/>.</exception>
  public PuzzleViewModel GetSolutionViewModelById(int id)
  {
    var puzzle = _puzzleRepo.GetById(id);
    ArgumentNullException.ThrowIfNull(puzzle);
    return new PuzzleViewModel { Id = puzzle.Id, Grid = ConvertPuzzleTo2DGrid(puzzle.Solution) };
  }

  /// <summary>
  /// Converts an 81-character puzzle string into a 9×9 integer grid.
  /// Empty cells represented by <c>'.'</c> are mapped to <c>0</c>; digit characters are mapped to their integer value.
  /// </summary>
  /// <param name="dbPuzzle">The 81-character puzzle string. Defaults to an all-empty grid.</param>
  /// <returns>A 9×9 <see cref="int"/> array representing the puzzle grid.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="dbPuzzle"/> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when <paramref name="dbPuzzle"/> is empty or whitespace.</exception>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="dbPuzzle"/> is not exactly 81 characters.</exception>
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
