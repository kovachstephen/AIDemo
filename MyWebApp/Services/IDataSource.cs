using MyWebApp.Models;

namespace MyWebApp.Services;

public interface IWhiskeyDataSource
{
    List<WhiskeyDto> GetWhiskeys();
    WhiskeyDto? GetWhiskeyByBrand(string brand);
    void SaveWhiskey(WhiskeyDto whiskey);
    void SaveAllWhiskeys(List<WhiskeyDto> whiskeys);
    void DeleteWhiskey(string brand);
    List<WhiskeyDto> GetActiveWhiskeys();
}

public interface ICocktailDataSource
{
    List<CocktailDto> GetCocktails();
}

public interface IBrandDataSource
{
    List<BrandDto> GetBrands();
    BrandDto? GetBrandByName(string name);
    void SaveBrand(BrandDto brand);
    List<BrandDto> GetActiveBrands();
}
