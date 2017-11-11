namespace bookstore.ViewModels
{
    public class UpdateShoppingCartItemViewModel
    {
        public int BookId { get; set; }
        public bool IsRemoved { get; set; }
        public int Quantity { get; set; }
    }
}