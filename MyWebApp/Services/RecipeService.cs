namespace MyWebApp.Services;

public interface IRecipeService
{
    string GetRecipe(string cocktailName);
}

public class RecipeService : IRecipeService
{
    private readonly IDataService _dataService;

    public RecipeService(IDataService dataService)
    {
        _dataService = dataService;
    }

    public string GetRecipe(string cocktailName)
    {
        var cocktail = _dataService.GetCocktailByName(cocktailName);
        return cocktail?.Recipe ?? "Mix ingredients according to taste preferences.";
    }
}
