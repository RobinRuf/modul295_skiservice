using System.ComponentModel.DataAnnotations;

namespace SkiService.Models.dto
{
    public class CreateCustomerDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
    }
}
