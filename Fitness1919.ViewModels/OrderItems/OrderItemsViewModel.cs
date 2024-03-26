namespace Fitness1919.Web.ViewModels.OrderItems
{
    public class OrderItemsViewModel
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public Data.Models.Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string OrderId { get; set; }
        public Data.Models.Order Order { get; set; }

    }
}
