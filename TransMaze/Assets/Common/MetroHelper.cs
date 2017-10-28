using Assets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class MetroHelper
{
    public static string DataFile = "ListeStations.txt";
    private static MetroGraph _graph = null;

    public static Station CurrentStation { get; set; }
    public static Line CurrentLine { get; set; }
    public static MetroGraph Graph
    {
        get
        {
            if (_graph == null)
                _graph = MetroGraph.ImportFile(DataFile);
            return _graph;
        }
    }

    /// <summary>
    /// Extension method on dictionaries, to select a random element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="dico"></param>
    /// <returns></returns>
    public static T GetRandomElement<T,K>(this Dictionary<K,T> dico)
    {
        int nbElement = dico.Count;
        return dico.Values.ElementAt(UnityEngine.Random.Range(0, nbElement));
    }

    /// <summary>
    /// Returns a random Station on the whole network. 
    /// </summary>
    public static Station GetRandomStation
    {
        get
        {
            return GetRandomElement(Graph.Stations);
        }
    }

    /// <summary>
    /// Returns the best path to go from one station to another
    /// </summary>
    /// <param name="line"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static IEnumerable<Station> GetBestWay(Station from, Station to)
    {
        return Graph.GetBestPath(from, to);
    }

    /// <summary>
    /// Returns the cost of the best path to go from one station to another
    /// </summary>
    /// <param name="line"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float GetBestWayCost(Station from, Station to)
    {
        return Graph.GetBestPathCost(from, to);
    }

    /// <summary>
    /// Returns each station of the line (either in a direction of the other, depending on direction parameter).
    /// </summary>
    /// <param name="line"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static IEnumerable<Station> GetStationsOfLine(Line line, bool direction)
    {
        if (direction)
            return line.Stations;
        var rev = line.Stations.ToList();
        rev.Reverse();
        return rev;
    }

    /// <summary>
    /// For a line in a given direction, says if the candidateStation is after the currentStation or not on the line.
    /// If the station is the same, or either of the station is not on the line, then will return false. 
    /// </summary>
    /// <param name="line"></param>
    /// <param name="currentStation"></param>
    /// <returns></returns>
    public static bool IsStationAfterCurrentOnLine(Line line, bool direction, Station currentStation, Station candidateStation)
    {
        int index = line.Stations.IndexOf(currentStation);
        if (index == -1) return false;
        int indexCandidate = line.Stations.IndexOf(candidateStation);
        if (indexCandidate == -1) return false;
        return (direction && index < indexCandidate) || (!direction && index > indexCandidate);
    }
}
