using Microsoft.EntityFrameworkCore;
using DBModels;

namespace DBContext
{
    public class DataBaseContext: DbContext
    {
        private readonly string Host = "0xc00007b.site";
        private readonly string Port = "28008";
        private readonly string Database_name = "Umlaut";
        private readonly string Username = "postgres";
        private readonly string Password = "";

        public DbSet<Graduate> Graduates { get; set; }

        public DbSet<Faculties> Faculties { get; set; }

        public DbSet<Locations> Locations { get; set; }

        public DbSet<Specializations> Specializations { get; set; }

        public DataBaseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={Host};Port={Port};Database={Database_name};Username={Username};Password={Password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Graduate>().HasIndex(u => u.ResumeLink).IsUnique();
            modelBuilder.Entity<Locations>().HasIndex(u => u.Location).IsUnique();
            modelBuilder.Entity<Specializations>().HasIndex(u => u.Specialization).IsUnique();
            modelBuilder.Entity<Faculties>().HasIndex(u => u.Faculty).IsUnique();
        }
    }
}