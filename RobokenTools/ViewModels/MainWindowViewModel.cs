using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace RobokenTools.ViewModels
{
    class MainWindowViewModel : Abstracts.ViewModelBase
    {
        public event ValueEventHandler<Abstracts.DataPlotter> PlotterSelected;

        public event ValueEventHandler<Abstracts.DataPlotter> DataAvailable;

        public MainWindowViewModel()
        {
            LoadCommand = new RelayCommand(() =>
            {
                var w = new Windows.LoadPlotterWindow();
                w.PlotterSelected += LoadPlotterWindow_PlotterSelected;
                w.ShowDialog();
            });

            SaveCommand = new RelayCommand(() =>
            {
                var data = Plotter?.Data?.ToDataPoints();

                if (data != null)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "txtファイル|*.txt|すべてのファイル|*.*";
                    if (sfd.ShowDialog() ?? false)
                    {
                        using (var sw = new StreamWriter(sfd.FileName))
                        {
                            foreach (var d in data)
                                sw.WriteLine($"{d.X}\t{d.Y}");
                        }
                    }
                }
            });

            SaveCsvCommand = new RelayCommand(() =>
            {
                var data = Plotter?.Data?.ToDataPoints();

                if (data != null)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "csvファイル|*.csv|txtファイル|*.txt|すべてのファイル|*.*";
                    if (sfd.ShowDialog() ?? false)
                    {
                        using (var sw = new StreamWriter(sfd.FileName))
                        {
                            foreach (var d in data)
                                sw.WriteLine($"{d.X}, {d.Y}");
                        }
                    }
                }
            });
        }

        private void LoadPlotterWindow_PlotterSelected(object sender, ValueEventArgs<Abstracts.DataPlotter> e)
        {
            var w = (Windows.LoadPlotterWindow)sender;
            w.PlotterSelected -= LoadPlotterWindow_PlotterSelected;
            w.Close();

            Plotter = e.Value;
            PlotterSelected?.Invoke(this, e);

            if (IsRunning)
                e.Value.Open();
        }

        public RelayCommand LoadCommand { get; }

        public RelayCommand SaveCommand { get; }

        public RelayCommand SaveCsvCommand { get; }

        Abstracts.DataPlotter _plotter;
        public Abstracts.DataPlotter Plotter
        {
            get => _plotter;
            set
            {
                if (value == _plotter) return;
                if (_plotter != null)
                {
                    _plotter.Close();
                    _plotter.DataAvailable -= _plotter_DataAvailable;
                }

                _plotter = value;

                if (_plotter != null)
                {
                    _plotter.DataAvailable += _plotter_DataAvailable;
                }

                RaisePropertyChanged();
            }
        }

        bool _isRunning = true;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (value == _isRunning) return;
                _isRunning = value;

                if (value)
                    Plotter?.Open();
                else
                    Plotter?.Close();

                RaisePropertyChanged();
            }
        }

        double _span = 1000;
        public double Span
        {
            get => _span;
            set
            {
                if (value == _span) return;
                _span = value;
                RaisePropertyChanged();
                DeltaLabel = Math.Round(value, 0) + "ms";
            }
        }

        bool _isInfinityMode = false;
        public bool IsInfinityMode
        {
            get => _isInfinityMode;
            set
            {
                if (value == _isInfinityMode) return;
                _isInfinityMode = value;
                RaisePropertyChanged();
                DeltaLabel = Math.Round(Span, 0) + "ms";
            }
        }

        string _deltaLabel = "1000ms";
        public string DeltaLabel
        {
            get => _deltaLabel;
            set
            {
                if (IsInfinityMode)
                    _deltaLabel = "Infinity";
                else
                    _deltaLabel = value;

                RaisePropertyChanged();
            }
        }

        private void _plotter_DataAvailable(object sender, EventArgs e)
        {
            Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
            {
                DataAvailable?.Invoke(this, new ValueEventArgs<Abstracts.DataPlotter>((Abstracts.DataPlotter)sender));
            }));
        }
    }
}
