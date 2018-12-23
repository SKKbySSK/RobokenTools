using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RobokenTools
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModels.MainWindowViewModel viewModel = new ViewModels.MainWindowViewModel();
        OxyPlot.Wpf.Series current;
        OxyPlot.Wpf.Axis yAxis;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.DataAvailable += ViewModel_DataAvailable;
            viewModel.PlotterSelected += ViewModel_PlotterSelected;
        }

        private void ViewModel_PlotterSelected(object sender, ValueEventArgs<Abstracts.DataPlotter> e)
        {
            ResetView();
            switch (e.Value)
            {
                default:
                    var line = new OxyPlot.Wpf.LineSeries();
                    current = line;
                    line.ItemsSource = e.Value.Data.ToDataPoints();
                    plotV.Series.Add(line);
                    plotV.Axes.Add(new OxyPlot.Wpf.DateTimeAxis()
                    {
                        LabelFormatter = d => (DateTime.Now - OxyPlot.Axes.DateTimeAxis.ToDateTime(d)).TotalMilliseconds + "ms",
                        IsZoomEnabled = false,
                        IsPanEnabled = false,
                        MajorGridlineColor = Colors.Gray,
                        MajorGridlineThickness = 1,
                        MinorGridlineColor = Colors.LightGray,
                        MinorGridlineThickness = 0.2,
                        Title = "時間",
                    });

                    yAxis = new OxyPlot.Wpf.LinearAxis()
                    {
                        Maximum = (int)maxS.Value,
                        Minimum = (int)minS.Value,
                    };
                    plotV.Axes.Add(yAxis);
                    break;
            }
        }

        private void ViewModel_DataAvailable(object sender, ValueEventArgs<Abstracts.DataPlotter> e)
        {
            switch (e.Value)
            {
                default:
                    Plot(e.Value);
                    break;
            }
        }

        private void Plot(Abstracts.DataPlotter plotter)
        {
            current.ItemsSource = null;

            if (viewModel.Span == spanS.Maximum)
            {
                current.ItemsSource = plotter.Data.ToDataPoints();
            }
            else
            {
                var now = DateTime.Now;
                var from = now.Subtract(TimeSpan.FromMilliseconds(viewModel.Span));
                current.ItemsSource = plotter.Data.ToDataPoints(from, now);
            }

            plotV.InvalidatePlot();
        }

        private void ResetView()
        {
            plotV.Series.Clear();
            plotV.Axes.Clear();
        }

        private void spanS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            viewModel.IsInfinityMode = e.NewValue == spanS.Maximum;
        }

        private void minS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (yAxis != null)
                yAxis.Minimum = e.NewValue;
        }

        private void maxS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (yAxis != null)
                yAxis.Maximum = e.NewValue;
        }
    }
}
