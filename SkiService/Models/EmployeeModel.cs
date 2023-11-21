using System.ComponentModel.DataAnnotations;

namespace SkiService.Models
{
    public class EmployeeModel
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int LoginAttempts { get; set; }
        public bool IsLocked { get; set; }

        // Navigation Property (every employee can be assigned to 0..* ServiceOrders)
        public List<ServiceOrderModel> ServiceOrders { get; set; }
    }

}
