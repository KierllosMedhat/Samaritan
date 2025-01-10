using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SamaritanAPI.Models;
using SamaritanAPI.Models.Types;

namespace SamaritanAPI.Authentication
{
    public class AppUser : IdentityUser
    {
        public required string FullName { get; set; }
        public Status? Status { get; set; }
        public BloodGroup? BloodGroup { get; set; }
        public List<Donor>? DonorsList { get; set; }

        public List<Notification>? Notifications { get; set; }
    }
}