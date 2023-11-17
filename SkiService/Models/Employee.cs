namespace SkiService.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        // Navigation Property (every employee can be assigned to 0..* ServiceOrders)
        public List<ServiceOrder> ServiceOrders { get; set; }
    }

}
