using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int a = 1;
		int b = 1;
		int c = 3;
		Debug.Log ((c | b));
		if (a == (c | b)) {
			Debug.Log ("TRUE");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
