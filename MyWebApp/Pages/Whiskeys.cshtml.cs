using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class WhiskeysModel : PageModel
{
    private readonly IWhiskeyService _whiskeyService;

    public List<WhiskeyDto> Whiskeys { get; set; } = new();

    public WhiskeysModel(IWhiskeyService whiskeyService)
    {
        _whiskeyService = whiskeyService;
    }

    public void OnGet()
    {
        Whiskeys = _whiskeyService.GetAllWhiskeys();
    }

    public IActionResult OnPostDelete(string brand)
    {
        _whiskeyService.DeleteWhiskey(brand);
        return RedirectToPage();
    }
}
