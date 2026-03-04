using Microsoft.AspNetCore.Mvc;
using PD411_Shop.Repositories;
using PD411_Shop.Services;
using PD411_Shop.ViewModels;

namespace PD411_Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductRepostitory _productRepo;

        public CartController(ProductRepostitory productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            var sessionItems = HttpContext.Session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();

            var allProducts = await _productRepo.GetProductsAsync();

            var cartList = sessionItems.Select(item => {
                var product = allProducts.FirstOrDefault(p => p.Id == item.ProductId);
                return new CartIndexVM
                {
                    ProductId = item.ProductId,
                    Name = product?.Name ?? "Unknown",
                    Price = product?.Price ?? 0,
                    Image = product?.Image,
                    Count = item.Count
                };
            }).ToList();

            return View(cartList);
        }

        public IActionResult Add(int id)
        {
            CartService.AddToCart(HttpContext.Session, id);
            return RedirectToAction("Index"); 
        }

        public IActionResult Remove(int id)
        {
            CartService.RemoveFromCart(HttpContext.Session, id);
            return RedirectToAction("Index");
        }

        public IActionResult Increment(int id)
        {
            CartService.Increment(HttpContext.Session, id);
            return RedirectToAction("Index");
        }

        public IActionResult Decrement(int id)
        {
            var items = HttpContext.Session.Get<List<CartItemVM>>();
            var item = items?.FirstOrDefault(i => i.ProductId == id);

            if (item != null && item.Count > 1)
            {
                CartService.Decrement(HttpContext.Session, id);
            }
            else
            {
                CartService.RemoveFromCart(HttpContext.Session, id);
            }
            return RedirectToAction("Index");
        }
    }
}