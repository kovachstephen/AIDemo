using System.Text.Json;
using MyWebApp.Models;

namespace MyWebApp.Services;

public interface IDataService
{
    List<WhiskeyDto> GetAllWhiskeys();
    List<CocktailDto> GetAllCocktails();
    List<BrandDto> GetAllBrands();
    CocktailDto? GetCocktailByName(string name);
    BrandDto? GetBrandByKey(string key);
}

public class DataService : IDataService
{
    private readonly string _contentRoot;
    private List<WhiskeyDto>? _whiskeys;
    private List<CocktailDto>? _cocktails;
    private List<BrandDto>? _brands;

    public DataService(IWebHostEnvironment environment)
    {
        _contentRoot = environment.ContentRootPath;
    }

    public List<WhiskeyDto> GetAllWhiskeys()
    {
        _whiskeys ??= LoadJson<List<WhiskeyDto>>("data.json") ?? new List<WhiskeyDto>();
        return _whiskeys;
    }

    public List<CocktailDto> GetAllCocktails()
    {
        _cocktails ??= LoadJson<List<CocktailDto>>("cocktails.json") ?? new List<CocktailDto>();
        return _cocktails;
    }

    public List<BrandDto> GetAllBrands()
    {
        _brands ??= LoadJson<List<BrandDto>>("brands.json") ?? new List<BrandDto>();
        return _brands;
    }

    public CocktailDto? GetCocktailByName(string name)
    {
        return GetAllCocktails().FirstOrDefault(c => 
            c.Name?.ToLower().Replace(" ", "-") == name.ToLower());
    }

    public BrandDto? GetBrandByKey(string key)
    {
        var brandKey = key.Replace("-", " ").ToLower().Replace("'", "");
        return GetAllBrands().FirstOrDefault(x => 
            x.Brand?.ToLower().Replace("'", "") == brandKey);
    }

    private T? LoadJson<T>(string fileName) where T : class
    {
        var path = Path.Combine(_contentRoot, fileName);
        var json = System.IO.File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json);
    }
}
