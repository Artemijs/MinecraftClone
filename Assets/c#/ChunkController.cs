using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController {
	Chunk[,,] _chunks;
	Vector3 _cSize; //number of chunks in the _chunks array
	Vector3 _cDistance;
	Vector3 _origin;
	//I PITY THE FOOL THAT READS THIS
	//U FUUKING KNEW ID HAV E TO READ THIS U CUNT 
	public ChunkController(){
		_cSize = World._worldSize;
		_chunks = new Chunk[(int)_cSize.x, (int)_cSize.y, (int)_cSize.z];
		_origin = new Vector3 (0,0,0);
		
		Vector3 playerPos = GameObject.Find ("PlayerModel").transform.position;
		int a = (int)((playerPos.x - _origin.x)/(Chunk._size.x*Block._size.x));
		int b = (int)((playerPos.y - _origin.y)/(Chunk._size.y*Block._size.y));
		int c = (int)((playerPos.z- _origin.z)/(Chunk._size.z*Block._size.z));
		_cDistance = new Vector3 (a, b, c);
		Vector3 hws = World._worldSize * 0.5f;
		for (int x = (int)(_cDistance.x - (hws.x)); x < _cDistance.x + Mathf.RoundToInt (hws.x + 0.01f); x++) {
			//for (int y = (int)(_cDistance.y - (hws.y)); y < _cDistance.y + Mathf.RoundToInt (hws.y + 0.01f); y++) {
			for (int y = 0; y < World._worldSize.y; y++) {
				for (int z = (int)(_cDistance.z - (hws.z)); z < _cDistance.z + Mathf.RoundToInt (hws.z + 0.01f); z++) {
					Vector3 pos = new Vector3 (x * Chunk._size.x, y * Chunk._size.y, z * Chunk._size.z);
					//+ offset;
					//ix = 
					//_chunks[x, y, z] = new Chunk (pos);
					_chunks [x % (int)(World._worldSize.x), y % (int)(World._worldSize.y), z % (int)(World._worldSize.z)] = new Chunk (pos);
				}
			}
		}
		_origin = new Vector3 (
			((int)(World._worldSize.x*0.5f)) * Chunk._size.x * Block._size.x,
			((int)(World._worldSize.y*0.5f)) * Chunk._size.y * Block._size.y,
			((int)(World._worldSize.z*0.5f)) * Chunk._size.z * Block._size.z);
	}
	public void Update(Vector3 playerPos){
		//Chunk c = _chunks [0];
		int x = (int)((playerPos.x - _origin.x)/(Chunk._size.x*Block._size.x));
		int y = (int)((playerPos.y - _origin.y)/(Chunk._size.y*Block._size.y));
		int z = (int)((playerPos.z- _origin.z)/(Chunk._size.z*Block._size.z));
		//Debug.Log ("int distance from origin " + x);
		//return;
		if (x != _cDistance.x || y != _cDistance.y || z != _cDistance.z) {
			//changed blocks

			//2 bools per axis
			bool[] result = new bool[6];
			//check which axis has moved
			CheckMoveAxis(ref result, x, y, z);
			OffsetChunks (result);
			_cDistance = new Vector3(x,y,z);

		}
	}
	public void Draw(){
		for (int x = 0; x < _cSize.x; x++) {
			for (int y = 0; y < _cSize.y; y++) {
				for (int z = 0; z < _cSize.z; z++) {
					_chunks [x, y, z].Draw ();
				}
			}
		}
	}
	Block GetBlock(Vector3 position){
		Block b;

		int x = (int)(Mathf.Round(position.x)/(Chunk._size.x*Block._size.x)) % (int)(World._worldSize.x);
		int y = (int)(Mathf.Round (position.y) / (Chunk._size.y * Block._size.y));// % (int)(World._worldSize.y);
		int z = (int)(Mathf.Round(position.z)/(Chunk._size.z*Block._size.z)) % (int)(World._worldSize.z);
		if(x >= _cSize.x || y >= _cSize.y || z >= _cSize.z ||
			x < 0 || y < 0 || z < 0 ){

			return null;

		}
		//Debug.Log ("indices : "+x + ", " + y + ", " + z);
		//Debug.Log ("position : "+position.x + ", " + position.y + ", " + position.z);
		Chunk c = _chunks [x, y, z];

		b = c.GetBlock (position);

		return b;	
	}
	public Block GetNeighbor(BlockSide bs, Vector3 position){
		if (bs == BlockSide.BOTTOM) {
			position += new Vector3 (0, -Block._size.y, 0);
		}
		else if (bs == BlockSide.TOP) {
			position += new Vector3 (0, Block._size.y, 0);
		}
		else if (bs == BlockSide.FORWARD) {
			position += new Vector3 (0, 0, Block._size.z);
		}
		else if (bs == BlockSide.BACK) {
			position += new Vector3 (0, 0, -Block._size.z);
		}
		else if (bs == BlockSide.LEFT) {
			position += new Vector3 (-Block._size.x, 0, 0);
		}
		else if (bs == BlockSide.RIGHT) {
			position += new Vector3 (Block._size.x, 0, 0);
		}
		return GetBlock(position);
	}
	public int GetBlockId(Vector3 position){
		int x = (int)(position.x/(Chunk._size.x*Block._size.x));
		int y = (int)(position.y/(Chunk._size.y*Block._size.y));
		int z = (int)(position.z/(Chunk._size.z*Block._size.z));
		if(x >= _cSize.x || y >= _cSize.y || z >= _cSize.z ||
			x < 0 || y < 0 || z < 0 ){
			return -1;
		}
		return _chunks [x, y, z].GetIndex (position);
	}
	private void CheckMoveAxis(ref bool[] result, int x, int y, int z){
		int index = 0;
		if (x != _cDistance.x) {
			//player has moved on this axis 
			result [index] = true;
			if (x > _cDistance.x) {
				result [index + 1] = true;
			} else
				result [index + 1] = false;
			
		} else {
			//player hasnt moved on this axis: 0 0
			result [index] = false;
			result [index + 1] = false;
		}
		index = 2;
		if (y != _cDistance.y) {
			//player has moved on this axis 
			result [index] = true;
			if (y > _cDistance.y) {
				result [index + 1] = true;
			} else
				result [index + 1] = false;

		} else {
			//player hasnt moved on this axis: 0 0
			result [index] = false;
			result [index + 1] = false;
		}
		index = 4;
		if (z != _cDistance.z) {
			//player has moved on this axis 
			result [index] = true;
			if (z > _cDistance.z) {
				result [index + 1] = true;
			} else
				result [index + 1] = false;

		} else {
			//player hasnt moved on this axis: 0 0
			result [index] = false;
			result [index + 1] = false;
		}
	}
	private void ReChunkDaChunk(ref Chunk c){
		c.RecalculateChunk ();
	}
	private void OffsetChunks(bool[] axis){
		//axis = bool[6], 2bools per axis x, y, z
		//1st bool : has player moved on axis
		//2nd bool : direction of movement

		if(axis[0]){//x axis
			Debug.Log("MOVED ON X");
			if (axis [1]) {
				//+1 to current pos
				//find out which chunk needs to move 
				//0 + _cDistance.x % World._size
				int cIndex = (int)(_cDistance.x) % (int)(World._worldSize.x);
				if (cIndex > World._worldSize.x || cIndex < 0) {
					Debug.Log ("x index "+cIndex+" max index "+World._worldSize.x);
				}
				float xPos = (_cDistance.x + (World._worldSize.x)) * Chunk._size.x * Block._size.x;
				for( int i =0; i < World._worldSize.y; i++){
					for( int j =0; j < World._worldSize.z; j++){
						Chunk c = _chunks[ cIndex, i, j];
						// how do you actually move this bitch
						Vector3 pos = c.GetPosition();
						pos.x = xPos;
						c.SetChunkPos(pos);
						ReChunkDaChunk (ref c);
					}
				}
				//
			} else {
			//-1 to current pos
				int cIndex = (int)(World._worldSize.x - 1 + _cDistance.x) % (int)(World._worldSize.x);
				float xPos = (_cDistance.x - 1) * Chunk._size.x * Block._size.x;
				for( int i =0; i < World._worldSize.y; i++){
					for( int j =0; j < World._worldSize.z; j++){
						Chunk c = _chunks[ cIndex, i, j];
						// how do you actually move this bitch
						Vector3 pos = c.GetPosition();
						pos.x = xPos;
						c.SetChunkPos(pos);
						ReChunkDaChunk (ref c);
					}
				}
		
			}
		}
		if(axis[4]){//z axis
			Debug.Log("MOVED ON z");
			if (axis [5]) {
				//+1 to current pos
				//find out which chunk needs to move 
				//0 + _cDistance.x % World._size
				int cIndex = (int)(_cDistance.z) % (int)(World._worldSize.z);
				if (cIndex > World._worldSize.z || cIndex < 0) {
					Debug.Log ("z index "+cIndex+" max index "+World._worldSize.z);
				}
				float zPos = (_cDistance.z + (World._worldSize.z)) * Chunk._size.z * Block._size.z;
				for( int i =0; i < World._worldSize.y; i++){
					for( int j =0; j < World._worldSize.x; j++){
						Chunk c = _chunks[ j, i, cIndex];
						// how do you actually move this bitch
						Vector3 pos = c.GetPosition();
						pos.z = zPos;
						c.SetChunkPos(pos);
						ReChunkDaChunk (ref c);
					}
				}
				//
			} else {
			//-1 to current pos
				int cIndex = (int)(World._worldSize.z - 1 + _cDistance.z) % (int)(World._worldSize.z);
				float zPos = (_cDistance.z - 1) * Chunk._size.z * Block._size.z;
				for( int i =0; i < World._worldSize.y; i++){
					for( int j =0; j < World._worldSize.x; j++){
						Chunk c = _chunks[ j, i, cIndex];
						// how do you actually move this bitch
						Vector3 pos = c.GetPosition();
						pos.z = zPos;
						c.SetChunkPos(pos);
						ReChunkDaChunk (ref c);
					}
				}
		
			}
		}
	}

}
