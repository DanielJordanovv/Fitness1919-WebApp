using System.ComponentModel.DataAnnotations;

namespace Fitness1919.Web.ViewModels.Statistics
{
    public class StatisticsViewModel
    {
        [Display(Name = "Count Clients")]
        public int CountClients { get; set; }

        [Display(Name = "Count Products")]
        public int CountProducts { get; set; }

        [Display(Name = "Count Orders")]
        public int CountOrders { get; set; }

        [Display(Name = "Sum Orders")]
        public decimal SumOrders { get; set; }
    }
}
