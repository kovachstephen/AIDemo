using MyWebApp.Models;

namespace MyWebApp.Services;

public interface IWhiskeyDataSource
{
    List<WhiskeyDto> GetWhiskeys();
}

public interface ICocktailDataSource
{
    List<CocktailDto> GetCocktails();
}

public interface IBrandDataSource
{
    List<BrandDto> GetBrands();
}
