namespace PD411_Shop.ViewModels
{
    public class EditProductVM : CreateProductVM
    {
        public int Id { get; set; }
        public string? ExistingImage { get; set; }
    }
}