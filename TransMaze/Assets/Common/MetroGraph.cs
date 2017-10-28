using System;
using System.Collections.Generic;
using System.Linq;
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
        internal void AddLine(string name, params string[] stationNames)
        {

            Line line = new Line(name);
            Lines.Add(line);
            Station previousStation = null; // used to compute connections
            Debug.LogFormat("Ligne {0}: {1}", name, string.Join(", ", stationNames));
            foreach (string stationName in stationNames)
            {
                Station station = null;
                if (Stations.ContainsKey(stationName))
                    station = Stations[stationName];
                else
                {
                    station = new Station(stationName);
                    Stations[stationName] = station;
                }
                line.AddStation(station);
                // Manage connections in both directions
                station.AddConnection(previousStation, line, false);
                if (previousStation != null) previousStation.AddConnection(station, line, true);
                previousStation = station;
            }
        }

        /// <summary>
        /// Automatically build a new Graph based on its description from a text file. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static internal MetroGraph ImportFile(string fileName = "ListeStations.txt")
        {
            var t1 = Resources.Load(fileName);
            if (t1 == null)
            {
                Debug.LogErrorFormat("The file {0} do not exist.", fileName);
                return null;
            }
            TextAsset textAsset = t1 as TextAsset;
            if (textAsset == null)
            {
                Debug.LogErrorFormat("The file {0} exists but is not a valid TextAsset.", fileName);
                return null;
            }
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
            Debug.LogFormat("Number of lines imported: {0}", graph.Lines.Count);
            Debug.LogFormat("Number of stations: {0} ({1}/{2} are shared, {3} avg connections per station)",
                graph.Stations.Count,
                graph.Lines.Sum(lin => lin.Stations.Count) - graph.Stations.Count,
                graph.Lines.Sum(lin => lin.Stations.Count),
                graph.Stations.Count == 0 ? "NA" : graph.Stations.Average(st => st.Value.ConnectionCount).ToString("N2"));
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
            // When line = null, it means that we are outside the station (starting and ending points) 
            internal Node(Station station, Line line, bool direction, int id)
            {
                Station = station;
                Line = line;
                Direction = direction;
                Id = id;
            }
            internal int Id { get; set; }
            Station Station { get; set; }
            Line Line { get; set; }
            bool Direction { get; set; }
        }


        internal IEnumerable<Station> GetBestPath(Station station1, Station station2)
        {
            List<Node> Nodes = new List<Node>();
            int nodeId = 1;
            foreach (Line line in Lines)
                foreach (Station station in line.Stations)
                {
                    Nodes.Add(new Node(station, line, true, nodeId++));
                    Nodes.Add(new Node(station, line, false, nodeId++));
                }
            foreach (Station station in Stations.Values)
                Nodes.Add(new Node(station, null, true, nodeId++));


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
