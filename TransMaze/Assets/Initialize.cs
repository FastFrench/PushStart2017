using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MetroHelper.CurrentStation = MetroHelper.GetRandomStation;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
