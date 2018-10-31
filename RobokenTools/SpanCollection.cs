using System;
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

    public class SpanCollection
    {
        private List<SpanData> collection = new List<SpanData>();

        public void Add(DateTime date, double value)
        {
            collection.Add(new SpanData(date, value));
        }

        public void Add(double value) => Add(DateTime.Now, value);

        public List<SpanData> GetValues(DateTime from, DateTime to)
        {
            return collection.Where(pair => pair.Date >= from && to >= pair.Date).ToList();
        }
    }
}
