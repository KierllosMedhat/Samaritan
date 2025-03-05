using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SamaritanAPI.Models;
using SamaritanAPI.Models.Types;

namespace SamaritanAPI.Authentication
{
    public class AddOrUpdateAppUserModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        public required string FullName { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; } = string.Empty;
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "PhoneNumber is required")]
        [Length(11,11,ErrorMessage = "Invalid Phone Number, Must be 11 digits!")]
        public required string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public required string Role { get; set;}
        public Status Status { get; set; } = Status.Offline;
        // public BloodGroup? BloodGroup { get; set; }
        // public List<Donor>? Donors { get; set; }
    }
}