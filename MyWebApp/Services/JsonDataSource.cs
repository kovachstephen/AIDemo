using System.Text.Json;
using MyWebApp.Models;

namespace MyWebApp.Services;

public class JsonDataSource : IWhiskeyDataSource, ICocktailDataSource, IBrandDataSource
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

    public WhiskeyDto? GetWhiskeyByBrand(string brand)
    {
        return GetWhiskeys().FirstOrDefault(w => 
            w.Brand?.ToLower().Replace(" ", "-") == brand.ToLower());
    }

    public void SaveWhiskey(WhiskeyDto whiskey)
    {
        var whiskeys = GetWhiskeys();
        var existing = whiskeys.FindIndex(w => 
            w.Brand?.ToLower() == whiskey.Brand?.ToLower() && 
            w.Type?.ToLower() == whiskey.Type?.ToLower());
        
        if (existing >= 0)
        {
            whiskeys[existing] = whiskey;
        }
        else
        {
            whiskeys.Add(whiskey);
        }
        
        SaveJson("data.json", whiskeys);
    }

    public void SaveAllWhiskeys(List<WhiskeyDto> whiskeys)
    {
        _whiskeys = whiskeys;
        SaveJson("data.json", whiskeys);
    }

    public void DeleteWhiskey(string brand)
    {
        var whiskeys = GetWhiskeys();
        var whiskey = whiskeys.FirstOrDefault(w => 
            w.Brand?.ToLower().Replace(" ", "-").Replace("'", "") == brand.ToLower());
        
        if (whiskey != null)
        {
            whiskey.IsDeleted = true;
            SaveJson("data.json", whiskeys);
        }
        
        var brands = GetBrands();
        var brandObj = brands.FirstOrDefault(b => 
            b.Brand?.ToLower().Replace(" ", "-").Replace("'", "") == brand.ToLower());
        
        if (brandObj != null)
        {
            brandObj.IsDeleted = true;
            SaveJson("brands.json", brands);
        }
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

    public BrandDto? GetBrandByName(string name)
    {
        return GetBrands().FirstOrDefault(b => 
            b.Brand?.ToLower().Replace(" ", "-").Replace("'", "") == name.ToLower());
    }

    public void SaveBrand(BrandDto brand)
    {
        var brands = GetBrands();
        var existing = brands.FindIndex(b => 
            b.Brand?.ToLower() == brand.Brand?.ToLower());
        
        if (existing >= 0)
        {
            brands[existing] = brand;
        }
        else
        {
            brands.Add(brand);
        }
        
        SaveJson("brands.json", brands);
    }

    public List<WhiskeyDto> GetActiveWhiskeys()
    {
        return GetWhiskeys().Where(w => !w.IsDeleted).ToList();
    }

    public List<BrandDto> GetActiveBrands()
    {
        return GetBrands().Where(b => !b.IsDeleted).ToList();
    }

    private T? LoadJson<T>(string fileName) where T : class
    {
        var path = Path.Combine(_contentRoot, fileName);
        var json = System.IO.File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json);
    }

    private void SaveJson<T>(string fileName, T data)
    {
        var path = Path.Combine(_contentRoot, fileName);
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(path, json);
    }
}
