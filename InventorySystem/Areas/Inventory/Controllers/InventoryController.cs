using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Models.ViewModels;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace InventorySystem.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventory)]
    public class InventoryController : Controller
    {
        public readonly IWorkUnit _workOfUnit;

        [BindProperty] 
        public InventoryVM inventoryVM { get; set; }

        public InventoryController(IWorkUnit workOfUnit)
        {
            _workOfUnit = workOfUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewInventory()
        {
            inventoryVM = new InventoryVM()
            {
                Inventory = new InventarySystem.Models.Inventory(),
                StoreList = _workOfUnit.Inventory.RetrieveAllDropdownList("Store")
            };

            inventoryVM.Inventory.State = false;
            // Retrieve the user Id from session
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            inventoryVM.Inventory.UserApplicationId = claim.Value;
            inventoryVM.Inventory.StartDate = DateTime.Now;
            inventoryVM.Inventory.EndDate = DateTime.Now;

            return View(inventoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewInventory(InventoryVM inventoryVM)
        {
            if(ModelState.IsValid)
            {
                inventoryVM.Inventory.StartDate = DateTime.Now;
                inventoryVM.Inventory.EndDate = DateTime.Now;
                await _workOfUnit.Inventory.Add(inventoryVM.Inventory);
                await _workOfUnit.Save();
                return RedirectToAction("InventoryDetails", new { id = inventoryVM.Inventory.Id });
            }
            inventoryVM.StoreList = _workOfUnit.Inventory.RetrieveAllDropdownList("Store");
            return View(inventoryVM);
        }

        public async Task<IActionResult> InventoryDetails(int id)
        {
            inventoryVM = new InventoryVM();
            inventoryVM.Inventory = await _workOfUnit.Inventory.RetrieveFirst(i => i.Id == id, includeProperties: "Store");
            inventoryVM.InventoryDetails = await _workOfUnit.InventoryDetails.RetrieveAll(d => d.InventoryId == id,
                                                                                includeProperties:"Product,Product.Brand");
            return View(inventoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InventoryDetails(int inventoryId, int productId, int amountId)
        {
            inventoryVM = new InventoryVM();
            inventoryVM.Inventory = await _workOfUnit.Inventory.RetrieveFirst(i => i.Id == inventoryId);
            var productStore = await _workOfUnit.StoreProduct.RetrieveFirst(b => b.ProductId == productId &&
                                                                                    b.StoreId == inventoryVM.Inventory.StoreId);
            var detail = await _workOfUnit.InventoryDetails.RetrieveFirst(d => d.InventoryId == inventoryId &&
                                                                                d.ProductId == productId);
            if(detail == null)
            {
                inventoryVM.InventoryDetail = new InventoryDetails();
                inventoryVM.InventoryDetail.ProductId = productId;
                inventoryVM.InventoryDetail.InventoryId = inventoryId;
                if (productStore != null)
                {
                    inventoryVM.InventoryDetail.PreviousStock = productStore.Amount;
                }
                else
                {
                    inventoryVM.InventoryDetail.PreviousStock = 0;
                }
                inventoryVM.InventoryDetail.Amount = amountId;
                await _workOfUnit.InventoryDetails.Add(inventoryVM.InventoryDetail);
                await _workOfUnit.Save();
            }
            else
            {
                detail.Amount += amountId;
                await _workOfUnit.Save();
            }
            return RedirectToAction("InventoryDetails", new { id = inventoryId });
        }

        public async Task<IActionResult> Plus(int id)
        {
            inventoryVM = new InventoryVM();
            var details = await _workOfUnit.InventoryDetails.Retrieve(id);
            inventoryVM.Inventory = await _workOfUnit.Inventory.Retrieve(details.InventoryId);

            details.Amount += 1;
            await _workOfUnit.Save();
            return RedirectToAction("InventoryDetails", new { id = details.InventoryId });
        }

        public async Task<IActionResult> Less(int id)
        {
            inventoryVM = new InventoryVM();
            var details = await _workOfUnit.InventoryDetails.Retrieve(id);
            inventoryVM.Inventory = await _workOfUnit.Inventory.Retrieve(details.InventoryId);
            if(details.Amount == 1)
            {
                _workOfUnit.InventoryDetails.Remove(details);
                await _workOfUnit.Save();
            }
            else
            {
                details.Amount -= 1;
                await _workOfUnit.Save();
            }               
            return RedirectToAction("InventoryDetails", new { id = details.InventoryId });
        }

        public async Task<IActionResult> GenerateStock(int id)
        {
            var inventory = await _workOfUnit.Inventory.Retrieve(id);
            var listDetail = await _workOfUnit.InventoryDetails.RetrieveAll(d => d.InventoryId == id);
            // Retrieve the user Id from session
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            foreach (var item in listDetail)
            {
                var storeProduct = new StoreProduct();
                storeProduct = await _workOfUnit.StoreProduct.RetrieveFirst(b => b.ProductId == item.ProductId &&
                                                                                 b.StoreId == inventory.StoreId,
                                                                                 isTracking: false);
                if(storeProduct != null) // If the stock product already exist, the amount must to be updated
                {
                    await _workOfUnit.KardexInventory.RegisterKardex(storeProduct.Id, "Entry", "Inventory Register",
                                                                     storeProduct.Amount, item.Amount, claim.Value);
                    storeProduct.Amount += item.Amount;
                    await _workOfUnit.Save();
                }
                else // The stock register doesn't exist, must to create it 
                {
                    storeProduct = new StoreProduct();
                    storeProduct.StoreId = inventory.StoreId;
                    storeProduct.ProductId = item.ProductId;
                    storeProduct.Amount = item.Amount;
                    await _workOfUnit.StoreProduct.Add(storeProduct);
                    await _workOfUnit.Save();
                    await _workOfUnit.KardexInventory.RegisterKardex(storeProduct.Id, "Entry", "Initial Inventory",
                                                                     storeProduct.Amount, item.Amount, claim.Value);
                }
            }
            // Update the inventory header
            inventory.State = true;
            inventory.EndDate = DateTime.Now;
            await _workOfUnit.Save();
            TempData[DS.Success] = "The inventory was registered sucessfully";
            return RedirectToAction("Index");
        }

        public IActionResult KardexProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult KardexProduct(string startDateId, string endDateId, int productId)
        {
            return RedirectToAction("KardexProductResult", new { startDateId, endDateId, productId });
        }

        public async Task<IActionResult> KardexProductResult(string startDateId, string endDateId, int productId)
        {
            KardexInventoryVM kardexInventoryVM = new KardexInventoryVM();
            kardexInventoryVM.Product = new Product();
            kardexInventoryVM.Product = await _workOfUnit.Product.Retrieve(productId);

            kardexInventoryVM.StartDate = DateTime.Parse(startDateId); // 00:00:00
            kardexInventoryVM.EndDate = DateTime.Parse(endDateId).AddHours(23).AddMinutes(59);

            kardexInventoryVM.KardexInventoryList = await _workOfUnit.KardexInventory.RetrieveAll(
                                                               k => k.StoreProduct.ProductId == productId &&
                                                               (k.RegisterDate >= kardexInventoryVM.StartDate &&
                                                                k.RegisterDate <= kardexInventoryVM.EndDate),
                                        includeProperties: "StoreProduct,StoreProduct.Product,StoreProduct.Store",
                                        orderBy: o => o.OrderBy(o => o.RegisterDate)
                );

            return View(kardexInventoryVM);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> RetrieveAll()
        {
            var all = await _workOfUnit.StoreProduct.RetrieveAll(includeProperties: "Store,Product");
            return Json(new { data = all });
        }

        [HttpGet]
        public async Task<IActionResult> SearchProduct(string term)
        {
            if(!String.IsNullOrEmpty(term))
            {
                var productList = await _workOfUnit.Product.RetrieveAll(p => p.State == true);
                var data = productList.Where(x => x.SerialNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                                  x.Description.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(data);
            }
            return Ok();
        }
        #endregion
    }
}
