using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.ViewModels;

namespace PD411_Shop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<SelectListItem>> GetSelectCategoriesAsync()
        {
            List<CategoryModel> categories = await _context.Categories.ToListAsync();

            IEnumerable<SelectListItem> selectItems = categories
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()));


            // Приклад того самого коду без використання Select
            //List<SelectListItem> result = new List<SelectListItem>();
            //foreach (var c in categories)
            //{
            //    var item = new SelectListItem(c.Name, c.Id.ToString();
            //    result.Add(item);
            //}

            return selectItems;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .AsEnumerable();

            return View(products);
        }


        // GET
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateProductVM
            {
                SelectCategories = await GetSelectCategoriesAsync()
            };

            return View(viewModel);
        }

        // POST
        // [FromForm] - очікує дані у форматі multipart/form-data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]CreateProductVM vm)
        {
            if(!ModelState.IsValid)
            {
                vm.SelectCategories = await GetSelectCategoriesAsync();
                return View(vm);
            }

            ProductModel model = new ProductModel
            {
                Name = vm.Name ?? string.Empty,
                Amount = vm.Amount,
                Color = vm.Color,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId
            };

            // Save Image
            if(vm.Image != null)
            {
                string root = Directory.GetCurrentDirectory();
                string imagesPath = Path.Combine(root, "wwwroot", "images");
                string ext = Path.GetExtension(vm.Image.FileName);
                string name = Guid.NewGuid().ToString();
                string fileName = name + ext;
                string filePath = Path.Combine(imagesPath, fileName);

                using var imageStream = vm.Image.OpenReadStream();
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                imageStream.CopyTo(fileStream);

                model.Image = fileName;
            }

            await _context.Products.AddAsync(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product != null)
            {
                if(product.Image != null)
                {
                    string root = Directory.GetCurrentDirectory();
                    string imagesPath = Path.Combine(root, "wwwroot", "images");
                    string filePath = Path.Combine(imagesPath, product.Image);

                    if(System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
