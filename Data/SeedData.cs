using BlazorApp2.Data;
using BlazorApp2.Models;

namespace BlazorApp2.Data
{
    public class SeedData
    {
        public async Task InitializeAsync(UniversityContext context)
        {
            if (context is not null && context.Students is not null)
            {
                var students = new Student[]
                {
            new Student { Id = 1, FullName = "Иванов Иван", AverageScore = 4.8 },
            new Student { Id = 2, FullName = "Петрова Анна", AverageScore = 5.0 },
            new Student { Id = 3, FullName = "Сидоров Алексей", AverageScore = 3.9 }
                };

                if (students is not null)
                {
                    context.Students.AddRange(students);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
