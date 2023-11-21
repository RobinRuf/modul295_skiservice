using System.ComponentModel.DataAnnotations;

namespace SkiService.Models
{
    public class CustomerModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Navigation Property (Every Customer can have multiple ServiceOrders
        public List<ServiceOrderModel> ServiceOrders { get; set; }
    }
}
