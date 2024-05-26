using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yetai_Eats.Model;

namespace Yetai_Eats.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<IndividualMenuItem> IndividualMenuItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        //public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<DiliveryRIder> DiliveryRIders { get; set; }
        public DbSet<PendingMessage> PendingMessages { get; set; }

    }
}
