using Microsoft.EntityFrameworkCore;
using SudokuPages.Data.Repos;
using SudokuPages.Domain;

namespace SudokuPages;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string connectionString = builder.Configuration.GetConnectionString("default") ?? throw new ArgumentException("default connection string");
        builder.Services.AddDbContext<SudokuDbContext>(options => options.UseSqlite(connectionString));
        builder.Services.AddTransient<PuzzleRepo>();
        builder.Services.AddTransient<PuzzleService>();
        builder.Services.AddControllersWithViews();


        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Puzzle/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Puzzle}/{action=Index}/{id?}");


        app.Run();
    }
}
