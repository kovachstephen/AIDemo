using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class BrandModel : PageModel
{
    private readonly IBrandService _brandService;

    public BrandDto? Brand { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? B { get; set; }

    public BrandModel(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public void OnGet()
    {
        Brand = _brandService.GetBrandByKey(B ?? "");
    }
}
