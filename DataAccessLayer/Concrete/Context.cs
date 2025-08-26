using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Intersection>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<IntersectionChangeRequest>()
                .HasOne(r => r.RequestedBy)
                .WithMany()
                .HasForeignKey(r => r.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IntersectionChangeRequest>()
                .HasOne(r => r.ReviewedBy)
                .WithMany()
                .HasForeignKey(r => r.ReviewedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IntersectionChangeRequest>()
                .HasOne(r => r.Intersection)
                .WithMany()
                .HasForeignKey(r => r.IntersectionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IntersectionChangeRequest>()
                .HasIndex(x => new { x.IntersectionID, x.Status })
                .HasFilter("[IntersectionID] IS NOT NULL AND [Status] = 0")
                .IsUnique();
            modelBuilder.Entity<Intersection>()
                .HasOne(i => i.User)
                .WithMany(u => u.Intersections)
                .HasForeignKey(i => i.UserID)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Intersection> Intersections { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<IntersectionImage> IntersectionsImages { get; set; }
        public DbSet<Substructure> Substructures { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
