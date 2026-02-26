using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using MyWebApp.Models;

namespace MyWebApp.Pages;

public class BrandModel : PageModel
{
    public BrandDto? Brand { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? B { get; set; }

    public void OnGet()
    {
        var brandPath = Path.Combine(Directory.GetCurrentDirectory(), "brands.json");
        var brandData = System.IO.File.ReadAllText(brandPath);
        var brands = JsonSerializer.Deserialize<List<BrandDto>>(brandData) ?? new List<BrandDto>();
        
        var brandKey = B?.Replace("-", " ").ToLower()?.Replace("'", "") ?? "";
        Brand = brands.FirstOrDefault(x => x.Brand?.ToLower().Replace("'", "") == brandKey);
    }
}
