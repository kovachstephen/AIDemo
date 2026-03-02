using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class WhiskeyEditModel : PageModel
{
    private readonly IWhiskeyService _whiskeyService;
    private readonly IAuthService _authService;

    [BindProperty]
    public WhiskeyDto Whiskey { get; set; } = new();

    public bool IsNew { get; set; }
    public bool IsLoggedIn => _authService.IsLoggedIn;

    [BindProperty(SupportsGet = true)]
    public string? Brand { get; set; }

    public WhiskeyEditModel(IWhiskeyService whiskeyService, IAuthService authService)
    {
        _whiskeyService = whiskeyService;
        _authService = authService;
    }

    public IActionResult OnGet()
    {
        if (!_authService.IsLoggedIn)
        {
            return RedirectToPage("/Login", new { message = "Please login to add or edit whiskeys." });
        }

        if (string.IsNullOrEmpty(Brand))
        {
            IsNew = true;
            Whiskey = new WhiskeyDto();
        }
        else
        {
            IsNew = false;
            var existing = _whiskeyService.GetWhiskeyByBrand(Brand);
            Whiskey = existing ?? new WhiskeyDto { Brand = Brand.Replace("-", " ") };
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!_authService.IsLoggedIn)
        {
            return RedirectToPage("/Login", new { message = "Please login to save whiskeys." });
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _whiskeyService.SaveWhiskey(Whiskey);
        return RedirectToPage("/Whiskeys");
    }
}
