using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class BrandModel : PageModel
{
    private readonly IDataService _dataService;

    public BrandDto? Brand { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? B { get; set; }

    public BrandModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public void OnGet()
    {
        Brand = _dataService.GetBrandByKey(B ?? "");
    }
}
