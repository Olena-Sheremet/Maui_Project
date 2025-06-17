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
    private List<Student> _students;
    private Student _selectedStudent;
    private List<Course> _courses;

    public MainPage()
    {
        InitializeComponent();
        _studentService = new StudentDataService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _students = await _studentService.LoadStudentsDataAsync();
        if (_students == null || !_students.Any())
        {
            await DisplayAlert("Помилка", "Не вдалося завантажити дані студентів", "OK");
            return;
        }

        // Заповнюємо Picker студентів
        StudentPicker.ItemsSource = _students.Select(s => s.FullName).ToList();
        StudentPicker.SelectedIndex = -1;

        // Очистити курси та графіки
        CoursePicker.ItemsSource = null;
        GradesChart.Series = Array.Empty<ISeries>();
        GradesChart.XAxes = null;
        GradesChart.YAxes = null;
    }

    private void StudentPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (StudentPicker.SelectedIndex == -1)
        {
            _selectedStudent = null;
            CoursePicker.ItemsSource = null;
            GradesChart.Series = Array.Empty<ISeries>();
            GradesChart.XAxes = null;
            GradesChart.YAxes = null;
            return;
        }

        _selectedStudent = _students[StudentPicker.SelectedIndex];
        _courses = _selectedStudent.Courses;

        // Заповнюємо Picker курсів
        var pickerItems = new List<string> { "Загальний графік" };
        pickerItems.AddRange(_courses.Select(c => c.CourseName));
        CoursePicker.ItemsSource = pickerItems;
        CoursePicker.SelectedIndex = 0;

        ShowAverageGradesChart();
    }

    private void CoursePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_selectedStudent == null)
        {
            // Студент не вибраний - не показуємо графік
            GradesChart.Series = Array.Empty<ISeries>();
            GradesChart.XAxes = null;
            GradesChart.YAxes = null;
            return;
        }

        if (CoursePicker.SelectedIndex == 0)
        {
            ShowAverageGradesChart();
        }
        else
        {
            ShowGradesForSelectedCourse(CoursePicker.SelectedIndex);
        }
    }

    private void ShowAverageGradesChart()
    {
        if (_courses == null || !_courses.Any())
        {
            GradesChart.Series = Array.Empty<ISeries>();
            GradesChart.XAxes = null;
            GradesChart.YAxes = null;
            return;
        }

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
                TextSize = 0
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
    }

    private void ShowGradesForSelectedCourse(int selectedIndex)
    {
        if (_courses == null || selectedIndex < 1 || selectedIndex > _courses.Count)
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
                TextSize = 0
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
    }
}
