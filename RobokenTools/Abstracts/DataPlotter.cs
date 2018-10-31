using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace RobokenTools.Abstracts
{
    public abstract class DataPlotter
    {
        public event EventHandler Refresh;

        protected void RefreshView()
        {
            Refresh?.Invoke(this, new EventArgs());
        }

        public double Maximum { get; }

        public double Minimum { get; }

        public SpanCollection Data { get; } = new SpanCollection();

        public abstract void Open();

        public abstract void Close();
    }
}
