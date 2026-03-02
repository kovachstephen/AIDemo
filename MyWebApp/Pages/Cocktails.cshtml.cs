using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class CocktailsModel : PageModel
{
    private readonly ICocktailService _cocktailService;

    public List<CocktailDto> Cocktails { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public string? Spirit { get; set; }

    public CocktailsModel(ICocktailService cocktailService)
    {
        _cocktailService = cocktailService;
    }

    public void OnGet()
    {
        var allCocktails = _cocktailService.GetAllCocktails();
        
        if (string.IsNullOrEmpty(Spirit))
        {
            Cocktails = allCocktails;
        }
        else
        {
            Cocktails = allCocktails.Where(c => 
                c.BaseSpirit?.ToLower() == Spirit.ToLower()).ToList();
        }
    }
}
