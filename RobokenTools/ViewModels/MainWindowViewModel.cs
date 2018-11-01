﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        }

        private void LoadPlotterWindow_PlotterSelected(object sender, ValueEventArgs<Abstracts.DataPlotter> e)
        {
            var w = (Windows.LoadPlotterWindow)sender;
            w.PlotterSelected -= LoadPlotterWindow_PlotterSelected;
            w.Close();

            Plotter = e.Value;
            PlotterSelected?.Invoke(this, e);
        }

        public RelayCommand LoadCommand { get; }

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

        string _deltaLabel = "1000ms";
        public string DeltaLabel
        {
            get => _deltaLabel;
            set
            {
                if (value == _deltaLabel) return;
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
