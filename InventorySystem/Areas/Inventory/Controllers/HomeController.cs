using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InventarySystem.Models.ViewModels;
using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using System.Collections.Generic;

namespace InventorySystem.Areas.Inventory.Controllers;

[Area("Inventory")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWorkUnit _workOfUnit;


    public HomeController(ILogger<HomeController> logger, IWorkUnit workOfUnit)
    {
        _logger = logger;
        _workOfUnit = workOfUnit;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Product> productList = await _workOfUnit.Product.RetrieveAll();
        return View(productList);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
