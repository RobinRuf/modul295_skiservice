using System.ComponentModel.DataAnnotations;

namespace SkiService.Models
{
    public class ServiceOrderModel
    {
        [Key]
        public int OrderID { get; set; }
        public int? EmployeeID { get; set; }
        public int CustomerID { get; set; }
        public int? ServiceType { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string? Comment { get; set; }
        public string Sum { get; set; }

        // Navigation Properties (A service can be assigned to 0..1 employee)
        public EmployeeModel? Employee { get; set; }
        public CustomerModel Customer { get; set; }
        public ServiceModel Service { get; set; }
        public StatusModel StatusInfo { get; set; }
        public PriorityModel PriorityInfo { get; set; }
    }
}
