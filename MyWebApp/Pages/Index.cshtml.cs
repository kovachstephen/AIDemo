using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly IAuthService _authService;

    public bool IsLoggedIn => _authService.IsLoggedIn;
    public string? Username => _authService.Username;

    public IndexModel(IAuthService authService)
    {
        _authService = authService;
    }
}
