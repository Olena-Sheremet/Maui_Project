using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MauiProject.Services;
using SkiaSharp;

namespace MauiProject;

public partial class MainPage : ContentPage
{
    private readonly StudentDataService _studentService;

    public MainPage(StudentDataService studentService)
    {
        InitializeComponent();
        _studentService = studentService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var student = await _studentService.LoadStudentDataAsync();

        if (student == null)
        {
            await DisplayAlert("Помилка", "Не вдалося завантажити дані студента.", "OK");
            return;
        }

        Title = $"Успішність: {student.FirstName} {student.LastName}";

        var courseNames = student.Courses.Select(c => c.CourseName).ToList();
        var averageGrades = student.Courses
            .Select(c => c.Grades.Average(g => g.GradeValue))
            .ToArray();

        GradesChart.Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = averageGrades,
                Fill = new SolidColorPaint(SKColors.SkyBlue),
                Stroke = new SolidColorPaint(SKColors.DarkBlue) { StrokeThickness = 2 }
            }
        };

        GradesChart.XAxes = new Axis[]
        {
            new Axis
            {
                Labels = courseNames.ToArray(),
                LabelsRotation = 15,
                TextSize = 16,
                LabelsPaint = new SolidColorPaint(SKColors.Black)
            }
        };

        GradesChart.YAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                MaxLimit = 100,
                TextSize = 16,
                LabelsPaint = new SolidColorPaint(SKColors.Black)
            }
        };
    }
}