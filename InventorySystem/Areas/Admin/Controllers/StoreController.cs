using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")] // Always indicate the Area that belongs
    [Authorize(Roles = DS.Role_Admin)] 
    public class StoreController : Controller
    {
        private readonly IWorkUnit _unitedOfWork;

        public StoreController(IWorkUnit unitOfWork)
        {
            _unitedOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Store store = new Store();

            if(id==null)
            {
                //Create a new Store
                store.State = true;
                return View(store);
            }
            // Update Store
            store = await _unitedOfWork.Store.Retrieve(id.GetValueOrDefault());
            if(store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Avoid falsifyig requests to try load data 
        public async Task<IActionResult> Upsert(Store store)
        {
            if(ModelState.IsValid) // Validate that everything inside our Model comes right (props etc..)
            {
                if(store.Id == 0)
                {
                    await _unitedOfWork.Store.Add(store);
                    TempData[DS.Success] = "Store created successfully";
                }
                else
                {
                    _unitedOfWork.Store.Update(store);
                    TempData[DS.Success] = "Store updated successfully";
                }
                await _unitedOfWork.Save();
                return RedirectToAction(nameof(Index)); // In Write methods doesn't not return a View instead of we use redirect
            }
            TempData[DS.Error] = "Error saving";
            return View(store); // If doesn't apply changes redirect to the same views
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> RetrieveAll()
        {
            var all = await _unitedOfWork.Store.RetrieveAll();
            return Json(new { data = all });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var storeDb = await _unitedOfWork.Store.Retrieve(id);
            if (storeDb == null)
            {
                return Json(new { success = false, message = "Something went wrong deleting" });
            }
            _unitedOfWork.Store.Remove(storeDb);
            await _unitedOfWork.Save();
            return Json(new { success = true, message = "Store deleted succesfully" });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool val = false;
            var list = await _unitedOfWork.Store.RetrieveAll();
            if(id==0)
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
