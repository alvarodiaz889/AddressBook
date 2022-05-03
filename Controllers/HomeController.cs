using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AddressBook.Models;
using Microsoft.AspNetCore.Authorization;
using AddressBook.Data;
using Microsoft.Extensions.Options;

namespace AddressBook.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Contact");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var error = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        _logger.LogInformation($"Home Controller: {error}");
        return View(error);
    }
}
