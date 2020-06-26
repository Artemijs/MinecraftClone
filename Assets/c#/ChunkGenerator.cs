using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
					boolMap[index] = (y == 2 && x != 1);
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
					if (GetBoolFromPos(pos, ref boolMap))
					{
						//get neighbours 
						bool[] bools = GetNeighbours(pos, ref boolMap);
						MakeCubeMesh(bools, pos, ref vertices, ref tris, ref uvs);
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

		chunk = new Chunk(position, mesh, collider);
	}
	void MakeCubeMesh(bool[] nbools, Vector3Int pos, ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs)
	{
		if (!nbools[0])
		{//left
			MakeLeftPlane(ref verts, ref tris, pos);
			AddUVSet(ref uvs);
		}
		if (!nbools[1])
		{//right
			MakeRightPlane(ref verts, ref tris, pos);
			AddUVSet(ref uvs);
		}
		if (!nbools[2])
		{//forward
			MakeForwardPlane(ref verts, ref tris, pos);
			AddUVSet(ref uvs);
		}
		if (!nbools[3])
		{//back
			MakeBackPlane(ref verts, ref tris, pos);
			AddUVSet(ref uvs);
		}
		if (!nbools[4])
		{//top
			MakeTopPlane(ref verts, ref tris, pos);
			AddUVSet(ref uvs);
		}
		if (!nbools[5])
		{//bottom
			MakeBotPlane(ref verts, ref tris, pos);
			AddUVSet(ref uvs);
		}
	}
	void AddUVSet(ref List<Vector2> uvs)
	{
		uvs.AddRange(new Vector2[] {
			new Vector2(0.0f, 0.0f),
			new Vector2(1.0f, 0.0f),
			new Vector2(0.0f, 1.0f),
			new Vector2(1.0f, 1.0f)
		});
	}
	void MakeTopPlane(ref List<Vector3> verts, ref List<int> tris, Vector3Int pos)
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
	void MakeLeftPlane(ref List<Vector3> verts, ref List<int> tris, Vector3Int pos)
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
	void MakeRightPlane(ref List<Vector3> verts, ref List<int> tris, Vector3Int pos)
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
	void MakeBotPlane(ref List<Vector3> verts, ref List<int> tris, Vector3Int pos)
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
	void MakeForwardPlane(ref List<Vector3> verts, ref List<int> tris, Vector3Int pos)
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
	void MakeBackPlane(ref List<Vector3> verts, ref List<int> tris, Vector3Int pos)
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
	public void SetUvs(ref List<Vector2> uvs, Block b, int index)
	{
		uvs.AddRange(new Vector2[] {
			new Vector2(0.0f, 0.0f),
			new Vector2(1.0f, 0.0f),
			new Vector2(0.0f, 1.0f),
			new Vector2(1.0f, 1.0f)
		});
	}
	bool[] GetNeighbours(Vector3Int pos, ref bool[] allbools)
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
	bool CheckOutOfBounds(Vector3Int pos1)
	{
		//if you are checking out of bounds of the array return a
		return (pos1.x < 0 || pos1.x >= Chunk._size.x ||
			pos1.y < 0 || pos1.y >= Chunk._size.y ||
			pos1.z < 0 || pos1.z >= Chunk._size.z);

	}
	public int GetIndexFromPos(Vector3Int pos)
	{
		return(int)(pos.z + pos.y * Chunk._size.y + Chunk._size.x * Chunk._size.x * pos.x);
		
	}
	public bool GetBoolFromPos(Vector3Int pos, ref bool[] boolMap)
	{
		int index = (int)(pos.z + pos.y * Chunk._size.y + Chunk._size.x * Chunk._size.x * pos.x);
		return boolMap[index];
	}
}
