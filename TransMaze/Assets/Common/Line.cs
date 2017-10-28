using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Common
{
    public class Line
    {
        public string Name { get; set; }
        public List<Station> Stations { get; set; }

        public Line(string name)
        {
            Name = name;
            Stations = new List<Station>();
        }

        public void AddStation(Station station)
        {
            Stations.Add(station);            
        }

        public override string ToString()
        {
            return string.Format("Line {0}: {1}", Name, string.Join(", ", Stations.Select(st=>st.Name).ToArray()));
        }

        /// <summary>
        /// Get the name of the last station of the line. In one way if direction is true, the other way when direction is false. 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public string DirectionName(bool direction)
        {
            if (Stations.Count == 0) return "This line is empty";
            if (direction)
                return Stations.Last().Name;
            return Stations.First().Name;
        }

        /// <summary>
        /// Says what is the next station on the line, from the currentStation and the given direction
        /// This method can return null when the Station is not on the line, or already the terminus. 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="currentStation"></param>
        /// <returns></returns>
        public Station GetNextStation(bool direction, Station currentStation)
        {
            int currentIndex = Stations.IndexOf(currentStation);
            if (currentStation==-1)
            {

            }
        }
    }
}
