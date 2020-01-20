using Microsoft.EntityFrameworkCore;

namespace dojo_activity_center.Models
{
    public class DojoActivityContext : DbContext
    {
        public DojoActivityContext(DbContextOptions<DojoActivityContext> options) : base(options) { }
        public DbSet<User> Users {get; set;}
        public DbSet<Activity> Activities {get; set;}
        public DbSet<Participate> Participate {get; set;}
    }
}