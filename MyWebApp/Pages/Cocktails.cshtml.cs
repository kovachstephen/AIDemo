using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using MyWebApp.Models;

namespace MyWebApp.Pages;

public class CocktailsModel : PageModel
{
    public List<CocktailDto> Cocktails { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public string? Spirit { get; set; }

    public void OnGet()
    {
        var cocktailPath = Path.Combine(Directory.GetCurrentDirectory(), "cocktails.json");
        var cocktailData = System.IO.File.ReadAllText(cocktailPath);
        var allCocktails = JsonSerializer.Deserialize<List<CocktailDto>>(cocktailData) ?? new List<CocktailDto>();
        
        if (string.IsNullOrEmpty(Spirit))
        {
            Cocktails = allCocktails;
        }
        else
        {
            Cocktails = allCocktails.Where(c => c.BaseSpirit?.ToLower() == Spirit.ToLower()).ToList();
        }
    }
}
