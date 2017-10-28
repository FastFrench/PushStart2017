using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationGenerator : MonoBehaviour {

	//Ce script sert à récupérer les infos de la gare en cours

	//compte du nombre de stations: 
	public int NbrStations;


	// Use this for initialization
	void Start () {

		MetroHelper.CurrentStation = MetroHelper.GetRandomStation;
		
	}

	// Update is called once per frame
	void Update () {
		NbrStations = MetroHelper.CurrentStation.ConnectionCount;
		Debug.Log (NbrStations);
		
	}
}
