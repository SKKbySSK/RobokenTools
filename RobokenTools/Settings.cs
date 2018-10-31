using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RobokenTools
{
    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (!loading)
            {
                Exporter.SerializeToFile(this, ConfigPath);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public const string ConfigPath = "config.json";

        private static bool loading = false;

        static Settings()
        {
            loading = true;
            if (File.Exists(ConfigPath))
            {
                try
                {
                    Current = Exporter.DeserializeFromFile<Settings>(ConfigPath);
                }
                catch (Exception)
                {
                    Current = new Settings();
                    File.Delete(ConfigPath);
                }
            }
            else
            {
                Current = new Settings();
            }
            loading = false;
        }

        public static Settings Current { get; }

        int _baudRate;
        public int BaudRate
        {
            get => _baudRate;
            set
            {
                if (value == _baudRate) return;
                _baudRate = value;
                RaisePropertyChanged();
            }
        }
    }
}
