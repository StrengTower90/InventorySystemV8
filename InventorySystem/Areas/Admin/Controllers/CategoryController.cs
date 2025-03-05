using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IWorkUnit _unitWork;

        public CategoryController(IWorkUnit unitWork)
        {
            _unitWork = unitWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Category category = new Category();

            if(id == null)
            {
                // Create a category
                category.State = true;
                return View(category);
            }
            //Update Category
            category = await _unitWork.Category.Retrieve(id.GetValueOrDefault());
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category category)
        {
            if(ModelState.IsValid)
            {
                if(category.Id == 0)
                {
                    await _unitWork.Category.Add(category);
                    TempData[DS.Success] = "Category was created successfully";
                }
                else
                {
                    _unitWork.Category.Update(category);
                    TempData[DS.Success] = "Category was updated successfully";
                }
                await _unitWork.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error during saving category";
            return View(category);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> RetrieveAll()
        {
            var all = await _unitWork.Category.RetrieveAll();
            return Json(new { data = all });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryDB = await _unitWork.Category.Retrieve(id);
            if(id == 0)
            {
                return Json(new { success = false, message = "Something went worng with deleting" });
            }
            _unitWork.Category.Remove(categoryDB);
            await _unitWork.Save();
            return Json(new { success = true, message = "Category was deleted successfully" });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool val = false;
            var list = await _unitWork.Category.RetrieveAll();
            
            if(id == 0)
            {
                val = list.Any(b => b.Name.ToLower().Trim() == name.ToLower().Trim());
            }
            else
            {
                val = list.Any(b => b.Name.ToLower().Trim() == name.ToLower().Trim() && b.Id != id);
            }
            if(val)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }
        #endregion

    }
}
