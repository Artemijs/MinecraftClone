using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ChunkGenerator : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	public void MakeChunk(out Chunk chunk, Vector3 position) {


		GameObject collider = null;
		int nrOfCubes = (int)(Chunk._size.x * Chunk._size.y * Chunk._size.z);
		Mesh mesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();


		List<BlockData> blocks = new List<BlockData>();
		BlockData bd;
		int index = 0;

		//create a bool map
		for (int x = 0; x < Chunk._size.x; x++)
		{
			for (int y = 0; y < Chunk._size.y; y++)
			{
				for (int z = 0; z < Chunk._size.z; z++)
				{
					bd._on = (y == 2);
					blocks.Add(bd);
					//boolMap[index] = true;
					index++;
				}
			}
		}
		Vector3Int pos = new Vector3Int();
		index = 0;
		for (int x = 0; x < Chunk._size.x; x++)
		{
			for (int y = 0; y < Chunk._size.y; y++)
			{
				for (int z = 0; z < Chunk._size.z; z++)
				{
					pos.x = x; pos.y = y; pos.z = z;
					if (GetBlockFromPos(pos,  blocks)._on)
					{
						//get neighbours 
						bool[] bools = GetNeighbours(pos, blocks);
						MakeCubeMesh(bools, pos,  vertices,  tris,  uvs);
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
	static public void MakeCubeMesh(bool[] nbools, Vector3Int pos,  List<Vector3> verts,  List<int> tris,  List<Vector2> uvs)
	{
		if (!nbools[0])
		{//left
			MakeLeftPlane( verts,  tris, pos);
			AddUVSet( uvs);
		}
		if (!nbools[1])
		{//right
			MakeRightPlane( verts,  tris, pos);
			AddUVSet( uvs);
		}
		if (!nbools[2])
		{//forward
			MakeForwardPlane( verts,  tris, pos);
			AddUVSet( uvs);
		}
		if (!nbools[3])
		{//back
			MakeBackPlane( verts,  tris, pos);
			AddUVSet( uvs);
		}
		if (!nbools[4])
		{//top
			MakeTopPlane( verts,  tris, pos);
			AddUVSet( uvs);
		}
		if (!nbools[5])
		{//bottom
			MakeBotPlane( verts,  tris, pos);
			AddUVSet( uvs);
		}
	}
	static public void AddUVSet( List<Vector2> uvs)
	{
		uvs.AddRange(new Vector2[] {
			new Vector2(0.0f, 0.0f),
			new Vector2(1.0f, 0.0f),
			new Vector2(0.0f, 1.0f),
			new Vector2(1.0f, 1.0f)
		});
	}
	static public void MakeTopPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
	{
		float s = 0.5f;
		verts.Add(new Vector3(pos.x - s, pos.y + s, pos.z - s));
		verts.Add(new Vector3(pos.x - s, pos.y + s, pos.z + s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z - s));
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
	static public void MakeRightPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
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
	static public void MakeBotPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
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
	static public void MakeForwardPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
	{
		//vert 0
		float s = 0.5f;
		verts.Add(new Vector3(pos.x + s, pos.y - s, pos.z + s));
		verts.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
		verts.Add(new Vector3(pos.x - s, pos.y - s, pos.z + s));
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
	static public void MakeBackPlane( List<Vector3> verts,  List<int> tris, Vector3Int pos)
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
	static public void SetUvs( List<Vector2> uvs, Block b, int index)
	{
		uvs.AddRange(new Vector2[] {
			new Vector2(0.0f, 0.0f),
			new Vector2(1.0f, 0.0f),
			new Vector2(0.0f, 1.0f),
			new Vector2(1.0f, 1.0f)
		});
	}
	static public bool[] GetNeighbours(Vector3Int pos, List<BlockData> allbools)
	{
		int i = 0;
		bool[] bools = new bool[6];
		Vector3Int nPos = pos + new Vector3Int(-1, 0, 0);//left
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)]._on;
		i++;
		nPos = pos + new Vector3Int(1, 0, 0);//right
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)]._on;
		i++;
		nPos = pos + new Vector3Int(0, 0, 1);//forward
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)]._on;
		i++;
		nPos = pos + new Vector3Int(0, 0, -1);//back
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)]._on;
		i++;
		nPos = pos + new Vector3Int(0, 1, 0);//top
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)]._on;
		i++;
		nPos = pos + new Vector3Int(0, -1, 0);//bot
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)]._on;


		//l r f b t b

		return bools;
	}
	static public bool CheckOutOfBounds(Vector3Int pos1)
	{
		//if you are checking out of bounds of the array return a
		return (pos1.x < 0 || pos1.x >= Chunk._size.x ||
			pos1.y < 0 || pos1.y >= Chunk._size.y ||
			pos1.z < 0 || pos1.z >= Chunk._size.z);

	}
	static public int GetIndexFromPos(Vector3Int pos)
	{
		return (int)(pos.z + pos.y * Chunk._size.y + Chunk._size.x * Chunk._size.x * pos.x);

	}
	static public BlockData GetBlockFromPos(Vector3Int pos, List<BlockData> boolMap)
	{
		int index = (int)(pos.z + pos.y * Chunk._size.y + Chunk._size.x * Chunk._size.x * pos.x);
		return boolMap[index];
	}
	#endregion
	public static void CreateCube(Vector3Int pos, Mesh mesh, List<BlockData> blocks)
	{
		List<Vector3> verts = new List<Vector3>(mesh.vertices);
		List<Vector2> uvs = new List<Vector2>(mesh.uv);
		List<int> tris = new List<int>(mesh.triangles);

		BlockData bd = blocks[GetIndexFromPos(pos)];
		bd._on = true;
		blocks[GetIndexFromPos(pos)] = bd;

		ChunkGenerator.MakeCubeMesh(ChunkGenerator.GetNeighbours(pos, blocks), pos,
			 verts,  tris,  uvs);
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

		for (int x = 0; x < Chunk._size.x; x++)
		{
			for (int y = 0; y < Chunk._size.y; y++)
			{
				for (int z = 0; z < Chunk._size.z; z++)
				{
					pos.x = x; pos.y = y; pos.z = z;
					if (GetBlockFromPos(pos, blocks)._on)
					{
						//get neighbours 
						bool[] bools = GetNeighbours(pos, blocks);
						MakeCubeMesh(bools, pos,  vertices,  tris,  uvs);

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
	
}
public class Pair<T, U> {
	public T one;
	public U two;
	public Pair(T o, U w) {
		one = o;
		two = w;
	}
}