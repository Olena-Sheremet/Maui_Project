using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MauiProject.Models;
using MauiProject.Services;
using SkiaSharp;

namespace MauiProject;
public partial class MainPage : ContentPage
{
    private readonly StudentDataService _studentService;
    private Student _student;
    private List<Course> _courses;

    public MainPage()
    {
        InitializeComponent();
        _studentService = new StudentDataService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _student = await _studentService.LoadStudentDataAsync();
        if (_student == null)
        {
            await DisplayAlert("Помилка", "Не вдалося завантажити дані студента", "OK");
            return;
        }

        _courses = _student.Courses;

        var pickerItems = new List<string> { "Загальний графік" };
        pickerItems.AddRange(_courses.Select(c => c.CourseName));
        CoursePicker.ItemsSource = pickerItems;
        CoursePicker.SelectedIndex = 0;

        ShowAverageGradesChart();
    }

    private void ShowAverageGradesChart()
    {
        // Масив середніх значень
        var averageGrades = _courses.Select(c =>
        {
            var gradesValues = c.Grades.Select(g => g.GradeValue);
            return gradesValues.Any() ? gradesValues.Average() : 0;
        }).ToArray();

        GradesChart.Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = averageGrades,
                Fill = new SolidColorPaint(SKColors.MediumSlateBlue),
                Name = "Середній бал"
            }
        };

        GradesChart.XAxes = new Axis[]
        {
            new Axis
            {
                Labels = _courses.Select(c => c.CourseName).ToArray(),
                TextSize = 0 // Приховуємо підписи під графіком
            }
        };

        GradesChart.YAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                MaxLimit = 100
            }
        };

        GradesChart.TooltipPosition = TooltipPosition.Auto;
    }

    private void ShowGradesForSelectedCourse(int selectedIndex)
    {
        if (selectedIndex < 1 || selectedIndex > _courses.Count)
            return;

        var selectedCourse = _courses[selectedIndex - 1];

        var subjectNames = selectedCourse.Grades.Select(g => g.CourseName).ToArray();
        var gradeValues = selectedCourse.Grades.Select(g => (double)g.GradeValue).ToArray();

        GradesChart.Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = gradeValues,
                Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                Name = "Оцінка"
            }
        };

        GradesChart.XAxes = new Axis[]
        {
            new Axis
            {
                Labels = subjectNames,
                TextSize = 0 // Ховаємо підписи під графіком
            }
        };

        GradesChart.YAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                MaxLimit = 100
            }
        };

        GradesChart.TooltipPosition = TooltipPosition.Auto;
    }

    private void CoursePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CoursePicker.SelectedIndex == 0)
        {
            ShowAverageGradesChart();
        }
        else
        {
            ShowGradesForSelectedCourse(CoursePicker.SelectedIndex);
        }
    }
}