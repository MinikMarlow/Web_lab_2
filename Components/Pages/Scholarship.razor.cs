using BlazorApp2.Data;
using BlazorApp2.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Components.Pages
{
    public partial class Scholarship : ComponentBase
    {
        private bool Loading;
        private List<Student> students = new();
        private List<Student> scholarshipStudents = new();
        private decimal TotalScholarship;

        [Inject]
        private IDbContextFactory<UniversityContext> DbFactory { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            try
            {
                await using var context = await DbFactory.CreateDbContextAsync();
                if (context != null && context.Students != null)
                {
                    var studentList = await context.Students.ToListAsync();
                    if (studentList != null)
                    {
                        students = studentList;
                        UpdateScholarshipData();
                    }
                }
            }
            finally
            {
                Loading = false;
            }
        }

        private void UpdateScholarshipData()
        {
            if (students == null) return;

            scholarshipStudents = students
                .Where(s => s != null && s.IsOnScholarship)
                .ToList();

            TotalScholarship = scholarshipStudents?
                .Sum(s => s?.Scholarship ?? 0) ?? 0;
        }

        private async Task UpdateStudentScore(Student student)
        {
            if (student == null) return;

            Loading = true;
            try
            {
                await using var context = await DbFactory.CreateDbContextAsync();
                if (context != null && context.Students != null)
                {
                    var dbStudent = await context.Students.FindAsync(student.Id);
                    if (dbStudent != null)
                    {
                        dbStudent.AverageScore = Math.Clamp(student.AverageScore, 0, 5);

                        // Автоматическое удаление при балле <4
                        if (dbStudent.AverageScore < 4.0)
                        {
                            context.Students.Remove(dbStudent);
                        }

                        await context.SaveChangesAsync();

                        var updatedStudents = await context.Students.ToListAsync();
                        if (updatedStudents != null)
                        {
                            students = updatedStudents;
                            UpdateScholarshipData();
                        }
                    }
                }
            }
            finally
            {
                Loading = false;
            }
        }
    }
}