using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Models.ViewModels;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IWorkUnit _workUnit;

        [BindProperty]
        private OrderDetailVM orderDetailVM { get; set; }

        public OrderController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            orderDetailVM = new OrderDetailVM()
            {
                Order = await _workUnit.Order.RetrieveFirst(o => o.Id == id, includeProperties: "UserApplication"),
                OrderDetailList = await _workUnit.OrderDetail.RetrieveAll(d => d.OrderId == id,
                                                                           includeProperties:"Product"),
            };
            return View(orderDetailVM);
        }

        [Authorize(Roles = DS.Role_Admin)]
        public async Task<IActionResult> Process(int id)
        {
            var order = await _workUnit.Order.RetrieveFirst(o => o.Id == id);
            order.OrderStatus = DS.InProcessStatus;
            await _workUnit.Save();
            TempData[DS.Success] = "Order was changed to in Process status";
            return RedirectToAction("Detail", new { id = id });
        }

        [Authorize(Roles = DS.Role_Admin)]
        public async Task<IActionResult> SubmitOrder(OrderDetailVM orderDetailVM)
        {
            var order = await _workUnit.Order.RetrieveFirst(o => o.Id == orderDetailVM.Order.Id);
            order.OrderStatus = DS.SentStatus;
            order.Carrier = orderDetailVM.Order.Carrier;
            order.ShippingNumber = orderDetailVM.Order.ShippingNumber;
            order.SendDate = DateTime.Now;
            await _workUnit.Save();
            TempData[DS.Success] = "Order was changed to sent status";
            return RedirectToAction("Detail", new { id = orderDetailVM.Order.Id });
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> RetrieveOrderList(string status)
        {
            //var all = await _workUnit.Order.RetrieveAll(includeProperties: "UserApplication");
            //return Json(new { data = all });
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<Order> all;
            if (User.IsInRole(DS.Role_Admin)) // Validate the user role
            {
                all = await _workUnit.Order.RetrieveAll(includeProperties: "UserApplication");
            }
            else
            {
                all = await _workUnit.Order.RetrieveAll(o => o.UserApplicationId == claim.Value,includeProperties: "UserApplication");
            }
            // Validate the status
            switch (status)
            {
                case "approved":
                    all = all.Where(o => o.OrderStatus == DS.ApprovedStatus);
                    break;
                case "completed":
                    all = all.Where(o => o.OrderStatus == DS.SentStatus);
                    break;
                default:
                    break;
            }

            return Json(new { data = all });
        }
        #endregion
    }
}
