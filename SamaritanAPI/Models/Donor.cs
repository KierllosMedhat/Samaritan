using SamaritanAPI.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaritanAPI.Models
{
    public class Donor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Range(18,80)]
        public int Age { get; set; }
        public Gender Gender { get; set; }
        [Required]
        [Length(11,11,ErrorMessage = "Invalid Phone Number, Must be 11 digits!")]
        public required string PhoneNumber { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public DateTime LastDonationDate { get; set; }
        public DateTime Availability { get; set; }
        //public int ServantDiallerId { get; set; }
        public List<Call>? CallLogs { get; set; }
        public List<Note>? Notes { get; set; }
        public List<Request>? Requests { get; set; }
    }
}
