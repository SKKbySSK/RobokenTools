using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobokenTools
{
    public class SpanData
    {
        public SpanData(DateTime date, double value)
        {
            Date = date;
            Value = value;
        }

        public DateTime Date { get; }

        public double Value { get; }
    }

    public class SpanCollection : IList<SpanData>
    {
        private List<SpanData> collection = new List<SpanData>();

        public SpanData this[int index] { get => ((IList<SpanData>)collection)[index]; set => ((IList<SpanData>)collection)[index] = value; }

        public int Count => ((IList<SpanData>)collection).Count;

        public bool IsReadOnly => ((IList<SpanData>)collection).IsReadOnly;

        public void Add(DateTime date, double value)
        {
            lock (collection)
            {
                collection.Add(new SpanData(date, value));
            }
        }

        public void Add(double value) => Add(DateTime.Now, value);

        public void Add(SpanData item)
        {
            lock (collection)
            {
                ((IList<SpanData>)collection).Add(item);
            }
        }

        public void Clear()
        {
            ((IList<SpanData>)collection).Clear();
        }

        public bool Contains(SpanData item)
        {
            return ((IList<SpanData>)collection).Contains(item);
        }

        public void CopyTo(SpanData[] array, int arrayIndex)
        {
            ((IList<SpanData>)collection).CopyTo(array, arrayIndex);
        }

        public IEnumerator<SpanData> GetEnumerator()
        {
            return ((IList<SpanData>)collection).GetEnumerator();
        }

        public List<SpanData> GetValues(DateTime from, DateTime to)
        {
            lock (collection)
            {
                return collection.Where(pair => pair.Date >= from && to >= pair.Date).ToList();
            }
        }

        public int IndexOf(SpanData item)
        {
            return ((IList<SpanData>)collection).IndexOf(item);
        }

        public void Insert(int index, SpanData item)
        {
            ((IList<SpanData>)collection).Insert(index, item);
        }

        public bool Remove(SpanData item)
        {
            return ((IList<SpanData>)collection).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<SpanData>)collection).RemoveAt(index);
        }

        public SpanCollection Clone()
        {
            lock (collection)
            {
                SpanCollection col = new SpanCollection();
                foreach (var s in this)
                {
                    col.Add(s.Date, s.Value);
                }
                return col;
            }
        }

        public List<OxyPlot.DataPoint> ToDataPoints()
        {
            lock (collection)
            {
                List<OxyPlot.DataPoint> ps = new List<OxyPlot.DataPoint>();
                foreach (var s in this)
                {
                    ps.Add(new OxyPlot.DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(s.Date), s.Value));
                }
                return ps.OrderByDescending(d => d.X).ToList();
            }
        }

        public List<OxyPlot.DataPoint> ToDataPoints(DateTime from, DateTime to)
        {
            var span = GetValues(from, to);
            lock (collection)
            {
                List<OxyPlot.DataPoint> ps = new List<OxyPlot.DataPoint>();
                foreach (var s in span)
                {
                    ps.Add(new OxyPlot.DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(s.Date), s.Value));
                }
                return ps.OrderByDescending(d => d.X).ToList();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<SpanData>)collection).GetEnumerator();
        }
    }
}
