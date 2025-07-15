namespace HoneyShop.ViewModels.Cart
{
    public class GetAllCartItemsViewModel
    {
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual GetProductDetailsForCartViewModel ProductDetails { get; set; } = null!;
    }
}
