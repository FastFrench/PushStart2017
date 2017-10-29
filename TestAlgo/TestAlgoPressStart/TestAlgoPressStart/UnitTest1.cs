using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assets.Common;
using System.Diagnostics;
//using UnityEngine;

namespace TestAlgoPressStart
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        // Use this for initialization
        [TestMethod]
        public void Start()
        {
            TimeSpan ts = new TimeSpan();
            int nbLoop = 1000;
            for (int i = 0; i < nbLoop; i++)
            {
                MetroHelper.CurrentStation = MetroHelper.GetRandomStation;
                Station destination = MetroHelper.GetRandomStation;
                //Debug.LogFormat("Search best path from {0} to {1}", MetroHelper.CurrentStation.Name, destination.Name);
                float distance = MetroHelper.Graph.ComputeDijkstra(MetroHelper.CurrentStation, destination);
                ts += MetroHelper.Graph.LastOperationTime.Elapsed;
                //Debug.WriteLine("Search best path from {0} to {1} : cost = {2:N2}. Delay: {3}", MetroHelper.CurrentStation.Name, destination.Name, distance, MetroHelper.Graph.LastOperationTime.Elapsed);
                //var bestPath = MetroHelper.GetBestWayPath(MetroHelper.CurrentStation, destination);
                //Debug.Log(bestPath.ToString());
                //MetroHelper.Graph.
            }
            Debug.WriteLine("Durée moyenne de la recherche sur {0} essais: {1:N2} ms", nbLoop, ts.TotalMilliseconds / nbLoop);

        }
    }
}
