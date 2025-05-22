using BlazorApp2.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Data
{
    public class DatabaseUtility
    {
        public static async Task EnsureDbCreatedAndSeedAsync(DbContextOptions<UniversityContext> options)
        {
            var factory = new LoggerFactory();
            var builder = new DbContextOptionsBuilder<UniversityContext>(options)
                .UseLoggerFactory(factory);

            using var context = new UniversityContext(builder.Options);

            if (await context.Database.EnsureCreatedAsync())
            {
                var seed = new SeedData();
                if (seed is not null && context.Students is not null)
                { 
                    await seed.InitializeAsync(context);
                }
            }

        }
    }
}