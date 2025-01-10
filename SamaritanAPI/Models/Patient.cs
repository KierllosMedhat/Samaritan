using SamaritanAPI.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaritanAPI.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        [Length(11, 11, ErrorMessage = "Invalid Phone Number, Must be 11 digits!")]
        public required string PhoneNumber { get; set; }
        [Required]
        [Length(14, 14, ErrorMessage = "Invalid ID Number, Must be 14 digits!")]
        public required string NationalId { get; set; }
        [Range(0, 120)]
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public string? Job { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public int NoOfChildren { get; set; }
        public string? JobsOfChildren { get; set; }
        public int NoOfSiblings { get; set; }
        public string? JobsOfSiblings { get; set; }
        public required string MedicalCondition { get; set; }
        public required string QuarantineLocation { get; set; }
        public string?PatientCompanionName { get; set; }
        [Length(11, 11, ErrorMessage = "Invalid Phone Number, Must be 11 digits!")]
        public string? PatientCompanionPhone{ get; set; }
        public DateTime LastDonationReceived { get; set; }
        public int NoOfDonationsReceived { get; set; }
        public int NoOfBagsDonated { get; set; }
        public List<Request>? Requests { get; set; }
    }

    public enum MaritalStatus
    {
        Married,
        Single,
        Widow
    }
}
