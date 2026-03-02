namespace MyWebApp.Services;

public interface IRecipeService
{
    string GetRecipe(string cocktailName);
}

public class RecipeService : IRecipeService
{
    private readonly ICocktailService _cocktailService;

    public RecipeService(ICocktailService cocktailService)
    {
        _cocktailService = cocktailService;
    }

    public string GetRecipe(string cocktailName)
    {
        var cocktail = _cocktailService.GetCocktailByName(cocktailName);
        return cocktail?.Recipe ?? "Mix ingredients according to taste preferences.";
    }
}
