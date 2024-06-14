using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheVoid.Models;

namespace TheVoid.Data
{
    public class VoidDbContext:IdentityDbContext<VoidUser>
    {
        public DbSet<VoidMessage> VoidMessages { get; set; }
        public VoidDbContext(DbContextOptions<VoidDbContext> options) : base(options)
        {
                
        }
    }
}
