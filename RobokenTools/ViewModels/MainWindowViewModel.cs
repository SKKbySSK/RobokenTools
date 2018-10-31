using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobokenTools.ViewModels
{
    class MainWindowViewModel : Abstracts.ViewModelBase
    {
        public MainWindowViewModel()
        {
            LoadCommand = new RelayCommand(() =>
            {
                var w = new Windows.LoadPlotterWindow();
                w.PlotterSelected += LoadPlotterWindow_PlotterSelected;
                w.ShowDialog();
            });
        }

        private void LoadPlotterWindow_PlotterSelected(object sender, ValueEventArgs<Abstracts.DataPlotter> e)
        {
            var w = (Windows.LoadPlotterWindow)sender;
            w.PlotterSelected -= LoadPlotterWindow_PlotterSelected;
            w.Close();
        }

        public RelayCommand LoadCommand { get; }
    }
}
