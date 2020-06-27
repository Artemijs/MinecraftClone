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
		List<Vector3> normals = new List<Vector3>();
		BlockFactory bFactory = BlockFactory.Instance;
		Vector3 bSize = Block._size;
		List<BlockData> blocks = new List<BlockData>();
		Block b;
		bool[] boolMap = new bool[nrOfCubes];
		int index = 0;
		int nrOfVisibleBlocks = 0;
		//create a bool map
		for (int x = 0; x < Chunk._size.x; x++)
		{
			for (int y = 0; y < Chunk._size.y; y++)
			{
				for (int z = 0; z < Chunk._size.z; z++)
				{
					boolMap[index] = (y == 2 );
					//boolMap[index] = true;
					index++;
				}
			}
		}
		Vector3Int pos = new Vector3Int();
		index = 0;
		BlockData bd;
		for (int x = 0; x < Chunk._size.x; x++)
		{
			for (int y = 0; y < Chunk._size.y; y++)
			{
				for (int z = 0; z < Chunk._size.z; z++)
				{
					pos.x = x; pos.y = y; pos.z = z;
					if (GetBoolFromPos(pos, ref boolMap))
					{
						//get neighbours 
						bool[] bools = GetNeighbours(pos, ref boolMap);
						int vertCount = vertices.Count;
						MakeCubeMesh(bools, pos, ref vertices, ref tris, ref uvs);
						vertCount = vertices.Count - vertCount;
						bd._faceCount = vertCount / 4;
						bd._on = true;
						blocks.Add(bd);
						index++;
					}
					else {
						bd._faceCount = 0;
						bd._on = false;
						blocks.Add(bd);
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

		chunk = new Chunk(position, mesh, collider, blocks, boolMap);
		
	}
	#region make cude code
	static public void MakeCubeMesh(bool[] nbools, Vector3Int pos, ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs)
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
	static public bool[] GetNeighbours(Vector3Int pos, ref bool[] allbools)
	{
		int i = 0;
		bool[] bools = new bool[6];
		Vector3Int nPos = pos + new Vector3Int(-1, 0, 0);//left
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(1, 0, 0);//right
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 0, 1);//forward
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 0, -1);//back
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, 1, 0);//top
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)];
		i++;
		nPos = pos + new Vector3Int(0, -1, 0);//bot
		if (CheckOutOfBounds(nPos)) bools[i] = false;
		else bools[i] = allbools[GetIndexFromPos(nPos)];


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
	static public bool GetBoolFromPos(Vector3Int pos, ref bool[] boolMap)
	{
		int index = (int)(pos.z + pos.y * Chunk._size.y + Chunk._size.x * Chunk._size.x * pos.x);
		return boolMap[index];
	}
	#endregion
	public static void CreateCube(Vector3Int pos, ref Mesh mesh, ref bool[] boolMap)
	{
		List<Vector3> verts = new List<Vector3>(mesh.vertices);
		List<Vector2> uvs = new List<Vector2>(mesh.uv);
		List<int> tris = new List<int>(mesh.triangles);
		boolMap[GetIndexFromPos(pos)] = true;

		ChunkGenerator.MakeCubeMesh(ChunkGenerator.GetNeighbours(pos, ref boolMap), pos,
			ref verts, ref tris, ref uvs);
		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		//_mesh.normals = normals;
		mesh.RecalculateNormals();
	}
	public static void DeleteCube(Vector3Int pos, ref Mesh mesh, ref bool[] boolMap)
	{
		List<Vector3> verts = new List<Vector3>(mesh.vertices);
		List<Vector2> uvs = new List<Vector2>(mesh.uv);
		List<int> tris = new List<int>(mesh.triangles);
		boolMap[GetIndexFromPos(pos)] = false;

		//ChunkGenerator.MakeCubeMesh(ChunkGenerator.GetNeighbours(pos, ref boolMap), pos,
		//	ref verts, ref tris, ref uvs);
		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		//_mesh.normals = normals;
		mesh.RecalculateNormals();
	}
	static public void Temp(Vector3Int pos, ref Mesh mesh, ref bool[] boolMap, ref List<BlockData> blocks) {
		//pos = new Vector3Int(1, 1, 1);
		pos.y -= 1;
		List<Action< List<Vector3>,  List<int>, Vector3Int>> _planeFunctions = new List<Action<List<Vector3>, List<int>, Vector3Int>>();
		_planeFunctions.Add(MakeLeftPlane);
		_planeFunctions.Add(MakeRightPlane);
		_planeFunctions.Add(MakeForwardPlane);
		_planeFunctions.Add(MakeBackPlane);
		_planeFunctions.Add(MakeTopPlane);
		_planeFunctions.Add(MakeBotPlane);
		Vector3Int[] nPoses = new Vector3Int[] {
			new Vector3Int(pos.x+1, pos.y, pos.z),
			new Vector3Int(pos.x-1, pos.y, pos.z),
			new Vector3Int(pos.x, pos.y, pos.z-1),
			new Vector3Int(pos.x, pos.y, pos.z+1),
			new Vector3Int(pos.x, pos.y-1, pos.z),
			//new Vector3Int(pos.x, pos.y-1, pos.z),
		};

		List<Vector3> verts = new List<Vector3>(mesh.vertices);
		List<Vector2> uvs = new List<Vector2>(mesh.uv);
		List<int> tris = new List<int>(mesh.triangles);
		int center = GetIndexFromPos(pos);
		boolMap[center] = false;

		List<int> nIdeces;
		//lrfbtb
		//let the center be the last thing in the array :D
		GetNeighborIndeces(pos, out nIdeces, blocks);
		int sidesFound = 0;
		for (int i = 0; i < verts.Count; i++) {
			//find match
			//find start
			//find end
			//break once all matches found
			
			for (int j = 0; j < nIdeces.Count; j++) {
				if (i == nIdeces[j]) {
					if (j == 6) {
						verts.RemoveRange(i, (blocks[center]._faceCount * 4));
						tris.RemoveRange(i / 4 * 6, (blocks[center]._faceCount * 6));
						uvs.RemoveRange(i, (blocks[center]._faceCount * 4));
						break;
					}
					if (nIdeces[j] < 0) continue;
					//found match
					sidesFound++;
					/*List<Vector3> newVerts = new List<Vector3>();
					List<Vector2> newUVS = new List<Vector2>();
					List<int> newTris = new List<int>();

					_planeFunctions[j](newVerts, newTris, nPoses[j]);
					newTris = new List<int>();
					MakePlaneTris(newTris, verts.Count);
					AddUVSet(newUVS);
					verts.AddRange(newVerts);
					uvs.AddRange(newUVS);
					tris.AddRange(newTris);*/
					
				}
			}
			//if (sidesFound == 5) break;

		}
		//remove existing vertices of the block
		
		//find what to insert
		//nothing to insert for the removed block
		//remove left
		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		//_mesh.normals = normals;
		mesh.RecalculateNormals();

	}

	

	public static void GetNeighborIndeces(Vector3Int pos, out List<int> indeces, List<BlockData> blocks) {
		indeces = new List<int>();
		//lrfbtb
		Vector3Int tPos = pos;
		//get left index
		tPos.x -= 1;
		indeces.Add(GetVertexIndex(tPos, blocks));
		//get right index
		tPos.x += 1;
		indeces.Add(GetVertexIndex(tPos, blocks));
		//get forward index
		tPos.z += 1;
		indeces.Add(GetVertexIndex(tPos, blocks));
		//get back index
		tPos.z -= 1;
		indeces.Add(GetVertexIndex(tPos, blocks));
		//get top index
		tPos.y += 1;
		indeces.Add(GetVertexIndex(tPos, blocks));
		//get bot index
		tPos.y -= 1;
		indeces.Add(-1);
		//center
		indeces.Add(GetVertexIndex(pos, blocks));

	}
	public static int GetVertexIndex(Vector3Int pos, List<BlockData> blocks) {
		int total = 0;
		for (int x = 0; x < Chunk._size.x; x++)
		{
			for (int y = 0; y < Chunk._size.y; y++)
			{
				for (int z = 0; z < Chunk._size.z; z++)
				{
					if (x == pos.x && y == pos.y && z == pos.z)
						break;
					int asdkjhg = GetIndexFromPos(new Vector3Int(x, y, z));
					if (asdkjhg >= 20)
					{
						int asdgfajshfg = 0;
						GetIndexFromPos(new Vector3Int(x, y, z));
					}
					total += blocks[asdkjhg]._faceCount;

				}
			}
		}
		return total*4;
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