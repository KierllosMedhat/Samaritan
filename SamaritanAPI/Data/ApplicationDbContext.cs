using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SamaritanAPI.Authentication;
using SamaritanAPI.Models;

namespace SamaritanAPI.Data
{
     public class ApplicationDbContext : IdentityDbContext<AppUser>
     {
          public DbSet<Request> Requests { get; set; }
          public DbSet<Donor> Donors { get; set; }
          public DbSet<Patient> Patients { get; set; }
          public DbSet<ServantCompanion> ServantCompanions { get; set; }
          public DbSet<Note> Notes { get; set; }
          public DbSet<Call> Calls { get; set; }
          public DbSet<Notification> Notifications { get; set; }
          private readonly IConfiguration configuration;

          public ApplicationDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
          {
               this.configuration = configuration;
          }
          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
               base.OnModelCreating(modelBuilder);

               // Donor
               modelBuilder.Entity<Donor>() // store the gender as String in the
                    .Property(d => d.Gender)// database instead of int values
                    .HasConversion<string>();

               // Patient
               modelBuilder.Entity<Patient>() // store the gender as String in the
                    .Property(d => d.Gender)// database instead of int values
                    .HasConversion<string>();

               // Patient
               modelBuilder.Entity<Patient>() // store the Marital status as String in the
                    .Property(d => d.MaritalStatus)// database instead of int values
                    .HasConversion<string>();

               // Call
               modelBuilder.Entity<Call>() // store the CallResponse as String in the
                    .Property(d => d.CallResponse)// database instead of int values
                    .HasConversion<string>();

          }
     }
}
