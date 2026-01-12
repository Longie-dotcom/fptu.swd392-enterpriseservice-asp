using Application.Helper;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed
{
    public static class Seeder
    {
        public static async Task SeedAsync(EnterpriseDBContext context)
        {
            // Guard
            if (await context.WasteTypes.AnyAsync())
            {
                ServiceLogger.Warning(
                    Level.Infrastructure,
                    "Database already seeded, seeding action has been terminated");
                return;
            }

            var wasteTypes = new List<WasteType>
            {
                new WasteType(
                    "ORGANIC",
                    "Food waste, biodegradable household waste"),

                new WasteType(
                    "PLASTIC",
                    "Plastic bottles, bags, containers"),

                new WasteType(
                    "PAPER",
                    "Paper, cardboard, newspapers"),

                new WasteType(
                    "METAL",
                    "Metal cans, scrap metal"),

                new WasteType(
                    "GLASS",
                    "Glass bottles and jars"),

                new WasteType(
                    "ELECTRONIC",
                    "Electronic waste such as phones, batteries"),

                new WasteType(
                    "HAZARDOUS",
                    "Hazardous waste requiring special handling"),

                new WasteType(
                    "GENERAL",
                    "Non-recyclable general waste")
            };

            await context.WasteTypes.AddRangeAsync(wasteTypes);
            await context.SaveChangesAsync();

            ServiceLogger.Logging(
                Level.Infrastructure,
                "Waste types seeded successfully");
        }
    }
}
