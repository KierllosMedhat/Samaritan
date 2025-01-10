using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaritanAPI.Models
{
    public class ServantCompanion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        [Length(11, 11, ErrorMessage = "Invalid Phone Number, Must be 11 digits!")]
        public required string PhoneNumber { get; set; }
        public Degree Degree { get; set; }
        public Faculty Faculty { get; set; }
        public bool HaveACar { get; set; }
        public List<Note>? Notes { get; set; }
        public List<Request>? Requests { get; set; }
        //TODO: Reports
    }

    public enum Degree
    {
        Student,
        Graduate
    }

    public enum Faculty 
    {
        Medicine,
        Dentistry,
        Pharmacy,
        PhysicalTherapy,
        Veterinary
    }
}
