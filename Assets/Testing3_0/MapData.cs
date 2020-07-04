using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapData : MonoBehaviour
{
		Node _map;
		public Material _material;

    // Start is called before the first frame update
	void Awake()
	{
		MeshGenerator.Init();
			TerrrainGen.SetUp();
		Chunk._size = 5;
		Chunk._uSize = Chunk._size * 1;

		Sector._sSize = 10;
		Sector._suSize = Sector._sSize * Chunk._uSize;
	}
	void Start() {
			//Vector3 pPos = new Vector3(UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(300, 1000));
			Vector3 pPos = new Vector3(0, 0, 0);
			_map = new Node(pPos, Vector3Int.zero, 3, 0, null);
			BlockData bd = _map.GetBlock(StaticFunctions.Vector3F2Int(pPos));
			Sector current = (Sector)(_map.Search(10, 0, StaticFunctions.Vector3F2Int(pPos)));
			_map.InitBlockData();
			Debug.Log(bd.Type);
			MeshGenerator.GenerateMesh(current);
	}

	// Update is called once per frame
	void Update()
	{
			_map.Draw(_material);
	}
	//GetBlockFromPosition()
	public BlockData GetBlockFromPosition(Vector3 pos) {

		return null;
	}
}
	




