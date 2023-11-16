using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Modul295_SkiService_WebAPI.Models
{
    public class User
    {
        [Key] // Definiert diese Eigenschaft als den Primärschlüssel.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Definiert Auto-Increment.
        public int Id { get; set; }

        [Required]
        [StringLength(100)] // Maximale Länge von 100 Zeichen.
        public string Username { get; set; }
        public DateTime ReleaseDate { get; set; }

        [StringLength(40)] // Maximale Länge von 100 Zeichen.
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
