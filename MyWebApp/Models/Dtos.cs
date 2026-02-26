using System.Text.Json.Serialization;

namespace MyWebApp.Models;

public class WhiskeyDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("mashBill")]
    public string? MashBill { get; set; }
}

public class Ingredient
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("volume")]
    public string? Volume { get; set; }
    [JsonPropertyName("preferredBrand")]
    public string? PreferredBrand { get; set; }
}

public class CocktailDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("baseSpirit")]
    public string? BaseSpirit { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    [JsonPropertyName("ingredients")]
    public List<Ingredient>? Ingredients { get; set; }
}

public class BrandDto
{
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("distillery")]
    public string? Distillery { get; set; }
    [JsonPropertyName("location")]
    public string? Location { get; set; }
    [JsonPropertyName("founded")]
    public int Founded { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    [JsonPropertyName("mapUrl")]
    public string? MapUrl { get; set; }
}
