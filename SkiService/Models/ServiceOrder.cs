namespace SkiService.Models
{
    public class ServiceOrder
    {
        public int OrderID { get; set; }
        public int? EmployeeID { get; set; }
        public string ServiceType { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string? Comments { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        // Navigation Properties (A service can be assigned to 0..1 employee)
        public Employee Employee { get; set; }
    }


}
