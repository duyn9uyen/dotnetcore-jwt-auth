using System.Xml;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore_jwt_auth.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<LoginModel> LoginModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<LoginModel>(entity =>
            {
                entity.ToTable("LoginModel");
                entity.HasKey(x => new { x.Id });
            });
        }
    }
}