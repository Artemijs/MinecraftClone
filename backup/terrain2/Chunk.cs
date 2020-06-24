using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockSide{
	LEFT =0,
	RIGHT,
	FORWARD,
	BACK,
	TOP,
	BOTTOM
}
public class Chunk {
	public static Vector3 _size = new Vector3 (10, 20, 10);
	Block[] _allBlocks;
	GameObject _chunkParent;
	public Chunk(Vector3 chunkPos){
		_chunkParent = new GameObject ("chunk");
		_chunkParent.transform.position = new Vector3 (_size.x * 0.5f, _size.y * 0.5f, _size.z * 0.5f);

		BlockFactory bFactory = BlockFactory.Instance;
		Vector3 bSize = Block._size;
		_allBlocks = new Block[(int)(_size.x * _size.y * _size.z)];
		int index = 0;
		for (int x = 0; x < _size.x; x++) {
			for (int y = 0; y < _size.y; y++) {
				for (int z = 0; z < _size.z; z++) {
					_allBlocks [index] = new Block (new Vector3(x * bSize.x, y * bSize.y, z * bSize.z));
					bFactory.SetBlockType(ref _allBlocks[index], chunkPos);
					index++;

				}
			}
		}
		for (int i = 0; i < _allBlocks.Length; i++) {
			Block[] neighbors = new Block[6];
			GetNeighbors (ref neighbors, _allBlocks[i].Position);
			bFactory.SetBlockMesh (ref _allBlocks [i], ref neighbors);
			bFactory.SetBlockTexture (ref _allBlocks [i]);
			_allBlocks [i].ParentMesh (_chunkParent.transform);
		}
		SetChunkPos (chunkPos);
	}
	public void SetChunkPos(Vector3 position){
		_chunkParent.transform.position = position;
	}
	private void GetNeighbors(ref Block[] array, Vector3 position){
		if (array.Length != 6)
			return;

		//the math requires the size to be  1, 1, 1 of the cube so i divide the pos by its actual size to get it to behave
		// such that is pos(0, 0.5, 0) pos.y/0.5 = 1
		position.x /= Block._size.x;
		position.y /= Block._size.y;
		position.z /= Block._size.z;
		Vector3 pos = position;
		//left
		pos.x -= 1;
		if (pos.x < 0) {
			array [(int)BlockSide.LEFT] = null;
		} else
			array [(int)BlockSide.LEFT] = _allBlocks [ (int)(pos.x*(_size.y*_size.z)+ pos.y*(_size.z)+pos.z)];
		//right
		pos = position;
		pos.x += 1;
		if (pos.x >= _size.x) {
			array [(int)BlockSide.RIGHT] = null;
		}
		else 
			array [(int)BlockSide.RIGHT] = _allBlocks [ (int)(pos.x*(_size.y*_size.z)+ pos.y*(_size.z)+pos.z)];
		//forward
		pos = position;
		pos.z += 1;
		if (pos.z >= _size.z) {
			array [(int)BlockSide.FORWARD] = null;
		}
		else 
			array [(int)BlockSide.FORWARD] = _allBlocks [ (int)(pos.x*(_size.y*_size.z)+ pos.y*(_size.z)+pos.z)];
		//back
		pos = position;
		pos.z -= 1;
		if (pos.z < 0)
			array [(int)BlockSide.BACK] = null;
		else
			array [(int)BlockSide.BACK] = _allBlocks [ (int)(pos.x*(_size.y*_size.z)+ pos.y*(_size.z)+pos.z)];



		//up
		pos = position;
		pos.y+= 1;
		if (pos.y >= _size.y)
			array [(int)BlockSide.TOP] = null;
		else {
			array [(int)BlockSide.TOP] = _allBlocks [(int)(pos.x * (_size.y * _size.z) + pos.y * (_size.z) + pos.z)];
		}
		//down
		pos = position;
		pos.y -= 1;
		if (pos.y < 0)
			array [(int)BlockSide.BOTTOM] = null;
		else
			array [(int)BlockSide.BOTTOM] = _allBlocks [ (int)(pos.x*(_size.y*_size.z)+ pos.y*(_size.z)+pos.z)];
	}
}
