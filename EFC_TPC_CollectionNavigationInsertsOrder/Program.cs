using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFSampleApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        using (var db = new MyContext())
        {
            // Recreate database
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Seed database


            db.SaveChanges();
        }

        using (var db = new MyContext())
        {
            // Run queries
            // var query = db.Blogs.ToList();
        }
        Console.WriteLine("Program finished.");
    }
}


public class MyContext : DbContext
{
    private static ILoggerFactory ContextLoggerFactory
        => LoggerFactory.Create(b =>
        {
            b
                .AddConsole()
                .AddFilter("", LogLevel.Debug);
        });

    // Declare DBSets
    public DbSet<MartialSkill> MartialSkills { get; set; }
    public DbSet<MartialSkill> MagicSkills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Select 1 provider
        optionsBuilder
            // .UseSqlite(@"Server=(localdb)\mssqllocaldb;Database=_ModelApp;Trusted_Connection=True;Connect Timeout=5;ConnectRetryCount=0")
            .UseSqlite("filename=_modelApp.db")
            //.UseInMemoryDatabase(databaseName: "_modelApp")
            //.UseCosmos("https://localhost:8081", @"C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", "_ModelApp")
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(ContextLoggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure model
    }
}
