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
                    PlotterSelected?.Invoke(this, new ValueEventArgs<Abstracts.DataPlotter>(new SerialTool.SerialPlotter(con)));
                }
            });
        }

        public RelayCommand LoadSerialConnectionCommand { get; set; }
    }
}
