using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SudokuPages.Models;

namespace SudokuPages.Controllers;

public class PuzzleController : Controller
{
    private readonly ILogger<PuzzleController> _logger;
    private readonly SudokuDbContext _dbContext;

    public PuzzleController(ILogger<PuzzleController> logger, SudokuDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet("/{id:int?}")]
    public IActionResult Index(int? id)
    {
        if (!id.HasValue)
        {
            int max = _dbContext.Puzzles.OrderByDescending(x => x.Id).First().Id;
            int min = _dbContext.Puzzles.OrderBy(x => x.Id).First().Id;
            Random r = new Random((int)DateTime.Now.Ticks);
            id = r.Next(min, max);
            return RedirectToAction("Index", new { id });
        }
        var puzzle = _dbContext.Puzzles.Where(x => x.Id == id).First()!;
        var grid = convertToGrid(puzzle.Initial);
        PuzzleViewModel viewModel = new PuzzleViewModel()
        {
            Id = puzzle.Id,
            Grid = grid,
        };
        return View(viewModel);
    }

    private static int[,] convertToGrid(string puzzle)
    {
        char[] elements = puzzle.ToCharArray();
        int[,] grid = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                char element = elements[i * 9 + j];
                grid[i, j] = element == '.' ? 0 : int.Parse(element.ToString());
            }
        }
        return grid;
    }

    [HttpGet]
    public IActionResult Random()
    {

        int max = _dbContext.Puzzles.OrderByDescending(x => x.Id).First().Id;
        int min = _dbContext.Puzzles.OrderBy(x => x.Id).First().Id;
        Random r = new Random((int)DateTime.Now.Ticks);
        var id = r.Next(min, max);
        return RedirectToAction("Index", new { id });
    }


    [HttpGet("/Solution/{id:int}")]
    public IActionResult Solution(int id)
    {
        var puzzle = _dbContext.Puzzles.Where(x => x.Id == id).First()!;

        var grid = convertToGrid(puzzle.Solution!);
        PuzzleViewModel viewModel = new PuzzleViewModel()
        {
            Id = puzzle.Id,
            Grid = grid,
        };
        return View(viewModel);
    }

    [HttpGet("/Print/{id:int}")]
    public IActionResult Print(int id)
    {
        var puzzle = _dbContext.Puzzles.Where(x => x.Id == id).First()!;
        var grid = convertToGrid(puzzle.Initial);
        PuzzleViewModel viewModel = new PuzzleViewModel()
        {
            Id = puzzle.Id,
            Grid = grid,
        };
        return View(viewModel);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
