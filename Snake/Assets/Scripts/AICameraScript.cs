using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Sets camera position according to the size of the map
        transform.position = new Vector3(11 * DataScript.Multiplier / 2, 11 * DataScript.Multiplier / 2, -11 * (DataScript.Multiplier*0.8f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
