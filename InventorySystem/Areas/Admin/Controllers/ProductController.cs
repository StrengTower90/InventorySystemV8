using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWorkUnit _workOfUnit;

        public ProductController(IWorkUnit workOfUnit)
        {
            _workOfUnit = workOfUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        /* Implementing ModelView */
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductVM productVM = new ProductVM
            {
                Product = new Product(),
                CategoryList = _workOfUnit.Product.RetrieveAllDropdownList("Category"),
                BrandList = _workOfUnit.Product.RetrieveAllDropdownList("Brand")
            };

            if(id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = await _workOfUnit.Product.Retrieve(id.GetValueOrDefault());
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> RetrieveAll()
        {
            var all = await _workOfUnit.Product.RetrieveAll(includeProperties:"Category,Brand"); //Trough that params we send allows to include relational navigation
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productDB = await _workOfUnit.Product.Retrieve(id);
            if(productDB == null)
            {
                return Json(new { success = false, message = "Error on deleting Product" });
            }
            _workOfUnit.Product.Remove(productDB);
            await _workOfUnit.Save();
            return Json(new { success = true, message = "Product was deleted successfully" });
        }

        [ActionName("ValidateSerie")]
        public async Task<IActionResult> ValidateSerie(string serie, int id = 0)
        {
            bool value = false;
            var list = await _workOfUnit.Product.RetrieveAll();
            if(id == 0)
            {
                value = list.All(b => b.SerieNumber.ToLower().Trim() == serie.ToLower().Trim());
            }
            else
            {
                value = list.Any(b => b.SerieNumber.ToLower().Trim() == serie.ToLower().Trim() && b.Id != id);
            }
            if(value)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion
    }
}
