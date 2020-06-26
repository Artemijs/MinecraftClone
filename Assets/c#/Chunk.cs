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
	public static Vector3 _size = new Vector3 (5, 5, 5);
	Block[] _allBlocks;
	Vector3 _position;
	Mesh _mesh;
	GameObject _collider;
	bool[] _boolMap;
	public Chunk(Vector3 chunkPos, int thing) {
		_collider = null;
		int nrOfCubes = (int)(_size.x * _size.y * _size.z);
		_mesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();
		List<Vector3> normals = new List<Vector3>();
		BlockFactory bFactory = BlockFactory.Instance;
		Vector3 bSize = Block._size;
		_allBlocks = new Block[(int)(_size.x * _size.y * _size.z)];
		_boolMap = new bool[(int)(_size.x * _size.y * _size.z)];
		int index = 0;
		int nrOfVisibleBlocks = 0;
		//create a bool map
		Dictionary<int, Vector3> _temp = new Dictionary<int, Vector3>();
		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				for (int z = 0; z < _size.z; z++)
				{
					_allBlocks[index] = new Block(new Vector3(x * bSize.x, y * bSize.y, z * bSize.z) + chunkPos);
					//set type and pos of every block
					bool[] nBools = bFactory.SetBlockType(ref _allBlocks[index]);
					if (nBools != null) {
						_boolMap[index] = (nBools != null);
						nrOfVisibleBlocks++;
					}
					_temp.Add(index, new Vector3(x, y, z));
					index++;
				}
			}
		}
		Vector3Int pos = new Vector3Int();
		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				for (int z = 0; z < _size.z; z++)
				{
					pos.x = x; pos.y = y; pos.z = z;
					if (GetBoolFromPos(pos)){
						//get neighbours 
						bool[] bools = GetNeighbours(pos);
						MakeCubeMesh(bools, pos, ref vertices, ref tris);
					}
				}
			}
		}
		_mesh.vertices = vertices.ToArray();
		//_mesh.uv = uvs;
		_mesh.triangles = tris.ToArray();
		//_mesh.normals = normals;
		_mesh.RecalculateNormals();
	}
	void MakeCubeMesh(bool[] nbools, Vector3Int pos, ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs) {
		if (nbools[0]){//left
			MakeLeftPlane(ref verts, ref tris, ref uvs, pos);
		}
		if (nbools[1])
		{//right
			MakeRightPlane(ref verts, ref tris, ref uvs, pos);
		}
		if (nbools[2])
		{//forward
			MakeForwardPlane(ref verts, ref tris, ref uvs, pos);
		}
		if (nbools[3])
		{//back
			MakeBackPlane(ref verts, ref tris, ref uvs, pos);
		}
		if (nbools[4])
		{//top
			MakeTopPlane(ref verts, ref tris, ref uvs, pos);
		}
		if (nbools[5])
		{//bottom
			MakeBotPlane(ref verts, ref tris, ref uvs, pos);
		}
	}
	void MakeTopPlane(ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs, Vector3Int pos) {
		float s = 0.5f;
		verts.Add(new Vector3(pos.x - s, pos.y + s, pos.z - s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z - s));
		verts.Add(new Vector3(pos.x - s, pos.y + s, pos.z + s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
		int start = verts.Count - 4;
		tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});
		
	}
	void MakeLeftPlane(ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs, Vector3Int pos)
	{
		//vert 0
		float s = 0.5f;
		verts.Add(new Vector3(pos.x - s, pos.y - s, pos.z + s));
		verts.Add(new Vector3(pos.x - s, pos.y + s, pos.z + s));
		verts.Add(new Vector3(pos.x - s, pos.y - s, pos.z - s));
		verts.Add(new Vector3(pos.x - s, pos.y + s, pos.z - s));
		int start = verts.Count - 4;
		tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	void MakeRightPlane(ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs, Vector3Int pos)
	{
		//vert 0
		float s = 0.5f;
		verts.Add(new Vector3(pos.x + s, pos.y - s, pos.z - s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z - s));
		verts.Add(new Vector3(pos.x + s, pos.y - s, pos.z + s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
		int start = verts.Count - 4;
		tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	void MakeBotPlane(ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs, Vector3Int pos)
	{
		//vert 0
		float s = 0.5f;
		verts.Add(new Vector3(pos.x + s, pos.y - s, pos.z - s));
		verts.Add(new Vector3(pos.x + s, pos.y - s, pos.z + s));
		verts.Add(new Vector3(pos.x - s, pos.y - s, pos.z - s));
		verts.Add(new Vector3(pos.x - s, pos.y - s, pos.z + s));
		int start = verts.Count - 4;
		tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	void MakeForwardPlane(ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs, Vector3Int pos)
	{
		//vert 0
		float s = 0.5f;
		verts.Add(new Vector3(pos.x + s, pos.y - s, pos.z + s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
		verts.Add(new Vector3(pos.x - s, pos.y -s, pos.z + s));
		verts.Add(new Vector3(pos.x - s, pos.y + s, pos.z + s));
		int start = verts.Count - 4;
		tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	void MakeBackPlane(ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs, Vector3Int pos)
	{
		//vert 0
		float s = 0.5f;
		verts.Add(new Vector3(pos.x - s, pos.y - s, pos.z - s));
		verts.Add(new Vector3(pos.x - s, pos.y + s, pos.z - s));
		verts.Add(new Vector3(pos.x + s, pos.y - s, pos.z - s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z - s));
		int start = verts.Count - 4;
		tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	bool[] GetNeighbours(Vector3Int pos) {
		int i = 0;
		bool[] bools = new bool[6];
		Vector3Int nPos = pos + new Vector3Int(-1, 0, 0);//left
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = GetBoolFromPos(nPos);
		i++;
		nPos = pos + new Vector3Int(0, 0, 1);//right
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = GetBoolFromPos(nPos);
		i++;
		nPos = pos + new Vector3Int(0, 0, -1);//forward
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = GetBoolFromPos(nPos);
		i++;
		nPos = pos + new Vector3Int(1, 0, 0);//back
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = GetBoolFromPos(nPos);
		i++;
		nPos = pos + new Vector3Int(0, 1, 0);//top
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = GetBoolFromPos(nPos);
		i++;
		nPos = pos + new Vector3Int(0, -1, 0);//bot
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = GetBoolFromPos(nPos);


		//l r f b t b
		/*{
		   GetBoolFromPos(pos + new Vector3int(-1, 0, 0);//left
		   (GetBlockType(pos + new Vector3(-Block._size.x, 0, 0)) != BlockType.AIR ),//left
		   (GetBlockType(pos + new Vector3(Block._size.x, 0, 0)) != BlockType.AIR ),//right
		   (GetBlockType(pos + new Vector3(0, 0, Block._size.z)) != BlockType.AIR ),//forward
		   (GetBlockType(pos + new Vector3(0, 0, -Block._size.z)) != BlockType.AIR ),//back
		   (GetBlockType(pos + new Vector3(0, Block._size.y, 0)) != BlockType.AIR ),//top
		   (GetBlockType(pos + new Vector3(0, -Block._size.y, 0)) != BlockType.AIR ),//bottom
	   };*/
		return bools;
	}
	bool CheckOutOfBounds(Vector3Int pos1) {
		//if you are checking out of bounds of the array return a
		return (pos1.x < 0 || pos1.x >= _size.x ||
			pos1.y < 0 || pos1.y >= _size.y ||
			pos1.z < 0 || pos1.z >= _size.z);

	}
	public bool GetBoolFromPos(Vector3Int pos) {
		int index = (int)(pos.z + pos.y * _size.y + _size.x * _size.x * pos.x);
		return _boolMap[index];
	}
	public Chunk(Vector3 chunkPos){
		_collider = null;
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
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;
	}
	public void SetChunkPos(Vector3 position){
		_position = position;
	}

	private void GetNeighbors(ref Block[] array, Vector3 position){
		if (array.Length != 6)
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
	}
	public void Draw(Material mat)
	{
		Graphics.DrawMesh(_mesh, _position, Quaternion.identity, mat, 0);
	}
	public void Draw(){
		BlockFactory.Instance.Draw (_mesh, _position);
	}
	public Block GetBlock(Vector3 position){
		
		position -= _position;
		//Debug.Log (position);
		int index = GetIndex (position);
		if (index == -1)
			return null;
		return _allBlocks [index];
	}
	public Block GetBlock(int index){
		return _allBlocks [index];
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
		int index = 0;
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
		_collider.GetComponent<MeshCollider>().sharedMesh = _mesh;
	}
}
