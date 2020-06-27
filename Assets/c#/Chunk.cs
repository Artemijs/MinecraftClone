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
public struct BlockData
{
	public bool _on;
};
//List<int> lst = ints.OfType<int>().ToList(); // this isn't going to be fast.
public class Chunk {
	public static Vector3 _size = new Vector3 (10, 10, 10);
	//Block[] _allBlocks;
	Vector3 _position;
	Mesh _mesh;
	GameObject _collider;
	List<BlockData> _allBlocks;
	public Chunk(Vector3 chunkPos, Mesh mesh, GameObject collider,
		List<BlockData> blocks) {
		_position = chunkPos;
		_mesh = mesh;
		_collider = collider;
		_allBlocks = blocks;
		//_boolMap = bools;

	}
	public void CreateCube(Vector3Int pos) {
		ChunkGenerator.CreateCube(pos,  _mesh, _allBlocks);
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;
	}
	public void DeleteCube(Vector3Int pos)
	{
		//ChunkGenerator.Temp(pos, ref _mesh, ref _boolMap, ref _allBlocks);
		ChunkGenerator.DeleteCube(pos, _mesh, _allBlocks);
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;
		
	}
	public GameObject Collider {
		get { return _collider; }
		set { _collider = value; }
	}


















	public Chunk(Vector3 chunkPos){
		/*_collider = null;
		int nrOfCubes = (int)(Chunk._size.x * Chunk._size.y * Chunk._size.z);
		_mesh = new Mesh();
		//number of sides of cube * number of vertices per side * number of cubes
		Vector3[] vertices = new Vector3[6*4*nrOfCubes];
		Vector2[] uvs = new Vector2[vertices.Length];
		//number of sides of cube * number of cubes * number of triangles per side
		List<int> tris = new List<int>();
		//tri pattern 0 1 2 || 3 2 1
		Vector3[] normals = new Vector3[vertices.Length];
		BlockFactory bFactory = BlockFactory.Instance;
		Vector3 bSize = Block._size;
		_allBlocks = new Block[(int)(_size.x * _size.y * _size.z)];



		int index = 0;
		int nrOfVisibleBlocks = 0;
		//set block data: type, position
		//create a set of vertices and normals for every block
		for (int x = 0; x < _size.x; x++) {
			for (int y = 0; y < _size.y; y++) {
				for (int z = 0; z < _size.z; z++) {
					_allBlocks [index] = new Block (new Vector3(x * bSize.x , y * bSize.y, z * bSize.z) + chunkPos);
					//set type and pos of every block
					bool[] nBools = bFactory.SetBlockType(ref _allBlocks[index]);
					if (nBools != null) nrOfVisibleBlocks++;
					//create a set of vertices for EVERY block
					bFactory.MakeCubeVertsNorms(ref vertices, ref normals, index, _allBlocks[index].Position - chunkPos);
					//create triangles if any
					bFactory.CreateTris(ref tris, nBools, index);
					bFactory.SetUvs (ref uvs, _allBlocks[index], index);
					index++;
				}
			}
		}
		_position = chunkPos;
		_mesh.vertices = vertices;
		_mesh.uv = uvs;
		_mesh.triangles = tris.ToArray();
		_mesh.normals = normals;
		if (nrOfVisibleBlocks <= 0) return;
		_collider = new GameObject();
		_collider.transform.position = _position;
		_collider.AddComponent<MeshCollider>();
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;*/
	}
	public void SetChunkPos(Vector3 position){
		_position = position;
	}

	private void GetNeighbors(ref Block[] array, Vector3 position){
		/*if (array.Length != 6)
			return;
		position -= _position;

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
	*/}
	public void Draw(Material mat)
	{
		Graphics.DrawMesh(_mesh, _position, Quaternion.identity, mat, 0);
	}
	public void Draw(){
		BlockFactory.Instance.Draw (_mesh, _position);
	}
	public Block GetBlock(Vector3 position){

		/*	position -= _position;
			//Debug.Log (position);
			int index = GetIndex (position);
			if (index == -1)
				return null;
			return _allBlocks [index];*/
		return null;
	}
	public Block GetBlock(int index){
		return null;// _allBlocks [index];
	}
	public int GetIndex(Vector3 position){
		Vector3 pos = new Vector3(Mathf.Round(position.x/Block._size.x), Mathf.Round(position.y/Block._size.y), Mathf.Round(position.z/Block._size.z));
		if (pos.x < 0 || pos.y < 0 || pos.z < 0)
			return -1;
		return (int)(pos.x * (_size.y * _size.z) + pos.y * (_size.z) + pos.z);
	}
	public Vector3 GetPosition(){
		return _position;
	}
	public void RecalculateChunk(){
		/*	int index = 0;
			Vector2[] uvs = new Vector2[_mesh.vertexCount];
			List<int> tris = new List<int>();
			BlockFactory bFactory = BlockFactory.Instance;
			int nrOfVisibleBlocks = 0;
			for (int x = 0; x < _size.x; x++) {
				for (int y = 0; y < _size.y; y++) {
					for (int z = 0; z < _size.z; z++) {
						_allBlocks [index].Position = new Vector3(x * Block._size.x , y * Block._size.y, z * Block._size.z)+_position;
						//set type of every block
						bool[] nBools = bFactory.SetBlockType(ref _allBlocks[index]);
						if (nBools != null) nrOfVisibleBlocks++;
						//create triangles if any
						bFactory.CreateTris(ref tris, nBools, index);
						bFactory.SetUvs (ref uvs, _allBlocks[index], index);
						index++;

					}
				}
			}
			_mesh.uv = uvs;
			_mesh.triangles = tris.ToArray();
			if (nrOfVisibleBlocks <= 0) return;
			if(_collider == null)
				_collider = new GameObject();
			_collider.transform.position = _position;
			_collider.AddComponent<MeshCollider>();
			_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;*/
		//return null;
	}
}
