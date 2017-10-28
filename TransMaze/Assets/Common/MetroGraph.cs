using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Common
{
    public class MetroGraph
    {
        public MetroGraph()
        {
            Lines = new List<Line>();
            Stations = new Dictionary<string, Station>();
        }

        // All Stations known in the graph. 
        public Dictionary<string, Station> Stations { get; set; }
        
        // All the lines known in the graph
        public List<Line> Lines { get; set; }

        // Creates and fill a new line, adding it into the graph data
        public void AddLine(string name, params string[] stationNames)
        {
            Line line = new Line(name);
            Lines.Add(line);
            Station previousStation = null; // used to compute connections
            foreach (string stationName in stationNames)
            {
                Station station = null;
                if (Stations.ContainsKey(stationName))
                    station = Stations[stationName];
                else
                    station = new Station(name);
                line.AddStation(station);

                // Manage connections in both directions
                station.AddConnection(previousStation);
                if (previousStation!=null) previousStation.AddConnection(station);
                previousStation = station;
            }
        }

        /// <summary>
        /// Automatically build a new Graph based on its description from a text file. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public MetroGraph ImportFile(string fileName = "ListeStations.txt")
        {
            TextAsset textAsset = Resources.Load<TextAsset>(fileName);
            if (textAsset == null) return null;
            MetroGraph graph = new MetroGraph();
            var fullText = textAsset.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string lineName = null;
            List<string> stations = new List<string>();
            foreach (string textLine in fullText)
            {
                string res = textLine.Trim();
                if (res[0] == '[')
                {
                    if (lineName != null)
                    {
                        graph.AddLine(lineName, stations.ToArray());
                        stations.Clear();
                    }
                    lineName = textLine.Replace("[", "").Replace("]", "");
                }
                else
                {
                    Debug.AssertFormat(lineName != null, "The station {0} comes before any line header ine the file {1}.", res, fileName);
                    stations.Add(res);
                }
            }
            if (lineName != null)
                graph.AddLine(lineName, stations.ToArray());

            // Makes sure that the network is not empty
            Debug.AssertFormat(graph.Lines.Count > 0, "The network loaded from {0} is empty (no line).", fileName);

            // Displays some stats about the graph
            Debug.LogFormat("Number of lines imported: {0} ({1} are shared)", graph.Lines.Count);
            Debug.LogFormat("Number of stations: {0} ({1} are shared, {2:N2} avg connections per station)", graph.Stations.Count, graph.Stations.Count - graph.Lines.Sum(lin=>lin.Stations.Count), graph.Stations.Average(st=>st.Value.ConnectionCount));
            return graph;
        }

        internal float GetBestPathCost(Station from, Station to)
        {
            // Temporary
            return UnityEngine.Random.Range(2.0f, 45.4f);
        }

        #region Search in the graph (pathfinding)       
        const float CONNECTIONDELAY = 2.0f; // This gives the time you need when you want to change of line (staying on the same station)
        const float DELAYPERSTATION = 3.0f;   // This gives the time per station on the same line.         
        internal class Path
        {
            public float Cost { get; set; }

        }

        internal class Node
        {

        }

        internal class Segment
        {
            public Line Line { get; set; }
            public int StationCount { get; set;}

            public bool IsValid { get; set; }
            internal Segment(Line line, Station from, Station to)
            {
                if (line.Stations.Contains(from) && line.Stations.Contains(to))
                {
                    StationCount = Math.Abs(line.Stations.IndexOf(from) - line.Stations.IndexOf(to));
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                    StationCount = 9999; // Arbitrary high value for errors, so there is no chance for this Invalid segment to be kept
                }
            }
        }

        public IEnumerable<Station> GetBestPath(Station station1, Station station2)
        {
            // Temporary simplification: returns the first then the 2nd station, as if they were connected.
            yield return station1;
            yield return station2;

            //bool changes = true;
            //while(changes)
            //{

            //}
        }

        #endregion
    }
}
