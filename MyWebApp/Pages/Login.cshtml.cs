using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class LoginModel : PageModel
{
    private readonly IAuthService _authService;

    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public string? ErrorMessage { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Message { get; set; }

    public LoginModel(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult OnGet()
    {
        if (_authService.IsLoggedIn)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        if (_authService.Login(Username ?? "", Password ?? ""))
        {
            return RedirectToPage("/Index");
        }
        ErrorMessage = "Invalid username or password";
        return Page();
    }
}
