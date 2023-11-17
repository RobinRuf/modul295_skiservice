using System.ComponentModel.DataAnnotations;

namespace SkiService.Models.dto
{
    public class ServiceOrderDto
    {
        public int? EmployeeID { get; set; }

        [Required]
        public string ServiceType { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CompletionDate { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }

        [Required]
        [StringLength(255)]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string CustomerEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerPhone { get; set; }
    }
}
