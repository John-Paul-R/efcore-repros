using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExampleProject;

public class SqliteDbContext : DbContext
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
    public DbSet<MagicSkill> MagicSkills { get; set; }
    public DbSet<DivineSkill> DivineSkills { get; set; }
    public DbSet<Player> Players { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Select 1 provider
        optionsBuilder
            // .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=_ModelApp;Trusted_Connection=True;Connect Timeout=5;ConnectRetryCount=0")
            .UseSqlite("filename=_modelApp.db")
            //.UseInMemoryDatabase(databaseName: "_modelApp")
            //.UseCosmos("https://localhost:8081", @"C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", "_ModelApp")
            // .UseNpgsql(connectionString)
            .EnableSensitiveDataLogging()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
            .UseLoggerFactory(ContextLoggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbstractSkill>(e =>
        {
            e.UseTpcMappingStrategy()
                .HasKey(s => s.Id);
            e.Property(s => s.Id);
        });

        modelBuilder.Entity<MartialSkill>();
        modelBuilder.Entity<MagicSkill>();
        modelBuilder.Entity<DivineSkill>();

        modelBuilder.Entity<Player>()
            .HasMany(p => p.Skills)
            .WithOne(pts => pts.Player);

        modelBuilder.Entity<AbstractSkill>()
            .HasMany<PlayerToSkill>(a => a.PlayersWithSkill)
            .WithOne(pts => pts.Skill);


    }
}
