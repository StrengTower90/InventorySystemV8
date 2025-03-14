using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Models.ViewModels;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventory)]
    public class ProductController : Controller
    {
        private readonly IWorkUnit _workOfUnit;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IWorkUnit workOfUnit, IWebHostEnvironment webHostEnvironment)
        {
            _workOfUnit = workOfUnit;
            _webHostEnvironment = webHostEnvironment;
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
                BrandList = _workOfUnit.Product.RetrieveAllDropdownList("Brand"),
                ParentList = _workOfUnit.Product.RetrieveAllDropdownList("Product")
            };

            if(id == null)
            {
                productVM.Product.State = true;
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

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductVM productVM)
        {
            if(ModelState.IsValid)
            {
                // Retrieve Uploaded Files from the form
                var files = HttpContext.Request.Form.Files; 
                /* Gets the root directory of the web application where static files are store,
                 * This is needed to save the uploaded image.
                 */
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(productVM.Product.Id == 0)
                {
                    //Create
                    string upload = webRootPath + DS.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    //Extract the file extension
                    string extension = Path.GetExtension(files[0].FileName);

                    /* Opens a new file stream in the specified path and copies the uploaded file to it*/
                    using(var fileStream = new FileStream(Path.Combine(upload,fileName+extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream); //Save the new image 
                    }
                    productVM.Product.ImageUrl = fileName + extension;
                    await _workOfUnit.Product.Add(productVM.Product);
                }
                else
                {
                    //Update 
                    var objProduct = await _workOfUnit.Product.RetrieveFirst(p => p.Id == productVM.Product.Id, isTracking: false);
                    if(files.Count > 0)
                    {
                        string upload = webRootPath + DS.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //Delete the previous image
                        var previousFile = Path.Combine(upload, objProduct.ImageUrl);
                        if(System.IO.File.Exists(previousFile))
                        {
                            System.IO.File.Delete(previousFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream); 
                        }
                        productVM.Product.ImageUrl = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.ImageUrl = objProduct.ImageUrl;
                    }
                    _workOfUnit.Product.Update(productVM.Product);
                }
                TempData[DS.Success] = "Transaction was successfully";
                await _workOfUnit.Save();
                return View("Index");
            }
            productVM.CategoryList = _workOfUnit.Product.RetrieveAllDropdownList("Category");
            productVM.BrandList = _workOfUnit.Product.RetrieveAllDropdownList("Brand");
            productVM.ParentList = _workOfUnit.Product.RetrieveAllDropdownList("Parent");
            return View(productVM);
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

            //Remove the Image
            string upload = _webHostEnvironment.WebRootPath + DS.ImagePath;
            var previousFile = Path.Combine(upload, productDB.ImageUrl);
            if(System.IO.File.Exists(previousFile))
            {
                System.IO.File.Delete(previousFile);
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
                value = list.All(b => b.SerialNumber.ToLower().Trim() == serie.ToLower().Trim());
            }
            else
            {
                value = list.Any(b => b.SerialNumber.ToLower().Trim() == serie.ToLower().Trim() && b.Id != id);
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
