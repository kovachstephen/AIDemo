using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class BrandModel : PageModel
{
    private readonly IBrandService _brandService;

    public BrandDto? Brand { get; set; }
    
    public string? BrandKey { get; set; }
    
    public bool IsEditing { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? B { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool Edit { get; set; }

    public BrandModel(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public void OnGet()
    {
        BrandKey = B;
        IsEditing = Edit;
        Brand = _brandService.GetBrandByKey(B ?? "");
    }

    public IActionResult OnPost(BrandDto brand)
    {
        _brandService.SaveBrand(brand);
        return RedirectToPage("/Brand", new { b = brand.Brand?.ToLower().Replace(" ", "-").Replace("'", "") });
    }
}
