using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class WhiskeysModel : PageModel
{
    private readonly IWhiskeyService _whiskeyService;
    private readonly IAuthService _authService;

    public List<WhiskeyDto> Whiskeys { get; set; } = new();
    public bool IsLoggedIn => _authService.IsLoggedIn;

    public WhiskeysModel(IWhiskeyService whiskeyService, IAuthService authService)
    {
        _whiskeyService = whiskeyService;
        _authService = authService;
    }

    public void OnGet()
    {
        Whiskeys = _whiskeyService.GetAllWhiskeys();
    }

    public IActionResult OnPostDelete(string brand)
    {
        if (!_authService.IsLoggedIn)
        {
            return RedirectToPage("/Login");
        }
        _whiskeyService.DeleteWhiskey(brand);
        return RedirectToPage();
    }
}
