using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheVoid.Models;

namespace TheVoid.Data
{
    public class VoidDbContext:IdentityDbContext<VoidUser>
    {
        public DbSet<VoidMessage> VoidMessages { get; set; }
        public DbSet<ItemData> Items { get; set; }
        public DbSet<StorageItemData> StorageItems { get; set; }

        public VoidDbContext(DbContextOptions<VoidDbContext> options) : base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItemData>()
                .HasOne(d => d.User)
                .WithMany(u => u.Items)
                .HasForeignKey(d => d.UserId);
        }
    }
}
