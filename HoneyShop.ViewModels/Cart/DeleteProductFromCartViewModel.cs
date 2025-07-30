namespace HoneyShop.ViewModels.Cart
{
    public class DeleteProductFromCartViewModel
    {
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
