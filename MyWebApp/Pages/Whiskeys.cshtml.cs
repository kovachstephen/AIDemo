using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Pages;

public class WhiskeysModel : PageModel
{
    private readonly IDataService _dataService;

    public List<WhiskeyDto> Whiskeys { get; set; } = new();

    public WhiskeysModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public void OnGet()
    {
        Whiskeys = _dataService.GetAllWhiskeys();
    }
}
