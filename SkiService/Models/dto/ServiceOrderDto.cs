namespace SkiService.Models.dto
{
    public class ServiceOrderDto
    {
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
    }
}
