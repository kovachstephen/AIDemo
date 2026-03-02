using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class CocktailDetailModel : PageModel
{
    private readonly IDataService _dataService;

    public CocktailDto? Cocktail { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Name { get; set; }

    public CocktailDetailModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public void OnGet()
    {
        Cocktail = _dataService.GetCocktailByName(Name ?? "");
    }
}
