using PD411_Shop.Models;

namespace PD411_Shop.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<ProductModel> Products { get; set; } = [];
        public IEnumerable<CategoryModel> Categories { get; set; } = [];
        public PaginationVM Pagination { get; set; } = new();
        public int? CategoryId { get; set; } = null;
    }
}
