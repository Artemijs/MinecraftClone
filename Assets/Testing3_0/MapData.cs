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
		Chunk._material = _material;
		MeshGenerator.Init();
		TerrrainGen.SetUp();
		Chunk._size = 5;
		Chunk._uSize = Chunk._size * 1;
		List<int> _nodeChildCount = StaticFunctions._nodeChildCount;
		List<int> _uSizeArray = StaticFunctions._nodeChildCount;
		Sector._sSize = 10;
		Sector._suSize = Sector._sSize * Chunk._uSize;

		_nodeChildCount = new List<int>();
		_nodeChildCount.AddRange(new int[] { 5, 10, 2, 2, 2, 2, 2 });
		_uSizeArray = new List<int>();
		_nodeChildCount.Add(5);//blocks in chunk 5x5x5
		_uSizeArray.Add(5);
		_nodeChildCount.Add(Sector._sSize);//chunks in sector 10x10x10
		_uSizeArray.Add(_uSizeArray[0] * _nodeChildCount[1]);
		_nodeChildCount.Add(2);//the rest is a 2x2x2

		for (int depth = 1; depth < StaticFunctions._maxDepth; depth++) {
			_uSizeArray.Add(_uSizeArray[depth] * _nodeChildCount[depth+1]);
			_nodeChildCount.Add(2);
		}
		StaticFunctions._nodeChildCount = _nodeChildCount;
		StaticFunctions._uSizeArray = _uSizeArray;
	}
	void Start() {
		//Vector3 pPos = new Vector3(UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(300, 1000));
	}

	// Update is called once per frame
	void Update()
	{
			
	}
	//GetBlockFromPosition()
	public BlockData GetBlockFromPosition(Vector3 pos) {

		return null;
	}
}
	




