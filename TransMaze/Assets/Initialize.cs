using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common;

public class Initialize : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MetroHelper.CurrentStation = MetroHelper.GetRandomStation;
        Station destination = MetroHelper.GetRandomStation;
        Debug.LogFormat("Search best path from {0} to {1}", MetroHelper.CurrentStation.Name, destination.Name);
        float distance = MetroHelper.Graph.ComputeDijkstra(MetroHelper.CurrentStation, destination);
        Debug.LogFormat("Search best path from {0} to {1} : cost = {2:N2}", MetroHelper.CurrentStation.Name, destination.Name, distance);
        //var bestPath = MetroHelper.GetBestWayPath(MetroHelper.CurrentStation, destination);
        //Debug.Log(bestPath.ToString());
        //MetroHelper.Graph.
    }

    // Update is called once per frame
    void Update () {
		
	}
}
