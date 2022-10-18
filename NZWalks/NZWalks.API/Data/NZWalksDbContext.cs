using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext 
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options ) : base( options )
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role>()
                .HasOne(x=>x.role)
                .WithMany(y=>y.userRoles)
                .HasForeignKey(x=>x.RoleId);

            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.user)
                .WithMany(y => y.userRoles)
                .HasForeignKey(x => x.UserId);
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User_Role> Users_Roles { get; set; }    
    }
   
   
}
