using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InventarySystem.Models.ViewModels;
using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using System.Collections.Generic;
using InventarySystem.Models.Speficications;
using System.Globalization;

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

    public IActionResult Index(int pageNumber = 1, string search = "", string currentSearch = "")
    {
        if(pageNumber < 1) { pageNumber = 1; }

        if(!String.IsNullOrEmpty(search))
        {
            pageNumber = 1;
        }
        else
        {
            search = currentSearch;
        }
        ViewData["CurrentSearch"] = search;

        Parameters parameters = new Parameters()
        {
            PageNumber = pageNumber,
            PageSize = 4
        };

        var result = _workOfUnit.Product.RetrieveAllPaginated(parameters);

        if(!String.IsNullOrEmpty(search))
        {
            result = _workOfUnit.Product.RetrieveAllPaginated(parameters, p => p.Description.Contains(search));
        }

        ViewData["TotalPages"] = result.MetaData.TotalPages;
        ViewData["TotalRecords"] = result.MetaData.TotalCount;
        ViewData["PageSize"] = result.MetaData.PageSize;
        ViewData["PageNumber"] = pageNumber;
        ViewData["Previous"] = "disabled";
        ViewData["Next"] = "";

        if(pageNumber > 1) { ViewData["Previous"] = ""; }
        if(result.MetaData.TotalPages <= pageNumber) { ViewData["Next"] = "disabled"; }

        return View(result);
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
