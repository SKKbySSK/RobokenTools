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
                case SerialTool.SerialPlotter serial:
                    var line = new OxyPlot.Wpf.LineSeries();
                    current = line;
                    line.ItemsSource = e.Value.Data.ToDataPoints();
                    plotV.Series.Add(line);
                    plotV.Axes.Add(new OxyPlot.Wpf.DateTimeAxis()
                    {
                        LabelFormatter = d => (DateTime.Now - OxyPlot.Axes.DateTimeAxis.ToDateTime(d)).TotalMilliseconds + "ms",
                        IsZoomEnabled = true,
                        IsPanEnabled = true,
                        MajorGridlineColor = Colors.Gray,
                        MajorGridlineThickness = 1,
                        MinorGridlineColor = Colors.LightGray,
                        MinorGridlineThickness = 0.2,
                        Title = "時間",
                    });
                    break;
            }

            e.Value.Open();
        }

        private void ViewModel_DataAvailable(object sender, ValueEventArgs<Abstracts.DataPlotter> e)
        {
            switch (e.Value)
            {
                case SerialTool.SerialPlotter serial:
                    PlotSerial(serial);
                    break;
            }
        }

        private void PlotSerial(SerialTool.SerialPlotter plotter)
        {
            current.ItemsSource = null;
            var now = DateTime.Now;
            var from = now.Subtract(TimeSpan.FromMilliseconds(viewModel.Span));
            current.ItemsSource = plotter.Data.ToDataPoints(from, now);
            plotV.InvalidatePlot();
        }

        private void ResetView()
        {
            plotV.Series.Clear();
            plotV.Axes.Clear();
        }
    }
}
