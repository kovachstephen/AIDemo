using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class CocktailDetailModel : PageModel
{
    private readonly IDataService _dataService;
    private readonly IRecipeService _recipeService;

    public CocktailDto? Cocktail { get; set; }
    public string? Recipe { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Name { get; set; }

    public CocktailDetailModel(IDataService dataService, IRecipeService recipeService)
    {
        _dataService = dataService;
        _recipeService = recipeService;
    }

    public void OnGet()
    {
        Cocktail = _dataService.GetCocktailByName(Name ?? "");
        Recipe = Cocktail != null ? _recipeService.GetRecipe(Cocktail.Name!) : null;
    }
}
