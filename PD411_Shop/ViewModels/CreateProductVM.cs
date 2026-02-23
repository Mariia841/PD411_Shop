using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PD411_Shop.ViewModels
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [MaxLength(200, ErrorMessage = "Максимальна довжина 200 символів")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Потрібно вказати ціну")]
        [Range(0, 99999999, ErrorMessage = "К-сть повинна бути в діапазоні 0 - 99999999")]
        public double Price { get; set; } = 0d;
        [Required(ErrorMessage = "Потрібно вказати кількість")]
        [Range(0, 10000, ErrorMessage = "К-сть повинна бути в діапазоні 0 - 10000")]
        public int Amount { get; set; } = 0;
        [Required(ErrorMessage = "Обов'язкове поле")]
        [MaxLength(100, ErrorMessage = "Максимальна довжина 100 символів")]
        public string? Color { get; set; }
        public IFormFile? Image { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> SelectCategories { get; set; } = [];
    }
}
