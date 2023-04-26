using Microsoft.EntityFrameworkCore;

namespace ExampleProject;

public class Program
{
#pragma warning disable CS1998 (async, without any awaits)
    public static async Task Main(string[] args)
#pragma warning restore CS1998
    {
        InitializeDb();

        using (var db = new SqliteDbContext())
        {
            // Add some data to start with, verify basic saving works

            db.MagicSkills.Add(new MagicSkill
            {
                Name = "Firebolt",
                RunicName = "ignis",
            });
            db.MagicSkills.Add(new MagicSkill
            {
                Name = "Lightning",
                RunicName = "fulgur",
            });
            db.MartialSkills.Add(new MartialSkill
            {
                Name = "Combo1",
                HasStrike = true,
            });

            db.SaveChanges();

            // -- YOUR REPRO HERE --

            db.SaveChanges();
        }
        Console.WriteLine("Program finished.");
    }

#region Setup
    private static void InitializeDb()
    {
        using var db = new SqliteDbContext();
        // Recreate database
        db.Database.EnsureDeleted();
        db.Database.Migrate();

        // Seed database

        db.SaveChanges();
    }
#endregion
}
