using InventarySystem.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IWorkUnit _workUnit;

        public OrderController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> RetrieveOrderList()
        {
            var all = await _workUnit.Order.RetrieveAll(includeProperties: "UserApplication");
            return Json(new { data = all });
        }
        #endregion
    }
}
