using Assets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MetroHelper
{
    static string DataFile = "ListeStations_lv2";//"ListeStations";//"Assets/Resources/ListeStations.txt";
    static string PlanFile = "metro paris lv2";//"ListeStations";//"Assets/Resources/ListeStations.txt";

    /// <summary>
    /// Initialized by calling NS_InitialiseNetwork method
    /// It will provide the Plan texture corresponding to the level set. 
    /// </summary>
    public static Texture PlanTexture = null;
    private static MetroGraph _graph = null;

    public static Station CurrentStation { get; set; }
    public static Station DestinationStation { get; set; }
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

    #region network state
    /// <summary>
    /// Says if the next station can be reached from currentStation using the Line lineUsed, in a given direction
    /// </summary>
    /// <param name="currentStation"></param>
    /// <param name="destinationStation"></param>
    /// <param name="lineUsed"></param>
    /// <returns></returns>
    public static bool NS_IsConnectionAvalaible(Station currentStation, Line lineUsed, bool direction)
    {
        return true;
    }


    /// <summary>
    /// Says if there is any trouble on the given line from the currentStation in a given direction. 
    /// The returned string is null if no problem exists
    /// </summary>
    /// <param name="currentStation"></param>
    /// <param name="destinationStation"></param>
    /// <param name="lineUsed"></param>
    /// <returns></returns>
    public static string NS_NetworkTrouble(Station currentStation, Line lineUsed, bool direction)
    {
        return null;
    }

    /// <summary>
    /// Gets the first station on the line, from current station, and given direction, with a problem. null if none.
    /// </summary>
    /// <param name="currentStation"></param>
    /// <param name="destinationStation"></param>
    /// <param name="lineUsed"></param>
    /// <returns></returns>
    public static Station NS_FindFirstTroubleOnLine(Station currentStation, Line lineUsed, bool direction)
    {
        return null;
    }

    /// <summary>
    /// Gets the first station on the line, from current station, and given direction, with a problem. null if none.
    /// </summary>
    /// <param name="currentStation"></param>
    /// <param name="destinationStation"></param>
    /// <param name="lineUsed"></param>
    /// <returns></returns>
    public static IEnumerable<string> NS_FindAllTroubleMessagesForNetwork(int limitNbMessages, bool random)
    {
        yield break;
    }

    public static int DifficultyLevel { get; set; }
    /// <summary>
    /// Initializes the game data for a given level of difficulty. 
    /// - select proper network configuration (done)
    /// - initialize the Plan Texture (done)
    /// - Set a random Current and Destination stations (done)
    /// - Set the permanent obstacles (to do)
    /// </summary>
    /// <param name="level">This is difficulty level, 1 for very eay, 3 for challenging</param>
    /// <returns></returns>
    public static void InitialiseNetwork(int level)
    {
        if (level < 1) level = 1;
        if (level > 3) level = 3;
        DifficultyLevel = level;
        switch (level)
        {
            case 1:
                DataFile = "ListeStations_lv1";
                PlanFile = "metro paris lv1";
                break;
            case 2:
                DataFile = "ListeStations_lv2";
                PlanFile = "metro paris lv2";
                break;
            case 3:
                DataFile = "ListeStations_lv3";
                PlanFile = "metro paris lv3";
                break;
            default:
                Debug.LogError("Internal error");
                break;
        }
        _graph = null;
        Graph.InitLevel(DifficultyLevel);
        PlanTexture = Resources.Load<Texture>("PlanFile");
        if (PlanTexture == null)
            Debug.LogErrorFormat("The bitmap {0} for the plan can't be loaded", PlanFile);
        CurrentStation = GetRandomStation;
        DestinationStation = GetRandomStation;
    }
    #endregion network state

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
        if (nbElement == 0) return default(T);
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
    /// Returns the Station with a given name. Throws an exception if the station does not exist.  
    /// </summary>
    public static Station GetStation(string name)
    {
            return Graph.Stations.FirstOrDefault(st=>st.Value.Name.ToUpperInvariant() == name.ToUpperInvariant()).Value;
    }

    /// <summary>
    /// Returns the Station with a given numeric id.  
    /// </summary>
    public static Station GetStation(int id)
    {
        if (id < 0 || id >= Graph.Stations.Count) return null;
        return Graph.Stations.Values.OrderBy(st=>st.Name).ElementAt(id);
    }

    /// <summary>
    /// Returns the name of all the stations, sorted in alphabetique order.  
    /// </summary>
    public static List<string> GetStationNames()
    {
        return Graph.Stations.Keys.OrderBy(st => st).ToList();
    }

    ///// <summary>
    ///// Returns the best path to go from one station to another
    ///// </summary>
    ///// <param name="line"></param>
    ///// <param name="direction"></param>
    ///// <returns></returns>
    //public static Path GetBestWayPath(Station from, Station to)
    //{
    //    return Graph.GetBestPath(from, to);
    //}

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
