using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Common
{
    public class Station
    {
        public string Name { get; set; }

        public List<Line> Lines { get; set; }

        public List<Station> ConnectedStations { get; set; }

        public Station(string name)
        {
            Name = name;
            Lines = new List<Line>();
            ConnectedStations = new List<Station>();
        }

        /// <summary>
        /// Adds a new line reference to this station
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(Line line)
        {
            if (!Lines.Contains(line))
                Lines.Add(line);
        }

        public void AddConnection(Station station)
        {
            if (station == null) return;
            if (!ConnectedStations.Contains(station)) 
                ConnectedStations.Add(station);
        }

        /// <summary>
        /// Gives the number of lines sharing this station
        /// </summary>
        public int LineCount { get { return Lines.Count; } }


        /// <summary>
        /// Gives the number of stations that are directly connected to this line (no matter the line)
        /// </summary>
        public int ConnectionCount { get { return ConnectedStations.Count; } }

        public override string ToString()
        {
            return string.Format("Station {0}", Name);
        }
    }
}
