using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using MyWebApp.Models;

namespace MyWebApp.Pages;

public class WhiskeysModel : PageModel
{
    public List<WhiskeyDto> Whiskeys { get; set; } = new();

    public void OnGet()
    {
        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");
        var jsonData = System.IO.File.ReadAllText(jsonPath);
        Whiskeys = JsonSerializer.Deserialize<List<WhiskeyDto>>(jsonData) ?? new List<WhiskeyDto>();
    }
}
