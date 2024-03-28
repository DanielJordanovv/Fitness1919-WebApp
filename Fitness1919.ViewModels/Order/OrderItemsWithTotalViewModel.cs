using Fitness1919.Web.ViewModels.OrderItems;

public class OrderItemsWithTotalViewModel
{
    public List<OrderItemsViewModel> OrderItems { get; set; }
    public decimal TotalPrice { get; set; }
}
