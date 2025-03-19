using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models.ViewModels;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventorySystem.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class CartController : Controller
    {
        private readonly IWorkUnit _workUnit;

        [BindProperty]
        public CartShoppingVM cartShoppingVM { get; set; }

        public CartController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            cartShoppingVM = new CartShoppingVM();
            cartShoppingVM.Order = new InventarySystem.Models.Order();
            cartShoppingVM.ShoppingCartsList = await _workUnit.ShoppingCart.RetrieveAll(
                                                    u => u.UserApplicationId == claim.Value,
                                                    includeProperties:"Product");
            cartShoppingVM.Order.TotalOrder = 0;
            cartShoppingVM.Order.UserApplicationId = claim.Value;

            foreach(var list in cartShoppingVM.ShoppingCartsList)
            {
                list.Price = list.Product.Price; //always show the current price of the product
                cartShoppingVM.Order.TotalOrder += (list.Price * list.Amount);
            }

            return View(cartShoppingVM);
        }

        public async Task<IActionResult> plus(int cartId)
        {
            var shoppingCart = await _workUnit.ShoppingCart.RetrieveFirst(c => c.Id == cartId);
            shoppingCart.Amount += 1;
            
            await _workUnit.Save();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> less(int cartId)
        {
            var shoppingCart = await _workUnit.ShoppingCart.RetrieveFirst(c => c.Id == cartId);

            if(shoppingCart.Amount == 1)
            {
                //Remove the shopping card record and update the session
                var cartList = await _workUnit.ShoppingCart.RetrieveAll(
                                            c => c.UserApplicationId == shoppingCart.UserApplicationId);
                var amountProducts = cartList.Count();
                _workUnit.ShoppingCart.Remove(shoppingCart);
                await _workUnit.Save();
                HttpContext.Session.SetInt32(DS.ssShoppingCart, amountProducts - 1);
            }
            else
            {
                shoppingCart.Amount -= 1;
                await _workUnit.Save();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> remove(int cartId)
        {
            //Remove the shopping cart record and update the session
            var shoppingCart = await _workUnit.ShoppingCart.RetrieveFirst(c => c.Id == cartId);
            var cartList = await _workUnit.ShoppingCart.RetrieveAll(
                                        c => c.UserApplicationId == shoppingCart.UserApplicationId);
            var productsAmount = cartList.Count();
            _workUnit.ShoppingCart.Remove(shoppingCart);
            await _workUnit.Save();
            HttpContext.Session.SetInt32(DS.ssShoppingCart, productsAmount - 1);
            return RedirectToAction("Index");
        }
    }
}
