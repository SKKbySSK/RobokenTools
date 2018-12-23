using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobokenTools.ViewModels
{
    class LoadPlotterViewModel : Abstracts.ViewModelBase
    {
        public event ValueEventHandler<Abstracts.DataPlotter> PlotterSelected;

        public LoadPlotterViewModel()
        {
            LoadSerialConnectionCommand = new RelayCommand(o =>
            {
                if (o is SerialTool.Connection con)
                {
                    OnSelected(new SerialTool.SerialPlotter(con));
                }
            });

            LoadSyncSerialConnectionCommand = new RelayCommand(o =>
            {
                if (o is SerialTool.SEConnection con)
                {
                    OnSelected(new SerialTool.SESerialPlotter(con));
                }
            });
        }

        public RelayCommand LoadSerialConnectionCommand { get; }

        public RelayCommand LoadSyncSerialConnectionCommand { get; }

        int _maxDataCount = 500;
        public int MaxDataCount
        {
            get => _maxDataCount;
            set
            {
                if (value == _maxDataCount) return;
                _maxDataCount = value;
                RaisePropertyChanged();
            }
        }

        private void OnSelected(Abstracts.DataPlotter plotter)
        {
            plotter.Data.MaximumCount = MaxDataCount;
            PlotterSelected?.Invoke(this, new ValueEventArgs<Abstracts.DataPlotter>(plotter));
        }
    }
}
