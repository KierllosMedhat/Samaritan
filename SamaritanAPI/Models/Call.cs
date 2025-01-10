using SamaritanAPI.Authentication;
using SamaritanAPI.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaritanAPI.Models
{
    public class Call
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("ServantDialler")]
        public required string ServantDiallerId { get; set; }
        public required AppUser ServantDialler { get; set; }
        [Required]
        [ForeignKey("Donor")]
        public int DonorId { get; set; }
        [Required]
        public required Donor Donor { get; set; }
        public DateTime DateTime { get; set; }
        public CallResponse CallResponse { get; set; }
        public bool willDonate { get; set; }
        public string? Note { get; set; }
    }
}
