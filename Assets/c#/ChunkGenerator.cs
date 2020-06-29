using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this file assumes that every position provided is between 0 and Chunk._size
/// </summary>
public class ChunkGenerator : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	public void MakeChunk(out Chunk chunk, Vector3 position) {


		GameObject collider = null;
		int nrOfCubes = (int)(Chunk._size * Chunk._size * Chunk._size);
		Mesh mesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();


		List<BlockData> blocks = new List<BlockData>();
		BlockData bd;
		int index = 0;

		//create a bool map
		for (int x = 0; x < Chunk._size; x++)
		{
			for (int y = 0; y < Chunk._size; y++)
			{
				for (int z = 0; z < Chunk._size; z++)
				{
					bd._on = (y + position.y<= 2);
					if (!bd._on) bd._type = BlockType.AIR;
					else {
						bd._type = (BlockType)(Random.Range(0, 5));
					}
					//bd._on = (y == 0 && x == 0 && z == 0);
					blocks.Add(bd);
					index++;
				}
			}
		}
		Vector3Int pos = new Vector3Int();
		index = 0;
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
						index++;
					}
				}
			}
		}
		mesh.vertices = vertices.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		//_mesh.normals = normals;
		mesh.RecalculateNormals();
		if (collider == null)
			collider = new GameObject();
		collider.transform.position = position;
		collider.AddComponent<MeshCollider>();
		collider.GetComponent<MeshCollider>().sharedMesh = mesh;

		chunk = new Chunk(position, mesh, collider, blocks);
		
	}
	#region make cude code
	static public void MakeCubeMesh(BlockData[] nBlocks, BlockType type, Vector3Int pos,  List<Vector3> verts,  List<int> tris,  List<Vector2> uvs)
	{
		if (!nBlocks[0]._on)
		{//left
			MakeLeftPlane( verts,  tris, pos);
			AddUVSet( uvs, type, BlockSide.LEFT);
		}
		if (!nBlocks[1]._on)
		{//right
			MakeRightPlane( verts,  tris, pos);
			AddUVSet( uvs, type, BlockSide.RIGHT);
		}
		if (!nBlocks[2]._on)
		{//forward
			MakeForwardPlane( verts,  tris, pos);
			AddUVSet( uvs, type, BlockSide.FORWARD);
		}
		if (!nBlocks[3]._on)
		{//back
			MakeBackPlane( verts,  tris, pos);
			AddUVSet( uvs, type, BlockSide.BACK);
		}
		if (!nBlocks[4]._on)
		{//top
			MakeTopPlane( verts,  tris, pos);
			AddUVSet( uvs, type, BlockSide.TOP);
		}
		if (!nBlocks[5]._on)
		{//bottom
			MakeBotPlane( verts,  tris, pos);
			AddUVSet( uvs, type, BlockSide.BOTTOM);
		}
	}
	static public void AddUVSet( List<Vector2> uvs, BlockType type, BlockSide side)
	{
		float s = 1 / 6.0f ;
		s *= 1.01f;
		int x = (int)(side);
		int y = (int)(type);
		 float w = 0.95f;
		
		uvs.AddRange(new Vector2[] {
			new Vector2(((x + w) * s), 1-((y + w) * s)),
			new Vector2(((x + w)) * s, 1-(y * s)),
			new Vector2((x * s), 1-((y + w) * s) ),
			new Vector2((x * s), 1-(y * s)),
		});
	}
	static public void MakeTopPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
	{
		float s = 1;
		verts.Add(new Vector3(pos.x , pos.y + s, pos.z ));
		verts.Add(new Vector3(pos.x , pos.y + s, pos.z + s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z ));
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
	static public void MakeLeftPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
	{
		//vert 0
		float s = 1;
		verts.Add(new Vector3(pos.x, pos.y, pos.z + s));
		verts.Add(new Vector3(pos.x , pos.y + s, pos.z + s));
		verts.Add(new Vector3(pos.x , pos.y , pos.z ));
		verts.Add(new Vector3(pos.x , pos.y + s, pos.z ));
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
	static public void MakeRightPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
	{
		//vert 0
		float s = 1;
		verts.Add(new Vector3(pos.x + s, pos.y , pos.z ));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z ));
		verts.Add(new Vector3(pos.x + s, pos.y , pos.z + s));
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
	static public void MakeBotPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
	{
		//vert 0
		float s = 1;
		verts.Add(new Vector3(pos.x + s, pos.y , pos.z ));
		verts.Add(new Vector3(pos.x + s, pos.y , pos.z + s));
		verts.Add(new Vector3(pos.x , pos.y , pos.z ));
		verts.Add(new Vector3(pos.x , pos.y , pos.z + s));
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
	static public void MakeForwardPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
	{
		//vert 0
		float s = 1;
		verts.Add(new Vector3(pos.x + s, pos.y , pos.z + s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
		verts.Add(new Vector3(pos.x , pos.y, pos.z + s));
		verts.Add(new Vector3(pos.x, pos.y + s, pos.z + s));
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
	static public void MakeBackPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
	{
		//vert 0
		float s = 1;
		verts.Add(new Vector3(pos.x , pos.y , pos.z ));
		verts.Add(new Vector3(pos.x , pos.y + s, pos.z ));
		verts.Add(new Vector3(pos.x + s, pos.y , pos.z ));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z ));
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
	public static void MakePlaneTris(List<int> tris, int offset) {
		tris.AddRange(new int[]{
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
	static public BlockData[] GetNeighbours(Vector3Int pos, List<BlockData> allbools)
	{
		
		if(pos.x == 4)
			Debug.Log(pos);
		int i = 0;
		BlockData[] bData = new BlockData[6];
		Vector3Int nPos = pos + new Vector3Int(-1, 0, 0);//left
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(1, 0, 0);//right
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 0, 1);//forward
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 0, -1);//back
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 1, 0);//top
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, -1, 0);//bot
		if (CheckOutOfBounds(nPos)) bData[i]._on = false;
		else bData[i] = allbools[GetIndexFromPos(nPos)];


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
	static public BlockData GetBlockFromPos(Vector3Int pos, List<BlockData> boolMap)
	{
		int index = (int)(pos.z + pos.y * Chunk._size + Chunk._size * Chunk._size * pos.x);
		return boolMap[index];
	}
	#endregion
	public static void CreateCube(Vector3Int pos, BlockType type, Mesh mesh, List<BlockData> blocks)
	{
		List<Vector3> verts = new List<Vector3>(mesh.vertices);
		List<Vector2> uvs = new List<Vector2>(mesh.uv);
		List<int> tris = new List<int>(mesh.triangles);

		BlockData bd = blocks[GetIndexFromPos(pos)];
		bd._on = true;
		bd._type = type;
		blocks[GetIndexFromPos(pos)] = bd;

		ChunkGenerator.MakeCubeMesh(ChunkGenerator.GetNeighbours(pos, blocks), bd._type,
			pos, verts,  tris,  uvs);
		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		//_mesh.normals = normals;
		mesh.RecalculateNormals();
	}
	public static void DeleteCube(Vector3Int pos,  Mesh mesh, List<BlockData> blocks)
	{

		List<Vector3> vertices = new List<Vector3>();
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

		//chunk = new Chunk(position, mesh, collider, blocks, boolMap);
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
		if (type == BlockType.DIRT || type == BlockType.DIRT_GRASS) toughness = 1.25f;
		else if (type == BlockType.FROZEN_DIRT || type == BlockType.FROZEN_ICE_DIRT) toughness = 1.25f;
		else if (type == BlockType.TREE_WOOD) toughness = 1.25f;
		else if (type == BlockType.ROCK) toughness = 1.75f;
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