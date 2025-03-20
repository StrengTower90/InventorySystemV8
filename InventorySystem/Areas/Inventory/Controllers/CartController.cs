using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using InventarySystem.Models.ViewModels;
using InventarySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventorySystem.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class CartController : Controller
    {
        private readonly IWorkUnit _workUnit;
        private string _webUrl;

        [BindProperty]
        public CartShoppingVM cartShoppingVM { get; set; }

        public CartController(IWorkUnit workUnit, IConfiguration configuration)
        {
            _workUnit = workUnit;
            _webUrl = configuration.GetValue<string>("DomainUrls:WEB_URL");
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

        public async Task<IActionResult> Procced()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            cartShoppingVM = new CartShoppingVM()
            {
                Order = new InventarySystem.Models.Order(),
                ShoppingCartsList = await _workUnit.ShoppingCart.RetrieveAll(
                                c => c.UserApplicationId == claim.Value, includeProperties: "Product"),
                Company = await _workUnit.Company.RetrieveFirst()
            };

            cartShoppingVM.Order.TotalOrder = 0;
            cartShoppingVM.Order.UserApplication = await _workUnit.UserApplication.RetrieveFirst(u => u.Id == claim.Value);

            foreach (var list in cartShoppingVM.ShoppingCartsList)
            {
                list.Price = list.Product.Price;
                cartShoppingVM.Order.TotalOrder += (list.Price * list.Amount);
            }
            cartShoppingVM.Order.ClientNames = cartShoppingVM.Order.UserApplication.Names + " " +
                                               cartShoppingVM.Order.UserApplication.LastNames;
            cartShoppingVM.Order.Telephone = cartShoppingVM.Order.UserApplication.PhoneNumber;
            cartShoppingVM.Order.Address = cartShoppingVM.Order.UserApplication.Address;
            cartShoppingVM.Order.Country = cartShoppingVM.Order.UserApplication.Country;
            cartShoppingVM.Order.City = cartShoppingVM.Order.UserApplication.City;

            // Manage Stock
            foreach (var list in cartShoppingVM.ShoppingCartsList)
            {
                // capture the stock by every product
                var product = await _workUnit.StoreProduct.RetrieveFirst(b => b.ProductId == list.ProductId &&
                                                                        b.StoreId == cartShoppingVM.Company.StoreSaleId);
                if (list.Amount > product.Amount)
                {
                    TempData[DS.Error] = "The Product amount " + list.Product.Description +
                                          "Exceeds the current stock (" + product.Amount + ")";
                    return RedirectToAction("Index");
                }
            }
            return View(cartShoppingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Procced(CartShoppingVM cartShoppingVM)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            cartShoppingVM.ShoppingCartsList = await _workUnit.ShoppingCart.RetrieveAll(
                                                   c => c.UserApplicationId == claim.Value,
                                                   includeProperties: "Product");
            cartShoppingVM.Company = await _workUnit.Company.RetrieveFirst();
            cartShoppingVM.Order.TotalOrder = 0;
            cartShoppingVM.Order.UserApplicationId = claim.Value;
            cartShoppingVM.Order.OrderDate = DateTime.Now;

            foreach(var list in cartShoppingVM.ShoppingCartsList)
            {
                list.Price = list.Product.Price;
                cartShoppingVM.Order.TotalOrder += (list.Price + list.Amount);
            }
            // Control the Stock
            foreach(var list in cartShoppingVM.ShoppingCartsList)
            {
                var product = await _workUnit.StoreProduct.RetrieveFirst(b => b.ProductId == list.ProductId &&
                                                                b.StoreId == cartShoppingVM.Company.StoreSaleId);
                if(list.Amount > product.Amount)
                {
                    TempData[DS.Error] = "The Product amount " + list.Product.Description +
                                          "Excceds the current stock (" + product.Amount + ")";
                    return RedirectToAction("Index");
                }
            }
            cartShoppingVM.Order.OrderStatus = DS.PendingStatus;
            cartShoppingVM.Order.PaymentStatus = DS.PaymentPendingStatus;
            await _workUnit.Order.Add(cartShoppingVM.Order);
            await _workUnit.Save();
            // Saved Order detail
            foreach (var list in cartShoppingVM.ShoppingCartsList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = list.ProductId,
                    OrderId = cartShoppingVM.Order.Id,
                    Price = list.Price,
                    Amount = list.Amount
                };
                await _workUnit.OrderDetail.Add(orderDetail);
                await _workUnit.Save();
            }
            // Stripe
            var user = await _workUnit.UserApplication.RetrieveFirst(u => u.Id == claim.Value);
            var options = new SessionCreateOptions
            {
                SuccessUrl = _webUrl + $"Inventory/Cart/OrderConfirmation?id={cartShoppingVM.Order.Id}",
                CancelUrl = _webUrl + "Inventory/Cart/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.Email
            };
            foreach (var list in cartShoppingVM.ShoppingCartsList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    { 
                        UnitAmount = (long)(list.Price * 100), //$20 => 200
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = list.Product.Description
                        }
                    },
                    Quantity = list.Amount
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _workUnit.Order.UpdatePaymentStripeId(cartShoppingVM.Order.Id, session.Id, session.PaymentIntentId);
            await _workUnit.Save();
            Response.Headers.Add("Location", session.Url); // Redirect to Stripe
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _workUnit.Order.RetrieveFirst(o => o.Id == id, includeProperties: "UserApplication");
            var service = new SessionService();
            Session session = service.Get(order.SessionId);
            var shoppingCart = await _workUnit.ShoppingCart.RetrieveAll(u => u.UserApplicationId == order.UserApplicationId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _workUnit.Order.UpdatePaymentStripeId(id, session.Id, session.PaymentIntentId);
                _workUnit.Order.UpdateStatus(id, DS.ApprovedStatus, DS.PaymentApprovedStatus);
                await _workUnit.Save();

                // Reduce stock in the Store Sale
                var company = await _workUnit.Company.RetrieveFirst();
                foreach (var list in shoppingCart)
                {
                    var storeProduct = new StoreProduct();
                    storeProduct = await _workUnit.StoreProduct.RetrieveFirst(b => b.ProductId == list.ProductId
                                                                        && b.StoreId == company.StoreSaleId);
                    await _workUnit.KardexInventory.RegisterKardex(storeProduct.Id, "Exit",
                                                                        "Sale - Order# " + id,
                                                                        storeProduct.Amount,
                                                                        list.Amount,
                                                                        order.UserApplicationId);
                    storeProduct.Amount -= list.Amount;
                    await _workUnit.Save();
                }
            }
            //Delete the shopping cart
            List<ShoppingCart> shoppingCartList = shoppingCart.ToList();
            _workUnit.ShoppingCart.RemoveRange(shoppingCartList);
            await _workUnit.Save();
            HttpContext.Session.SetInt32(DS.ssShoppingCart, 0);

            return View(id);
        }
    }
}
