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
            connectionC.SelectionChanged -= connectionC_SelectionChanged;

            grid.DataContext = this;
            connectionC.ItemsSource = SerialTool.ConnectionManager.GetPorts();
            
            var rates = new int[]
            {
                110,
                300,
                600,
                1200,
                2400,
                4800,
                9600,
                14400,
                19200,
                38400,
                57600,
                115200,
                128000,
                256000
            };
            
            baudrateC.Text = Settings.Current.BaudRate.ToString();
            foreach (var r in rates)
                baudrateC.Items.Add(r);

            databitsC.SelectedItem = 8;
            databitsC.Items.Add(5);
            databitsC.Items.Add(6);
            databitsC.Items.Add(7);
            databitsC.Items.Add(8);

            var parities = new System.IO.Ports.Parity[]
            {
                 System.IO.Ports.Parity.None,
                 System.IO.Ports.Parity.Odd,
                 System.IO.Ports.Parity.Even,
                 System.IO.Ports.Parity.Mark,
                 System.IO.Ports.Parity.Space,
            };

            parityC.SelectedItem = System.IO.Ports.Parity.None;
            foreach (var p in parities)
                parityC.Items.Add(p);

            connectionC.SelectionChanged += connectionC_SelectionChanged;
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

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command),
            typeof(ICommand), typeof(ConnectionView),
            new PropertyMetadata(default(ICommand), (obj, e) => ((ConnectionView)obj).CommandPropertyChanged((ICommand)e.OldValue, (ICommand)e.NewValue)));

        private void CommandPropertyChanged(ICommand oldValue, ICommand newValue)
        {

        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
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

        private void loadB_Click(object sender, RoutedEventArgs e)
        {
            if (Connection == null)
            {
                MessageBox.Show("入力デバイスを選択してください");
                return;
            }

            if (int.TryParse(baudrateC.Text, out var baudrate) && baudrate > 0)
            {
                var databits = (int)databitsC.SelectedItem;
                var parity = (System.IO.Ports.Parity)parityC.SelectedItem;
                Settings.Current.BaudRate = baudrate;

                if (Command?.CanExecute(Connection) ?? false)
                {
                    Connection.BaudRate = baudrate;
                    Connection.DataBits = databits;
                    Connection.Parity = parity;
                    Command.Execute(Connection);
                }
            }
            else
            {
                MessageBox.Show("ボーレートが有効な値ではありません");
            }
        }
    }
}
