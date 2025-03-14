using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class BrandController : Controller
    {
        private readonly IWorkUnit _unitOfWork;

        public BrandController(IWorkUnit unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Brand brand = new Brand();

            if(id == null)
            {
                brand.State = true;
                return View(brand);
            }
            brand = await _unitOfWork.Brand.Retrieve(id.GetValueOrDefault());
            if(brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Brand brand)
        {
          if(ModelState.IsValid)
            {
                if(brand.Id == 0)
                {
                    await _unitOfWork.Brand.Add(brand);
                    TempData[DS.Success] = "Brand was created successfully";
                }
                else
                {
                    _unitOfWork.Brand.Update(brand);
                    TempData[DS.Success] = "Brand was updated successfully";
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Something went wrong with save";
            return View(brand);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> RetrieveAll()
        {
            var all = await _unitOfWork.Brand.RetrieveAll();
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var brandDB = await _unitOfWork.Brand.Retrieve(id);
            if(brandDB == null)
            {
                return Json(new { success = false, message = "Error when deleting" });
            }
            _unitOfWork.Brand.Remove(brandDB);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Deleting was succesfully" });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool value = false;
            var list = await _unitOfWork.Brand.RetrieveAll();
            if(id == 0)
            {
                value = list.Any(b => b.Name.ToLower().Trim() == name.ToLower().Trim());
            }
            else
            {
                value = list.Any(b => b.Name.ToLower().Trim() == name.ToLower().Trim() && b.Id != id);
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
