using Microsoft.EntityFrameworkCore;
using DBModels;
namespace DBContext
{
    public class DataBaseContext: DbContext
    {
        private const string Host = "localhost";
        private const string Port = "5432";
        private const string database = "Laba6db";
        private const string Username = "postgres";
        private const string Password = "Fndya21003";

        public DbSet<Graduate> Graduates { get; set; }

        public DataBaseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={Host};Port={Port};Database={database};Username={Username};Password={Password}");
            //добавить константу
        }
    }
}