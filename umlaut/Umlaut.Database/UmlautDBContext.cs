using Microsoft.EntityFrameworkCore;
using Umlaut.Database.Models;


namespace Umlaut.Database

{
    public class UmlautDBContext: DbContext
    {

        public UmlautDBContext(DbContextOptions<UmlautDBContext> options) : base(options) { }

        public DbSet<Graduate> Graduates { get; set; }

        public DbSet<Faculties> Faculties { get; set; }

        public DbSet<Locations> Locations { get; set; }

        public DbSet<Specializations> Specializations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Graduate>().HasIndex(u => u.ResumeLink).IsUnique();
            modelBuilder.Entity<Locations>().HasIndex(u => u.Location).IsUnique();
            modelBuilder.Entity<Specializations>().HasIndex(u => u.Specialization).IsUnique();
            modelBuilder.Entity<Faculties>().HasIndex(u => u.Faculty).IsUnique();
        }
    }
}