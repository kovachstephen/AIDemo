using System.Text.Json;
using MyWebApp.Models;

namespace MyWebApp.Services;

public class JsonDataSource : IDataSource
{
    private readonly string _contentRoot;
    private List<WhiskeyDto>? _whiskeys;
    private List<CocktailDto>? _cocktails;
    private List<BrandDto>? _brands;

    public JsonDataSource(IWebHostEnvironment environment)
    {
        _contentRoot = environment.ContentRootPath;
    }

    public List<WhiskeyDto> GetWhiskeys()
    {
        _whiskeys ??= LoadJson<List<WhiskeyDto>>("data.json") ?? new List<WhiskeyDto>();
        return _whiskeys;
    }

    public List<CocktailDto> GetCocktails()
    {
        _cocktails ??= LoadJson<List<CocktailDto>>("cocktails.json") ?? new List<CocktailDto>();
        return _cocktails;
    }

    public List<BrandDto> GetBrands()
    {
        _brands ??= LoadJson<List<BrandDto>>("brands.json") ?? new List<BrandDto>();
        return _brands;
    }

    private T? LoadJson<T>(string fileName) where T : class
    {
        var path = Path.Combine(_contentRoot, fileName);
        var json = System.IO.File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json);
    }
}
