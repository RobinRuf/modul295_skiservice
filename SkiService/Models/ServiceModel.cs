using System.ComponentModel.DataAnnotations;

namespace SkiService.Models
{
    public class ServiceModel
    {
        [Key]
        public int ID { get; set; }
        public string ServiceType { get; set; }

        // Navigation Property
        public List<ServiceOrderModel> ServiceOrders { get; set; }
    }
}
