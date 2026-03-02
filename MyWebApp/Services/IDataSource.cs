using MyWebApp.Models;

namespace MyWebApp.Services;

public interface IDataSource
{
    List<WhiskeyDto> GetWhiskeys();
    List<CocktailDto> GetCocktails();
    List<BrandDto> GetBrands();
}
