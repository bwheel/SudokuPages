
using Microsoft.AspNetCore.Mvc;
using SudokuPages.Domain;

namespace SudokuPages.Controllers;

[Route("[controller]")]
public class SolutionController : Controller
{
  private readonly PuzzleService _puzzleService;

  public SolutionController(PuzzleService puzzleService)
  {
    _puzzleService = puzzleService;
  }

  [HttpGet("/[controller]/{id:int}")]
  public IActionResult Index([FromRoute] int id)
  {
    return View(_puzzleService.GetSolutionViewModelById(id));
  }


  [HttpGet("/Print/{id:int}")]
  public IActionResult Print([FromRoute] int id)
  {
    return View(_puzzleService.GetSolutionViewModelById(id));
  }
}