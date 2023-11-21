using System.ComponentModel.DataAnnotations;

namespace SkiService.Models.dto
{
    public class CreateServiceOrderDto
    {
        // Customer information
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        // Service order details
        public string ServiceType { get; set; }
        public string Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Sum { get; set; }
    }
}
