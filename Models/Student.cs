namespace BlazorApp2.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public double AverageScore { get; set; }

        public bool IsOnScholarship => AverageScore >= 4.0;

        public decimal Scholarship
        {
            get
            {
                if (AverageScore >= 4.0 && AverageScore < 4.5) return 2000.00M;
                if (AverageScore >= 4.5 && AverageScore < 5.0) return 2500.00M;
                if (AverageScore == 5.0) return 3000.00M;
                return 0.00M;
            }
        }
    }
}