using MyWebApp.Models;

namespace MyWebApp.Services;

public interface IWhiskeyService
{
    List<WhiskeyDto> GetAllWhiskeys();
}

public interface ICocktailService
{
    List<CocktailDto> GetAllCocktails();
    CocktailDto? GetCocktailByName(string name);
}

public interface IBrandService
{
    List<BrandDto> GetAllBrands();
    BrandDto? GetBrandByKey(string key);
}

public class DataService : IWhiskeyService, ICocktailService, IBrandService
{
    private readonly IWhiskeyDataSource _whiskeyDataSource;
    private readonly ICocktailDataSource _cocktailDataSource;
    private readonly IBrandDataSource _brandDataSource;

    public DataService(
        IWhiskeyDataSource whiskeyDataSource,
        ICocktailDataSource cocktailDataSource,
        IBrandDataSource brandDataSource)
    {
        _whiskeyDataSource = whiskeyDataSource;
        _cocktailDataSource = cocktailDataSource;
        _brandDataSource = brandDataSource;
    }

    public List<WhiskeyDto> GetAllWhiskeys()
    {
        return _whiskeyDataSource.GetWhiskeys();
    }

    public List<CocktailDto> GetAllCocktails()
    {
        return _cocktailDataSource.GetCocktails();
    }

    public List<BrandDto> GetAllBrands()
    {
        return _brandDataSource.GetBrands();
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
