
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SudokuPages.Data.Repos;
using SudokuPages.Domain;

namespace SudokuPages.Controllers;

[Route("[controller]")]
public class SolutionController : Controller
{
  private readonly PuzzleRepo _puzzleRepo;
  private readonly PuzzleService _puzzleService;

  public SolutionController(PuzzleRepo puzzleRepo, PuzzleService puzzleService)
  {
    _puzzleRepo = puzzleRepo;
    _puzzleService = puzzleService;
  }

  [HttpGet("/[controller]/{id:int}")]
  public IActionResult Index([FromRoute] int id)
  {

    var puzzle = _puzzleRepo.GetById(id);
    ArgumentNullException.ThrowIfNull(puzzle);

    var grid = _puzzleService.ConvertPuzzleTo2DGrid(puzzle.Solution);
    PuzzleViewModel viewModel = new PuzzleViewModel()
    {
      Id = puzzle.Id,
      Grid = grid,
    };
    return View(viewModel);
  }


  [HttpGet("/Print/{id:int}")]
  public IActionResult Print([FromRoute] int id)
  {
    var puzzle = _puzzleRepo.GetById(id);
    ArgumentNullException.ThrowIfNull(puzzle);

    var grid = _puzzleService.ConvertPuzzleTo2DGrid(puzzle.Solution);
    PuzzleViewModel viewModel = new PuzzleViewModel()
    {
      Id = puzzle.Id,
      Grid = grid,
    };
    return View(viewModel);
  }
}