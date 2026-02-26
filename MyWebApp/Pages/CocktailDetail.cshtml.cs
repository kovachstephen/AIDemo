using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using MyWebApp.Models;

namespace MyWebApp.Pages;

public class CocktailDetailModel : PageModel
{
    public CocktailDto? Cocktail { get; set; }
    public string? Recipe { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Name { get; set; }

    public void OnGet()
    {
        var cocktailPath = Path.Combine(Directory.GetCurrentDirectory(), "cocktails.json");
        var cocktailData = System.IO.File.ReadAllText(cocktailPath);
        var cocktails = JsonSerializer.Deserialize<List<CocktailDto>>(cocktailData) ?? new List<CocktailDto>();
        
        Cocktail = cocktails.FirstOrDefault(c => c.Name?.ToLower().Replace(" ", "-") == Name?.ToLower());
        
        Recipe = Cocktail?.Name switch
        {
            "Old Fashioned" => "Add sugar and bitters to a rocks glass. Muddle, add ice and bourbon. Stir, garnish with orange peel.",
            "Manhattan" => "Add rye, sweet vermouth, and bitters to a mixing glass with ice. Stir, strain into a coupe glass. Garnish with a cherry.",
            "Whiskey Sour" => "Add bourbon, lemon juice, simple syrup, and egg white to a shaker. Dry shake, add ice, shake again. Strain into a rocks glass with ice.",
            "Mule" => "Add whiskey to a copper mug filled with ice. Top with ginger beer, stir gently. Garnish with lime.",
            "Hot Toddy" => "Add bourbon, honey, and lemon juice to a heatproof glass. Top with hot water, stir well. Garnish with cinnamon stick.",
            "Rusty Nail" => "Add scotch and Drambuie to a rocks glass with ice. Stir well. Garnish with a lemon twist.",
            "Irish Coffee" => "Add Irish whiskey and brown sugar to a warm glass. Pour in hot coffee, stir. Float heavy cream on top.",
            "Jack and Coke" => "Add bourbon to a glass with ice. Top with cola, stir gently. Garnish with a lime wedge.",
            _ => "Mix ingredients according to taste preferences."
        };
    }
}
