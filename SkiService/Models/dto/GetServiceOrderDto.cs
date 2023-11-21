namespace SkiService.Models.dto
{
    public class GetServiceOrderDto
    {
        public int OrderID { get; set; }
        public string? EmployeeName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ServiceType { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string? Comment { get; set; }
        public string Sum { get; set; }
    }
}
