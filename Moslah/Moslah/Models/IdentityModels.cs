using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moslah.Models.OwnModels;

namespace Moslah.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            news = new HashSet<News>();
            description = new HashSet<Description>();

        }

        [Required, StringLength(50)]
        public string Name { get; set; }
        [Range(minimum: 18, maximum: 100)]
        public int? Age { get; set; }
        public string Phone { get; set; }
        [StringLength(20)]
        public string City { get; set; }
        public virtual ICollection<News> news { get; set; }
        public virtual ICollection<Description> description { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<City> Cities { set; get; }
        public DbSet<Stations> Stations { set; get; }
        public DbSet<BusLocation> BusLocations { set; get; }
        public DbSet<MetroLocation> MetroLocations{ set; get; }
        public DbSet<MicroBus> Microbus { set; get; }
        public DbSet<News> News{ set; get; }
        public DbSet<Description> Description { set; get; }
        public DbSet<QuickSearch> Quicksearch { set; get; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}