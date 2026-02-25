using System.ComponentModel.DataAnnotations;

namespace PD411_Shop.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва категорії є обов'язковою")]
        [MinLength(3, ErrorMessage = "Мінімальна довжина — 3 символи")]
        [MaxLength(50, ErrorMessage = "Максимальна довжина — 50 символів")]
        [Display(Name = "Назва категорії")]
        public string Name { get; set; } = string.Empty;
    }
}