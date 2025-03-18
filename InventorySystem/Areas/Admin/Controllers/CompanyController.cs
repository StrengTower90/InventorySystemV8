using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Models.ViewModels;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IWorkUnit _workUnit;

        public CompanyController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

       public async Task<IActionResult> Upsert()
        {
            CompanyVM companyVM = new CompanyVM()
            {
                Company = new Company(),
                StoreList = _workUnit.Inventory.RetrieveAllDropdownList("Store")
            };

            companyVM.Company = await _workUnit.Company.RetrieveFirst();

            if(companyVM.Company == null)
            {
                companyVM.Company = new Company();
            }

            return View(companyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CompanyVM companyVM)
        {
            if(ModelState.IsValid)
            {
                TempData[DS.Success] = "Company was added successfully";
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if(companyVM.Company.Id == 0) //Create company
                {
                    companyVM.Company.CreatedById = claim.Value;
                    companyVM.Company.UpdatedById = claim.Value;
                    companyVM.Company.CreationDate = DateTime.Now;
                    companyVM.Company.UpdateDate = DateTime.Now;
                    await _workUnit.Company.Add(companyVM.Company);
                }
                else // Update company
                {
                    companyVM.Company.UpdatedById = claim.Value;
                    companyVM.Company.UpdateDate = DateTime.Now;
                    _workUnit.Company.Update(companyVM.Company);
                }
                await _workUnit.Save();
                return RedirectToAction("Index", "Home", new {area="Inventory"});
            }
            TempData[DS.Error] = "Something went wrong while save company";
            return View(companyVM);
        }
    }
}
