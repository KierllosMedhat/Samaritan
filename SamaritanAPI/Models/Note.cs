using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaritanAPI.Models
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Donor")]
        public int DonorId { get; set; }
        [Required]
        public required Donor Donor { get; set; }
        [Required]
        public required string NoteText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModification { get; set; }
        //public int AuthorId { get; set; }
    }
}
