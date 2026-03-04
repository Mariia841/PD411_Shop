namespace PD411_Shop.ViewModels
{
    public class CartIndexVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Count { get; set; }
        public string? Image { get; set; }

        public double TotalPrice => Price * Count;
    }
}
