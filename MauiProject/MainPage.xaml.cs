using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace MauiProject;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var values = new double[] { 93, 95, 93, 91, 87 };

        GradesChart.Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = values,
                GeometrySize = 15,
                Fill = null,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 3 }
            }
        };

        GradesChart.XAxes = new Axis[]
        {
            new Axis
            {
                Labels = new[] { "A", "B", "C", "D", "E" },
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