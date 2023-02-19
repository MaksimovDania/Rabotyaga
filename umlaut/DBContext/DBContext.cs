using Microsoft.EntityFrameworkCore;
using DBModels;

namespace DBContext
{
    public class DataBaseContext: DbContext
    {
        private const string Host = "";
        private const string Port = "";
        private const string database = "Umlaut";
        private const string Username = "postgres";
        private const string Password = "";

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
            optionsBuilder.UseNpgsql($"Host={Host};Port={Port};Database={database};Username={Username};Password={Password}");
        }
    }
}