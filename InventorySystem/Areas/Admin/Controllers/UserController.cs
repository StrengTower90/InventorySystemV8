using InventarySystem.DataAccess.Data;
using InventarySystem.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IWorkUnit _workOfUnit;
        private readonly ApplicationDbContext _db;

        public UserController(IWorkUnit workOfUnit, ApplicationDbContext db)
        {
            _workOfUnit = workOfUnit;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> RetrieveAll()
        {
            var userList = await _workOfUnit.UserApplication.RetrieveAll();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public async Task<IActionResult> LockAndUnlock([FromBody] string id)
        {
            var user = await _workOfUnit.UserApplication.RetrieveFirst(u => u.Id == id);
            if(user==null)
            {
                return Json(new { success = false, message = "User error" });
            }
            if(user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                // User Locked
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await _workOfUnit.Save();
            return Json(new { success = true, message = "Operation was successfully" });
        }

        #endregion
    }
}
