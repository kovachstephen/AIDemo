using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class CocktailDetailModel : PageModel
{
    private readonly ICocktailService _cocktailService;

    public CocktailDto? Cocktail { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Name { get; set; }

    public CocktailDetailModel(ICocktailService cocktailService)
    {
        _cocktailService = cocktailService;
    }

    public void OnGet()
    {
        Cocktail = _cocktailService.GetCocktailByName(Name ?? "");
    }
}
