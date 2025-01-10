using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SamaritanAPI.Authentication;

namespace SamaritanAPI.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public required string UserId { get; set; }
        public required AppUser User { get; set; }
        [Required]
        public required string Text { get; set; }
        public bool IsRead { get; set; }
    }
}
