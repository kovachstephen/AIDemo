using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class BrandModel : PageModel
{
    private readonly IBrandService _brandService;
    private readonly IAuthService _authService;

    public BrandDto? Brand { get; set; }
    
    public string? BrandKey { get; set; }
    
    public bool IsEditing { get; set; }
    
    public bool IsLoggedIn => _authService.IsLoggedIn;
    
    [BindProperty(SupportsGet = true)]
    public string? B { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool Edit { get; set; }

    public BrandModel(IBrandService brandService, IAuthService authService)
    {
        _brandService = brandService;
        _authService = authService;
    }

    public IActionResult OnGet()
    {
        BrandKey = B;
        
        if (Edit && !_authService.IsLoggedIn)
        {
            return RedirectToPage("/Login", new { message = "Please login to edit brand details." });
        }
        
        IsEditing = Edit && _authService.IsLoggedIn;
        Brand = _brandService.GetBrandByKey(B ?? "");
        return Page();
    }

    public IActionResult OnPost(BrandDto brand)
    {
        if (!_authService.IsLoggedIn)
        {
            return RedirectToPage("/Login", new { message = "Please login to save brand details." });
        }
        _brandService.SaveBrand(brand);
        return RedirectToPage("/Brand", new { b = brand.Brand?.ToLower().Replace(" ", "-").Replace("'", "") });
    }
}
