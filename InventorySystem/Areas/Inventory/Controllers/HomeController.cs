using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InventarySystem.Models.ViewModels;
using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using System.Collections.Generic;
using InventarySystem.Models.Speficications;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using InventarySystem.Utilities;
using System.Threading.Tasks;

namespace InventorySystem.Areas.Inventory.Controllers;

[Area("Inventory")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWorkUnit _workOfUnit;

    [BindProperty]
    public CartShoppingVM cartShoppingVM { get; set; }


    public HomeController(ILogger<HomeController> logger, IWorkUnit workOfUnit)
    {
        _logger = logger;
        _workOfUnit = workOfUnit;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, string search = "", string currentSearch = "")
    {
        // Control the session
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if(claim != null)
        {
            var listCart = await _workOfUnit.ShoppingCart.RetrieveAll(c => c.UserApplicationId == claim.Value);
            var productsAmount = listCart.Count();
            HttpContext.Session.SetInt32(DS.ssShoppingCart, productsAmount);
        }

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

    public async Task<IActionResult> Detail(int id)
    {
        cartShoppingVM = new CartShoppingVM();
        cartShoppingVM.Company = await _workOfUnit.Company.RetrieveFirst();
        cartShoppingVM.Product = await _workOfUnit.Product.RetrieveFirst(f => f.Id == id,
                                                            includeProperties: "Brand,Category");
        var storeProduct = await _workOfUnit.StoreProduct.RetrieveFirst(p => p.ProductId == id &&
                                                           p.StoreId == cartShoppingVM.Company.StoreSaleId);
        if(storeProduct==null)
        {
            cartShoppingVM.Stock = 0;
        }
        else
        {
            cartShoppingVM.Stock = storeProduct.Amount;
        }
        cartShoppingVM.ShoppingCart = new ShoppingCart()
        {
            Product = cartShoppingVM.Product,
            ProductId = cartShoppingVM.Product.Id
        };

        return View(cartShoppingVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize] // Only registed user can add products to shopping cart
    public async Task<IActionResult> Detail(CartShoppingVM cartShoppingVM)
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
        cartShoppingVM.ShoppingCart.UserApplicationId = claim.Value;

        ShoppingCart cartBD = await _workOfUnit.ShoppingCart.RetrieveFirst(c => c.UserApplicationId == claim.Value &&
                                                              c.ProductId == cartShoppingVM.ShoppingCart.ProductId);
        if(cartBD==null)
        {
            await _workOfUnit.ShoppingCart.Add(cartShoppingVM.ShoppingCart);
        }
        else
        {
            cartBD.Amount += cartShoppingVM.ShoppingCart.Amount;
            _workOfUnit.ShoppingCart.Update(cartBD);
        }
        await _workOfUnit.Save();
        TempData[DS.Success] = "Product added succesfully";

        //Add value to the session
        var listCart = await _workOfUnit.ShoppingCart.RetrieveAll(c => c.UserApplicationId == claim.Value);
        var productsAmount = listCart.Count();
        HttpContext.Session.SetInt32(DS.ssShoppingCart, productsAmount); //This method set the user data available along the entire application

        return RedirectToAction("Index");
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
