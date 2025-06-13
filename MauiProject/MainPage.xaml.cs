using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MauiProject.Services;

namespace MauiProject;

public partial class MainPage : ContentPage
{
    private readonly StudentDataService _studentService;

    public MainPage(StudentDataService studentService)
    {
        try
        {
            InitializeComponent();
            _studentService = studentService;
            LoadStudentGrades();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            throw;
        }
    }

    private async void LoadStudentGrades()
    {
        var student = await _studentService.LoadStudentDataAsync();

        if (student != null)
        {
            var labels = new List<string>();
            var averageGrades = new List<double>();

            foreach (var course in student.Courses)
            {
                if (course.Grades != null && course.Grades.Any())
                {
                    // Перетворюємо GradeValue на double
                    double average = course.Grades.Average(g =>
                    {
                        return Convert.ToDouble(g.GradeValue); 
                    });
                    labels.Add(course.CourseName);
                    averageGrades.Add(average);
                }
            }

            // Оновлення UI на головному потоці
            MainThread.BeginInvokeOnMainThread(() =>
            {
                StudentNameLabel.Text = $"{student.FirstName} {student.LastName}";

                GradesChart.Series = new ISeries[]
                {
                    new ColumnSeries<double>
                    {
                        Values = averageGrades
                    }
                };

                GradesChart.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = labels,
                        LabelsRotation = 15
                    }
                };
            });
        }
        else
        {
            await DisplayAlert("Помилка", "Не вдалося завантажити дані студента.", "OK");
        }
    }
}
