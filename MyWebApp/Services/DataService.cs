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
    private readonly IDataSource _dataSource;

    public DataService(IDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public List<WhiskeyDto> GetAllWhiskeys()
    {
        return _dataSource.GetWhiskeys();
    }

    public List<CocktailDto> GetAllCocktails()
    {
        return _dataSource.GetCocktails();
    }

    public List<BrandDto> GetAllBrands()
    {
        return _dataSource.GetBrands();
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
}
