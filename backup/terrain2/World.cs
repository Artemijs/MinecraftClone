using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	// Use this for initialization
	public static Vector3 _worldSize = new Vector3(5, 3, 10);
	void Start () {
		for (int x = 0; x < _worldSize.x; x++) {
			for (int y = 0; y < _worldSize.y; y++) {
				for (int z = 0; z < _worldSize.z; z++) {
					Chunk c = new Chunk (new Vector3 (x * Chunk._size.x, y * Chunk._size.y, z * Chunk._size.z));
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
