using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Models;
using System.Text.Json;

namespace PD411_Shop.Data.Initalizer
{
    public static class Seeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Застосування міграцій
            context.Database.Migrate();

            // 2. Roles & Users (Ваш код)
            if (!roleManager.Roles.Any())
            {
                var adminRole = new IdentityRole { Name = "admin" };
                var userRole = new IdentityRole { Name = "user" };

                roleManager.CreateAsync(adminRole).Wait();
                roleManager.CreateAsync(userRole).Wait();

                var admin = new UserModel
                {
                    Email = "admin@mail.com",
                    UserName = "admin",
                    EmailConfirmed = true,
                    FirstName = "John",
                    LastName = "Doe"
                };

                var user = new UserModel
                {
                    Email = "user@mail.com",
                    UserName = "user",
                    EmailConfirmed = true,
                    FirstName = "Joe",
                    LastName = "Biden"
                };

                userManager.CreateAsync(admin, "qwerty").Wait();
                userManager.CreateAsync(user, "qwerty").Wait();

                userManager.AddToRoleAsync(admin, "admin").Wait();
                userManager.AddToRoleAsync(user, "user").Wait();
            }

            // 3. Categories & Products
            if (!context.Categories.Any())
            {
                string root = Directory.GetCurrentDirectory();
                string folderPath = Path.Combine(root, "wwwroot", "seed_data");
                string filePath = Path.Combine(folderPath, "components.json");

                List<CategoryModel>? categories;

                if (File.Exists(filePath))
                {
                    // Якщо файл існує — читаємо з нього
                    string json = File.ReadAllText(filePath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    categories = JsonSerializer.Deserialize<List<CategoryModel>>(json, options);
                }
                else
                {
                    // Якщо файлу немає — генеруємо дані програмно
                    categories = GenerateDefaultData();

                    // (Опціонально) Зберігаємо згенероване в JSON, щоб ви могли його правити
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                    string jsonString = JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(filePath, jsonString);
                }

                if (categories != null)
                {
                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                }
            }
        }

        private static List<CategoryModel> GenerateDefaultData()
        {
            var categories = new List<CategoryModel>();
            var rand = new Random();

            var categoryNames = new[] {
                "Процесори", "Відеокарти", "Материнські плати", "Оперативна пам'ять",
                "Блоки живлення", "SSD Накопичувачі", "Корпуси", "Охолодження",
                "Монітори", "Периферія"
            };

            var brands = new Dictionary<string, string[]>
            {
                { "Процесори", new[] { "Intel Core i7", "AMD Ryzen 7", "Intel Core i5" } },
                { "Відеокарти", new[] { "NVIDIA RTX 4060", "NVIDIA RTX 4070", "AMD Radeon 7700" } },
                { "Материнські плати", new[] { "ASUS Prime", "MSI Tomahawk", "Gigabyte Gaming" } },
                { "Оперативна пам'ять", new[] { "Kingston Fury", "Corsair Vengeance" } },
                { "Блоки живлення", new[] { "Chieftec", "SeaSonic", "be quiet!" } },
                { "SSD Накопичувачі", new[] { "Samsung Evo", "Crucial P3", "WD Blue" } },
                { "Корпуси", new[] { "NZXT", "Deepcool", "Fractal Design" } },
                { "Охолодження", new[] { "Noctua", "Arctic", "Cooler Master" } },
                { "Монітори", new[] { "LG Ultra", "Samsung Odyssey", "Dell S-series" } },
                { "Периферія", new[] { "Logitech G", "Razer Deathadder", "Hator" } }
            };

            foreach (var catName in categoryNames)
            {
                var category = new CategoryModel
                {
                    Name = catName,
                };

                var productList = new List<ProductModel>();
                string[] currentBrands = brands.ContainsKey(catName) ? brands[catName] : new[] { "Generic" };

                for (int i = 1; i <= 20; i++)
                {
                    string brand = currentBrands[rand.Next(currentBrands.Length)];
                    productList.Add(new ProductModel
                    {
                        Name = $"{brand} #{i} Gen",
                        Description = $"Професійне рішення для вашого ПК. Категорія: {catName}. Гарантія 36 місяців.",
                        Price = rand.Next(10, 500) * 100,
                        Amount = rand.Next(5, 50),
                        Color = i % 2 == 0 ? "Black" : "White",
                        CreatedDate = DateTime.UtcNow,
                        Image = "/images/products/no-image.png"
                    });
                }

                category.GetType().GetProperty("Products")?.SetValue(category, productList);

                categories.Add(category);
            }

            return categories;
        }
    }
}