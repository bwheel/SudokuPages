using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SudokuPages.Data.Repos;
using SudokuPages.Domain;
using SudokuPages.Models;

namespace SudokuPages.Controllers;

[Route("[controller]")]
public class PuzzleController : Controller
{
    private readonly ILogger<PuzzleController> _logger;
    private readonly PuzzleRepo _puzzleRepo;
    private readonly PuzzleService _puzzleService;

    public PuzzleController(ILogger<PuzzleController> logger, PuzzleRepo puzzleRepo, PuzzleService puzzleService)
    {
        _logger = logger;
        _puzzleRepo = puzzleRepo;
        _puzzleService = puzzleService;
    }

    [HttpGet]
    [Route("/{id:int?}")]
    [Route("/[controller]/{id:int}")]
    public IActionResult Index(int? id)
    {
        _logger.LogInformation("Puzzle loading with id: {0}", id);
        if (!id.HasValue)
        {
            id = _puzzleRepo.GetRandomId();
            return RedirectToAction("Index", new { id });
        }

        return View(_puzzleService.GetPuzzleViewModelById(id.Value));
    }


    [HttpGet]
    public IActionResult Random()
    {
        var id = _puzzleRepo.GetRandomId();
        return RedirectToAction("Index", new { id });
    }


    [HttpGet("/[controller]/Print/{id:int}")]
    public IActionResult Print([FromRoute] int id)
    {
        return View(_puzzleService.GetPuzzleViewModelById(id));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
