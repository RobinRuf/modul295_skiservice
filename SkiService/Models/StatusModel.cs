using System.ComponentModel.DataAnnotations;

namespace SkiService.Models
{
    public class StatusModel
    {
        [Key]
        public int ID { get; set; }
        public string Status { get; set; }

        // Navigation Property
        public List<ServiceOrderModel> ServiceOrders { get; set; }
    }
}
