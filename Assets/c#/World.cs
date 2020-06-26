using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
	bool _ready = false;
	// Use this for initialization
	//public static Vector3 _worldSize = new Vector3(10, 3, 10);
	public static Vector3 _worldSize = new Vector3(20, 20, 20);
	GameObject _player;
	ChunkController _chunkCtrl;
	void Start () {
		_player = GameObject.Find ("PlayerModel");
		_player.transform.position = new Vector3 (
			((int)(_worldSize.x*0.5f)) * Chunk._size.x * Block._size.x,
			_player.transform.position.y,
			((int)(_worldSize.z*0.5f)) * Chunk._size.z * Block._size.z)
			+ new Vector3 (90, 0, 100);
		_chunkCtrl = new ChunkController ();
		_ready = true;

	}
	
	// Update is called once per frame
	void Update () {
		if( !_ready){
			return;
		}
		_chunkCtrl.Update (_player.transform.position);
		_chunkCtrl.Draw ();

	}
	public bool Ready{
		get{ return _ready;}
		set{ _ready = value;}
	}
	public ChunkController ChunkCtrl{
		get{ return _chunkCtrl;}
	}
}
