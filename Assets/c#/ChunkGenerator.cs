using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this file assumes that every position provided is between 0 and Chunk._size
/// </summary>
public class ChunkGenerator : MonoBehaviour
{
	static List<Vector3> _vertices;
	static List<Vector2> _uvs;
	static List<int> _tris;
	static List<BlockData> _blocks;
	static Vector3 _position;
	static Mesh _mesh;
	static int _nrOfCubes;
	// Start is called before the first frame update
	void Start()
	{
		_vertices = new List<Vector3>();
		_uvs = new List<Vector2>();
		_tris = new List<int>();
		_blocks = new List<BlockData>();
		_position = new Vector3();
		_mesh = new Mesh();
		_nrOfCubes = (int)(Chunk._size * Chunk._size * Chunk._size);
	}

	public static void MakeChunk(out Chunk chunk, Vector3 position) {

		_position = position;
		int nrOfblocks = CreateChunkTypeData();
		if (nrOfblocks == 0) {
			QuickCreateChunkMesh(out chunk);
			ResetCache();
			return;
		}
		CreateChunkMeshData();
		CreateChunkMesh(out chunk);
		ResetCache();

	}
	//bd._on = (y + position.y<= 2);
	/*if (!bd._on) bd._type = BlockType.AIR;
	else {
		bd._type = (BlockType)(Random.Range(0, 5));
	}*/
	static void ResetCache() {
		_vertices.Clear();
		_uvs.Clear();
		_tris.Clear();
		_blocks.Clear();
		_position = new Vector3();
		_mesh = new Mesh();
	}
	public static int CreateChunkTypeData() {
		BlockData bd;
		int notAirCount = 0;
		//create a bool map
		for (int x = 0; x < Chunk._size; x++)
		{
			for (int y = 0; y < Chunk._size; y++)
			{
				for (int z = 0; z < Chunk._size; z++)
				{
					bd._type = TerrrainGen.GetBlockType(_position + new Vector3(x, y, z));
					bd._on = (bd._type != BlockType.AIR);
					if (bd._on)
					{
						notAirCount++;
					}
					_blocks.Add(bd);
				}
			}
		}
		return notAirCount;
	}
	public static void CreateChunkMeshData() {
		BlockData bd;
		Vector3Int pos = new Vector3Int();
		for (int x = 0; x < Chunk._size; x++)
		{
			for (int y = 0; y < Chunk._size; y++)
			{
				for (int z = 0; z < Chunk._size; z++)
				{
					pos.x = x; pos.y = y; pos.z = z;
					bd = GetBlockFromPos(pos);
					if (bd._on)
					{
						//get neighbours 
						BlockData[] nBlocks = GetNeighbours(pos );
						MakeCubeMesh(nBlocks, bd._type, pos);
					}
				}
			}
		}
	}
	public static void QuickCreateChunkMesh(out Chunk chunk) {
		GameObject collider = null;
		if (collider == null)
			collider = new GameObject();
		collider.transform.position = _position;
		chunk = new Chunk(_position, _mesh, collider, _blocks);
	}
	public static void CreateChunkMesh(out Chunk chunk) {
		GameObject collider = null;
		_mesh.vertices = _vertices.ToArray();
		_mesh.uv = _uvs.ToArray();
		_mesh.triangles = _tris.ToArray();
		//_mesh.normals = normals;
		_mesh.RecalculateNormals();
		if (collider == null)
			collider = new GameObject();
		collider.transform.position = _position;
		//MeshCollider mc = new MeshCollider();
		collider.AddComponent<MeshCollider>();
		collider.GetComponent<MeshCollider>().sharedMesh = _mesh;

		chunk = new Chunk(_position, _mesh, collider, _blocks);
	}
	#region make cude code
	static public void MakeCubeMesh(BlockData[] nBlocks, BlockType type, Vector3Int pos)
	{
		if (!nBlocks[0]._on)
		{//left
			MakeLeftPlane( pos);
			AddUVSet( type, BlockSide.LEFT);
		}
		if (!nBlocks[1]._on)
		{//right
			MakeRightPlane(pos);
			AddUVSet( type, BlockSide.RIGHT);
		}
		if (!nBlocks[2]._on)
		{//forward
			MakeForwardPlane(pos);
			AddUVSet( type, BlockSide.FORWARD);
		}
		if (!nBlocks[3]._on)
		{//back
			MakeBackPlane(pos);
			AddUVSet( type, BlockSide.BACK);
		}
		if (!nBlocks[4]._on)
		{//top
			MakeTopPlane(pos);
			AddUVSet( type, BlockSide.TOP);
		}
		if (!nBlocks[5]._on)
		{//bottom
			MakeBotPlane(pos);
			AddUVSet( type, BlockSide.BOTTOM);
		}
	}
	static public void AddUVSet( BlockType type, BlockSide side)
	{
		float s = 1 / 6.0f ;
		s *= 1.01f;
		int x = (int)(side);
		int y = (int)(type);
		 float w = 0.95f;
		
		_uvs.AddRange(new Vector2[] {
			new Vector2(((x + w) * s), 1-((y + w) * s)),
			new Vector2(((x + w)) * s, 1-(y * s)),
			new Vector2((x * s), 1-((y + w) * s) ),
			new Vector2((x * s), 1-(y * s)),
		});
	}
	static public void MakeTopPlane( Vector3Int pos)
	{
		float s = 1;
		_vertices.Add(new Vector3(pos.x , pos.y + s, pos.z ));
		_vertices.Add(new Vector3(pos.x , pos.y + s, pos.z + s));
		_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z ));
		_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
		int start = _vertices.Count - 4;
		_tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	static public void MakeLeftPlane( Vector3Int pos)
	{
		//vert 0
		float s = 1;
		_vertices.Add(new Vector3(pos.x, pos.y, pos.z + s));
		_vertices.Add(new Vector3(pos.x , pos.y + s, pos.z + s));
		_vertices.Add(new Vector3(pos.x , pos.y , pos.z ));
		_vertices.Add(new Vector3(pos.x , pos.y + s, pos.z ));
		int start = _vertices.Count - 4;
		_tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	static public void MakeRightPlane( Vector3Int pos)
	{
		//vert 0
		float s = 1;
		_vertices.Add(new Vector3(pos.x + s, pos.y , pos.z ));
		_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z ));
		_vertices.Add(new Vector3(pos.x + s, pos.y , pos.z + s));
		_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
		int start = _vertices.Count - 4;
		_tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	static public void MakeBotPlane( Vector3Int pos)
	{
		//vert 0
		float s = 1;
		_vertices.Add(new Vector3(pos.x + s, pos.y , pos.z ));
		_vertices.Add(new Vector3(pos.x + s, pos.y , pos.z + s));
		_vertices.Add(new Vector3(pos.x , pos.y , pos.z ));
		_vertices.Add(new Vector3(pos.x , pos.y , pos.z + s));
		int start = _vertices.Count - 4;
		_tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	static public void MakeForwardPlane( Vector3Int pos)
	{
		//vert 0
		float s = 1;
		_vertices.Add(new Vector3(pos.x + s, pos.y , pos.z + s));
		_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
		_vertices.Add(new Vector3(pos.x , pos.y, pos.z + s));
		_vertices.Add(new Vector3(pos.x, pos.y + s, pos.z + s));
		int start = _vertices.Count - 4;
		_tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	static public void MakeBackPlane( Vector3Int pos)
	{
		//vert 0
		float s = 1;
		_vertices.Add(new Vector3(pos.x , pos.y , pos.z ));
		_vertices.Add(new Vector3(pos.x , pos.y + s, pos.z ));
		_vertices.Add(new Vector3(pos.x + s, pos.y , pos.z ));
		_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z ));
		int start = _vertices.Count - 4;
		_tris.AddRange(new int[]{
					0 + start,
					1 + start,
					2 + start,

					3 + start,
					2 + start,
					1 + start
				});

	}
	public static void MakePlaneTris(List<int> tris, int offset) {
		_tris.AddRange(new int[]{
					0 + offset,
					1 + offset,
					2 + offset,

					3 + offset,
					2 + offset,
					1 + offset
				});
	}
	/*static public void SetUvs( List<Vector2> uvs, Block b, int index)
	{
		uvs.AddRange(new Vector2[] {
			new Vector2(0.0f, 0.0f),
			new Vector2(1.0f, 0.0f),
			new Vector2(0.0f, 1.0f),
			new Vector2(1.0f, 1.0f)
		});
	}*/
	static public BlockData[] GetNeighbours(Vector3Int pos)
	{
		
		if(pos.x == 4)
			Debug.Log(pos);
		int i = 0;
		BlockData[] bData = new BlockData[6];
		Vector3Int nPos = pos + new Vector3Int(-1, 0, 0);//left
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = _blocks[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(1, 0, 0);//right
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = _blocks[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 0, 1);//forward
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = _blocks[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 0, -1);//back
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = _blocks[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 1, 0);//top
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = _blocks[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, -1, 0);//bot
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = _blocks[GetIndexFromPos(nPos)];


		//l r f b t b

		return bData;
	}
	static public bool CheckOutOfBounds(Vector3Int pos1)
	{
		//if you are checking out of bounds of the array return a
		return (pos1.x < 0 || pos1.x >= Chunk._size ||
			pos1.y < 0 || pos1.y >= Chunk._size||
			pos1.z < 0 || pos1.z >= Chunk._size);

	}
	static public int GetIndexFromPos(Vector3Int pos)
	{
		return (int)(pos.z + pos.y * Chunk._size + Chunk._size * Chunk._size * pos.x);

	}
	static public BlockData GetBlockFromPos(Vector3Int pos)
	{
		int index = (int)(pos.z + pos.y * Chunk._size + Chunk._size * Chunk._size * pos.x);
		return _blocks[index];
	}
	#endregion
	public static void CreateCube(Vector3Int pos, BlockType type, Mesh mesh, List<BlockData> blocks)
	{
		/*List<Vector3> _vertices = new List<Vector3>(mesh.vertices);
		List<Vector2> uvs = new List<Vector2>(mesh.uv);
		List<int> tris = new List<int>(mesh.triangles);

		BlockData bd = blocks[GetIndexFromPos(pos)];
		bd._on = true;
		bd._type = type;
		blocks[GetIndexFromPos(pos)] = bd;

		ChunkGenerator.MakeCubeMesh(ChunkGenerator.GetNeighbours(pos, blocks), bd._type,
			pos, _vertices,  tris,  uvs);
		mesh.vertices = _vertices.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		//_mesh.normals = normals;
		mesh.RecalculateNormals();*/
	}
	public static void DeleteCube(Vector3Int pos,  Mesh mesh, List<BlockData> blocks)
	{

		/*List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();


		BlockData bd = blocks[GetIndexFromPos(pos)];
		bd._on = false;
		blocks[GetIndexFromPos(pos)] = bd;

		for (int x = 0; x < Chunk._size; x++)
		{
			for (int y = 0; y < Chunk._size; y++)
			{
				for (int z = 0; z < Chunk._size; z++)
				{
					pos.x = x; pos.y = y; pos.z = z;
					bd = GetBlockFromPos(pos, blocks);
					if (bd._on)
					{
						//get neighbours 
						BlockData[] nBlocks = GetNeighbours(pos, blocks);
						MakeCubeMesh(nBlocks, bd._type, pos,  vertices,  tris,  uvs);

					}
				}
			}
		}
		mesh.Clear();
		mesh.vertices = vertices.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		//_mesh.normals = normals;
		mesh.RecalculateNormals();

		//collider.GetComponent<MeshCollider>().sharedMesh = mesh;

		//chunk = new Chunk(position, mesh, collider, blocks, boolMap);*/
	}
	public static BlockSide GetBlockSide(Vector3 normal) {
		BlockSide bs = BlockSide.TOP;

		if (normal == Vector3.up) bs = BlockSide.TOP;
		else if (normal == Vector3.down) bs = BlockSide.BOTTOM;
		else if (normal == Vector3.right) bs = BlockSide.RIGHT;
		else if (normal == Vector3.left) bs = BlockSide.LEFT;
		else if (normal == Vector3.forward) bs = BlockSide.FORWARD;
		else if (normal == Vector3.back) bs = BlockSide.BACK;

		return bs;
	}
	public static float GetToughnessFromType(BlockType type) {
		//time it takes to break the block IN SECONMDS
		float toughness = 0;
		if (type == BlockType.DIRT || type == BlockType.DIRT_GRASS) toughness = 0.01f;
		else if (type == BlockType.FROZEN_DIRT || type == BlockType.FROZEN_ICE_DIRT) toughness = 0.01f;
		else if (type == BlockType.TREE_WOOD) toughness = 0.01f;
		else if (type == BlockType.ROCK) toughness = 0.01f;
		else toughness = 1.15f;
		return toughness;
	}
}
public class Pair<T, U> {
	public T one;
	public U two;
	public Pair(T o, U w) {
		one = o;
		two = w;
	}
	public Pair()
	{
	}
}