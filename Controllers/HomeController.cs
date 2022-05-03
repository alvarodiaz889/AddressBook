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
    private readonly ApplicationDbContext _context;
    private readonly IOptions<AdminCredentialsOptions> _adminCreds;
    public HomeController(ILogger<HomeController> logger,
        ApplicationDbContext context,
        IOptions<AdminCredentialsOptions> options)
    {
        _logger = logger;
        _context = context;
        _adminCreds = options;
    }

    [Authorize]
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Contact");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var error = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        _logger.LogInformation($"Home Controller: {error}");
        return View(error);
    }
}
