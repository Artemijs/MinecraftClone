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
public enum BlockType
{

	ROCK = 0,
	DIRT,
	DIRT_GRASS,
	TREE_WOOD,
	FROZEN_DIRT,
	FROZEN_ICE_DIRT,
	AIR,
	GRASS
};
public struct BlockData
{
	public bool _on;
	public BlockType _type;
};
//List<int> lst = ints.OfType<int>().ToList(); // this isn't going to be fast.
public class Chunk {
	public static int _size = 5;
	//Block[] _allBlocks;
	Vector3Int _position;
	Mesh _mesh;
	GameObject _collider = null;
	List<BlockData> _allBlocks;
	public Chunk(Vector3 chunkPos, List<BlockData> blocks) {
		_position = new Vector3Int( (int)chunkPos.x, (int)chunkPos.y, (int)chunkPos.z );
		_mesh = new Mesh();
		//_collider = collider;
		_allBlocks = blocks;
		_collider = new GameObject();
		_collider.transform.position = _position;
		_collider.AddComponent<MeshCollider>();
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;


	}
	public void RecalculateCollider() {
		if(_collider == null)
			_collider = new GameObject();
		_collider.transform.position = _position;
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;
	}
	public void Draw(Material mat)
	{
		Graphics.DrawMesh(_mesh, _position, Quaternion.identity, mat, 0);
	}
	public void CreateCube(Vector3Int pos, BlockType type) {
		pos -= _position;
		ChunkGenerator.CreateCube(pos, type,  _mesh, _allBlocks);
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;
	}
	public void DeleteCube(Vector3Int pos)
	{
		pos -= _position;
		//ChunkGenerator.Temp(pos, ref _mesh, ref _boolMap, ref _allBlocks);
		ChunkGenerator.DeleteCube(pos, _mesh, _allBlocks);
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;
		
	}
	public BlockData GetBlockfromPos(Vector3Int pos) {
		pos -= _position;
		int index = (int)(pos.z + pos.y * _size + _size * _size * pos.x);
		return _allBlocks[index];
	}
	public bool CheckInside(Vector3Int pos) {
		pos -= _position;
		int index = (int)(pos.z + pos.y * _size + _size * _size * pos.x);
		return (index > 0 && index < _allBlocks.Count);
	}
	public GameObject Collider {
		get { return _collider; }
		set { _collider = value; }
	}
	public void Clear() {
		//GameObject.Destroy(_collider);
		//_collider = null;
		_mesh.Clear();
	}
	public Mesh MeshObj {
		get { return _mesh; }
		set { _mesh = value; }
	}
	public Vector3Int Position { get { return _position; }set { _position = value; } }
}
