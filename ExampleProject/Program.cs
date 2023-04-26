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
            db.DivineSkills.Add(new DivineSkill
            {
                Name = "Smite",
                RequiredDivinity = 3,
            });

            db.SaveChanges();

            // -- YOUR REPRO HERE --

            var martialOrMagicSkills = db.Skills
                .Where(s => s is MartialSkill || s is MagicSkill)
                .ToList();

            // The above query yields the following db command:
            // SELECT "t"."Id", "t"."Name", "t"."RequiredDivinity", "t"."RunicName", "t"."HasStrike", "t"."Discriminator"
            // FROM (
            //     SELECT "d"."Id", "d"."Name", "d"."RequiredDivinity", NULL AS "RunicName", NULL AS "HasStrike", 'DivineSkill' AS "Discriminator"
            //     FROM "DivineSkills" AS "d"
            //     UNION ALL
            //     SELECT "m"."Id", "m"."Name", NULL AS "RequiredDivinity", "m"."RunicName", NULL AS "HasStrike", 'MagicSkill' AS "Discriminator"
            //     FROM "MagicSkills" AS "m"
            //     UNION ALL
            //     SELECT "m0"."Id", "m0"."Name", NULL AS "RequiredDivinity", NULL AS "RunicName", "m0"."HasStrike", 'MartialSkill' AS "Discriminator"
            //     FROM "MartialSkills" AS "m0"
            // ) AS "t"
            // WHERE "t"."Discriminator" IN ('MartialSkill', 'MagicSkill')

            // This query targets all 3 tables, instead of just the two that match the types being filtered on.
            // This is inefficient and does not match the behavior observed when filtering on a single type. e.g.

            var magicSkills = db.Skills
                .Where(s => s is MagicSkill)
                .ToList();

            // yields the following (which appears more correct):
            // SELECT "m"."Id", "m"."Name", NULL AS "RequiredDivinity", "m"."RunicName", NULL AS "HasStrike", 'MagicSkill' AS "Discriminator"
            // FROM "MagicSkills" AS "m"
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
