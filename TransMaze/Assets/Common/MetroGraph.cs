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
            //Debug.LogFormat("Ligne {0}: {1}", name, string.Join(", ", stationNames));
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
        static internal MetroGraph ImportFile(string fileName = "ListeStations")
        {
            Debug.LogFormat("Reading file {0}...", fileName);
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            MetroGraph graph = new MetroGraph();
            try
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
                var fullText = textAsset.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                Debug.LogFormat("Analysis of the {0} lines...", fullText.Length);

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
            }
            finally
            {
                Debug.LogFormat("Reading time for the file: {0:N3}s.", sw.Elapsed.TotalSeconds);
            }
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

        /// <summary>
        /// Build the permanent obstacles
        /// </summary>
        /// <param name="difficultyLevel"></param>
        internal void InitLevel(int difficultyLevel)
        {
            
        }

        private float GetAtomicCost(IMetroLocation c1, IMetroLocation c2)
        {
            float cost = 0;
            if (c1.Station != c2.Station)
                cost += DELAYPERSTATION;
            if (c1.Line != c2.Line)
                cost += CONNECTIONDELAY;
            return cost;
        }

        const float MAX_COST = 240; // as an optimisation, we stop searching when the cost exceed this value. Set here something a bit over the maximal possible value.  

        ///// <summary>
        ///// Check all surrounding connection and select the best (by recursively check other nodes).
        ///// </summary>
        ///// <param name="current"></param>
        ///// <param name="currentCost"></param>
        ///// <param name="candidates"></param>
        ///// <param name="destination"></param>
        ///// <returns></returns>
        //private Path GetBestPath(List<Connection>  connectionsSoFar, float currentCost, Connection destination)
        //{
        //    Debug.Assert(connectionsSoFar.Count >= 1, "GetBestConnection error: connectionsSoFar can't be empty");
        //    float bestCost = MAX_COST;
        //    Connection bestChoice = null;
        //    List<Connection> newList = new List<Connection>(connectionsSoFar);
        //    Connection lastInserted = connectionsSoFar[0];
        //    if (lastInserted.Station == destination.Station) // We reached the target (no matter the line)
        //        return new Path(connectionsSoFar, currentCost); 

        //    newList.Insert(0, null);
        //    foreach (Connection connection in lastInserted.Station.ConnectedStations.Except(connectionsSoFar))
        //    {
        //        float firstStepCost = GetAtomicCost(lastInserted, connection) + currentCost;
        //        if (firstStepCost >= MAX_COST) continue;
        //        newList[0] = connection;
        //        var result = GetBestPath(newList, firstStepCost, destination);
        //        float remainingCost = result.Cost;

        //        float totalCost = firstStepCost + remainingCost;
        //        if (totalCost >= MAX_COST) continue;
        //        if (totalCost < bestCost)
        //        {
        //            bestCost = totalCost;
        //            bestChoice = connection;
        //        }
        //    }
        //    newList[0] = bestChoice;
        //    return  new Path(newList, bestCost);
        //}

        ///// <summary>
        ///// We start from outside the Metro station. So we consider 
        ///// </summary>
        ///// <param name="from"></param>
        ///// <param name="to"></param>
        ///// <returns></returns>
        //private IEnumerable<Path> GetPaths(Station from, Station to)
        //{
        //    Connection endConnection = new Connection(to, null, true);
        //    List<Connection> list = new List<Connection>();
        //    list.Add(null);
        //    if (from == to)
        //    {
        //        list[0] = endConnection;
        //        yield return new Path(list, 0);
        //        yield break;
        //    }
        //    Debug.LogFormat("");
        //    foreach (Line line in from.Lines)
        //    {
        //        Debug.LogFormat("Path from {0} to {1} on line {2}", from.Name, to.Name, line.Name);
        //        Connection startConnection = new Connection(from, line, true);
        //        list[0] = startConnection;
        //        yield return GetBestPath(new List<Connection>(list), CONNECTIONDELAY, endConnection);
        //        startConnection = new Connection(from, line, false);
        //        list[0] = startConnection;
        //        yield return GetBestPath(new List<Connection>(list), CONNECTIONDELAY, endConnection);
        //    }     
        //}

        //public Path GetBestPath(Station from, Station to)
        //{
        //    Path bestPath = null;
        //    float bestCost = MAX_COST;
        //    foreach (var pair in GetPaths(from, to))
        //    {
        //        if (pair.Cost < bestCost)
        //        {
        //            bestCost = pair.Cost;
        //            bestPath = pair;
        //        }
        //    }
        //    return bestPath;
        //}

        #region Search in the graph (pathfinding)       
        const float CONNECTIONDELAY = 5.0f; // This gives the time you need when you want to change of line (staying on the same station)
        const float DELAYPERSTATION = 1.3f;   // This gives the time per station on the same line.         


        public class Node : IMetroLocation
        {
            // When line = null, it means that we are outside the station (starting and ending points) 
            internal Node(Station station, Line line/*, bool direction*/, int id)
            {
                Station = station;
                Line = line;
                /*Direction = direction;*/
                Id = id;
            }
            internal int Id { get; set; }
            public Station Station { get; set; }
            public Line Line { get; set; }
            internal float Distance { get; set; }
            /*bool Direction { get; set; }*/
        }
        #endregion

        #region Dijkstra
        int[] closedSet { get; set; }
        int[] openedSet { get; set; }

        float[] distance { get; set; }
        List<Node> Nodes;
        Dictionary<Station, Dictionary<Line, Node>> Station2Nodes = new Dictionary<Station, Dictionary<Line, Node>>();
        public float ComputeDijkstra(Station from, Station to)
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            // Trouver le node ayant la distance la plus faible
            // Depuis un node, mettre à jour tous les nodes en contact.
            // Calculer la distance avec chaque node voisin
            Nodes = new List<Node>();
            int nodeId = 0;
            foreach (Station station in Stations.Values)
                Station2Nodes[station] = new Dictionary<Line, Node>();
            foreach (Line line in Lines)
                foreach (Station station in line.Stations)
                {
                    Node node = new Node(station, line, nodeId++);
                    if (station == from)
                        node.Distance = 0;
                    else
                        node.Distance = 99999;
                    Nodes.Add(node);
                    Station2Nodes[station][line] = node;               
                }
            
            List<Node> closed = new List<Node>();
            List<Node> opened = new List<Node>(Nodes);
            bool changed = true;
            float minDistance = MAX_COST;
            while(changed && opened.Count > 0)
            {
                float minValue = opened.Min(node => node.Distance);
                if (minValue > MAX_COST) break;
                Node nod = opened.FirstOrDefault(node => node.Distance <= minValue);
                Debug.Assert(nod != null);
                opened.Remove(nod);
                Station2Nodes[nod.Station].Remove(nod.Line);
                closed.Add(nod);
                if (nod.Station == to)
                {
                    //Debug.Log("La station d'arrivée est atteinte.");
                    minDistance = nod.Distance;
                    break;
                }
                // Update distance for each neighbourg of nod
                foreach(Connection con in nod.Station.ConnectedStations)
                {                    
                    var listDico = Station2Nodes[con.Station];
                    if (listDico!=null && listDico.ContainsKey(con.Line))
                    {
                        // Check cost to go from nod to con
                        Node connectedNode = listDico[con.Line];
                        float cost = GetAtomicCost(nod, connectedNode);
                        float dold = listDico[con.Line].Distance;
                        float dnew = nod.Distance+cost;
                        if (dold > dnew)
                            listDico[con.Line].Distance = dnew;
                    }
                }                
            }
            //Debug.LogFormat("Durée du traitement: {0}. Closed list contains {1} values", sw.Elapsed, closed.Count);
            return minDistance;
        }
        #endregion Dijkstra

    }
}
