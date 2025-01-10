using SamaritanAPI.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaritanAPI.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public RequestLevel RequestLevel { get; set; }
        public string RequestTimeline { get; set; } = string.Empty;
        public required Patient Patient { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public required string PatientFullName { get; set; }
        public required string MedicalCondition { get; set; }
        public int nBloodBagsReq { get; set; }
        public int nFreshBloodBagsReq { get; set; }
        public int nPlateletsReq { get; set; }
        public int nPlasmaReq { get; set; }
        public required string DonationLocation { get; set; }
        public DateTime DonationDate { get; set; }
        public int HemoglobinLevel { get; set; }
        public required string PatientCompanionName { get; set; }
        [Length(11, 11, ErrorMessage = "Invalid Phone Number, Must be 11 digits!")]
        public required string PatientCompanionPhone1 { get; set; }
        [Length(11, 11, ErrorMessage = "Invalid Phone Number, Must be 11 digits!")]
        public string? PatientCompanionPhone2 { get; set; }
        public string? PatientCompanionJob { get; set; }
        public bool CanAfford { get; set; }
        [ForeignKey("Donor")]
        public int DonorId { get; set; }
        public Donor? Donor { get; set; }
        [ForeignKey("ServantCompanion")]
        public int ServantCompanionId { get; set; }
        public ServantCompanion? ServantCompanion { get; set; }

    }
}
