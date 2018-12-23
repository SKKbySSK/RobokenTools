using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RobokenTools.Views
{
    class SyncConnectionView : ConnectionView
    {
        private System.Windows.Controls.TextBox startT = new System.Windows.Controls.TextBox() { Text = "73" },
            endT = new System.Windows.Controls.TextBox() { Text = "65" };

        public SyncConnectionView()
        {
            AddProperty("開始コード(16進数)", startT, true);
            AddProperty("開始コード(16進数)", endT, true);
        }

        protected override void ExecuteCommand()
        {
            if (int.TryParse(startT.Text, out _) && int.TryParse(endT.Text, out _))
            {
                var startB = Convert.ToByte(startT.Text, 16);
                var endB = Convert.ToByte(endT.Text, 16);

                if (Command?.CanExecute(Connection) ?? false)
                {
                    Command.Execute(new SerialTool.SEConnection(Connection, startB, endB));
                }
            }
            else
            {
                MessageBox.Show("開始コードまたは終了コードが数値でないか値が有効な範囲ではありません");
            }
        }
    }
}
