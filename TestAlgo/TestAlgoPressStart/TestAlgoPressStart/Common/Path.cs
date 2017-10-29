using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Common
{
    public class Path
    {
        internal Path(List<Connection> steps, float cost)
        {
            Steps = steps;
            Cost = cost;
        }
        internal Path(List<Connection> oldPath, float cost, Connection newConnection)
        {
            Steps = new List<Connection>(oldPath);
            Steps.Insert(0, newConnection);
            Cost = cost; 
        }

        public List<Connection> Steps { get; set; }
        public float Cost { get; set; }

        public override string ToString()
        {
            return string.Format("From {0} to {1}, duration: {2:N2}min, Steps: {3}",
                Steps.First().Station.Name,
                Steps.Last().Station.Name,
                Cost,
                string.Join(", ", Steps.Select(st => string.Format("{0} ({1})", st.Station.Name, st.Line == null ? "-" : st.Line.Name)).ToArray()));
        }
    }
}
