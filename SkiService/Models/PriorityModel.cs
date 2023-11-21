using System.ComponentModel.DataAnnotations;

namespace SkiService.Models
{
    public class PriorityModel
    {
        [Key]
        public int ID { get; set; }
        public string Priority { get; set; }

        // Navigation Property
        public List<ServiceOrderModel> ServiceOrders { get; set; }
    }
}
