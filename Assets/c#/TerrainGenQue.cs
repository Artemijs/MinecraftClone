﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TerrainGenQue : MonoBehaviour
{
	public delegate void MakeChunkAction<T1, T2>( T1 a, T2 b);
	static MakeChunkAction< Chunk, Vector3> _remakeChunkAction;
	static List<Pair<Chunk, Vector3Int>> _mcaQueParameters;
	public int _actionsPerFrame;
    // Start is called before the first frame update
    void Start()
    {
		_mcaQueParameters = new List<Pair<Chunk, Vector3Int>>();
		_remakeChunkAction = ChunkGenerator.RemakeChunk;

	}

    // Update is called once per frame
    void Update()
    {
		if (_mcaQueParameters.Count <= 0) return;
		int len = getActionsPerFrameVar();
		for (int i = 0; i < len; i++) {
			_remakeChunkAction.Invoke( _mcaQueParameters[i].one, _mcaQueParameters[i].two);
		}
		_mcaQueParameters.RemoveRange(0, len);
		Debug.Log(_mcaQueParameters.Count);
    }
	int getActionsPerFrameVar() {
		if (_mcaQueParameters.Count < _actionsPerFrame)
		{
			return _mcaQueParameters.Count;
		}
		else return _actionsPerFrame;
	}
	public static void Add2Que(Chunk c, Vector3Int pos) {
		_mcaQueParameters.Add(new Pair<Chunk, Vector3Int>(c, pos));
	}
	
}
