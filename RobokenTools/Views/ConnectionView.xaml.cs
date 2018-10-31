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

namespace RobokenTools.Views
{
    /// <summary>
    /// ConnectionView.xaml の相互作用ロジック
    /// </summary>
    public partial class ConnectionView : UserControl
    {
        public ConnectionView()
        {
            InitializeComponent();
            grid.DataContext = this;
            connectionC.ItemsSource = SerialTool.ConnectionManager.GetPorts();
        }

        public static readonly DependencyProperty ConnectionProperty = DependencyProperty.Register(nameof(Connection),
            typeof(SerialTool.Connection), typeof(ConnectionView),
            new PropertyMetadata(default(SerialTool.Connection), (obj, e) => ((ConnectionView)obj).ConnectionPropertyChanged((SerialTool.Connection)e.OldValue, (SerialTool.Connection)e.NewValue)));

        private void ConnectionPropertyChanged(SerialTool.Connection oldValue, SerialTool.Connection newValue)
        {
            connectionC.SelectionChanged -= connectionC_SelectionChanged;
            connectionC.SelectedItem = newValue;
            connectionC.SelectionChanged += connectionC_SelectionChanged;
        }

        public SerialTool.Connection Connection
        {
            get => (SerialTool.Connection)GetValue(ConnectionProperty);
            set => SetValue(ConnectionProperty, value);
        }

        private void connectionC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (connectionC.SelectedItem is SerialTool.Connection con)
                Connection = con;
        }

        private void refreshB_Click(object sender, RoutedEventArgs e)
        {
            connectionC.ItemsSource = SerialTool.ConnectionManager.GetPorts();
            connectionC.SelectedItem = null;
        }
    }
}
