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
using System.Windows.Shapes;

namespace RobokenTools.Windows
{
    /// <summary>
    /// LoadPlotterWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LoadPlotterWindow : Window
    {
        public event ValueEventHandler<Abstracts.DataPlotter> PlotterSelected;

        ViewModels.LoadPlotterViewModel viewModel = new ViewModels.LoadPlotterViewModel();

        public LoadPlotterWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.PlotterSelected += ViewModel_PlotterSelected;
        }

        private void ViewModel_PlotterSelected(object sender, ValueEventArgs<Abstracts.DataPlotter> e)
        {
            PlotterSelected?.Invoke(this, e);
        }
    }
}
