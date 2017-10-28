using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Common
{
    public class Connection
    {
        public Connection(Station station, Line line, bool direction)
        {
            Station = station;
            Line = line;
            Direction = direction;
        }

        public Station Station { get; set; }
        public Line Line { get; set; }
        public bool Direction { get; set; }
    }
}
