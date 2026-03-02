using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class WhiskeyEditModel : PageModel
{
    private readonly IWhiskeyService _whiskeyService;

    [BindProperty]
    public WhiskeyDto Whiskey { get; set; } = new();

    public bool IsNew { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Brand { get; set; }

    public WhiskeyEditModel(IWhiskeyService whiskeyService)
    {
        _whiskeyService = whiskeyService;
    }

    public void OnGet()
    {
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
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _whiskeyService.SaveWhiskey(Whiskey);
        return RedirectToPage("/Whiskeys");
    }
}
