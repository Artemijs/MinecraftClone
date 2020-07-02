using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Version3_1 {
	public class MeshGenerator {
		static Sector _currentSector;
		static List<Vector3> _vertices;
		static List<Vector2> _uvs;
		static List<int> _tris;
		static List<BlockData> _blocks;
		static Vector3 _position;
		static int _nrOfCubes;

		public MeshGenerator() {
		}
		public static void Init() {
			_vertices = new List<Vector3>(); ;
			_uvs = new List<Vector2>();
			_tris = new List<int>();
			_blocks = new List<BlockData>();
			_position = new Vector3();
		}
		public static void GenerateMesh(Sector s) {
			_currentSector = s;
			/*for (int i = 0; i < Sector._suSize; i+=Chunk._uSize) {
				for (int j = 0; j < Sector._suSize; j += Chunk._uSize) {
					for (int k = 0; k < Sector._suSize; k += Chunk._uSize) {
						Vector3Int chunkPos = new Vector3Int(
							//this goes through every block in chunk
						for (int bd_i = 0; bd_i < Sector._sSize; bd_i++) {
							for (int bd_j = 0; bd_j < Sector._sSize; bd_j++) {
								for (int bd_k = 0; bd_k < Sector._sSize; bd_k++) {
									BlockData bd = s.BlockData[bd_i, bd_j, bd_k];
								}
							}
						}
					}
				}
			}*/
			//ijk 000 == 000 
			//ijk 001 == 005
			//ijk*sSize = start ijk for block data
			//ijk+(111)*size = end for block data

			//this goes through every chunk in sector
			for (int i = 0; i < Sector._sSize; i++) {
				for (int j = 0; j < Sector._sSize; j++) {
					for (int k = 0; k < Sector._sSize; k++) {
						Chunk c = s.Chunks[i, j, k];
						RemakeChunk(c, new Vector3Int(i * Chunk._uSize, j * Chunk._uSize, k * Chunk._uSize));
					}
				}
			}
		}
		
		public static void RemakeChunk(Chunk chunk, Vector3Int position) {
			_position = position;

			CreateChunkMeshData(position, chunk);
			//chunk.Position = StaticFunctions.Vector3F2Int(_position);
			chunk.SolidMesh.vertices = _vertices.ToArray();
			chunk.SolidMesh.uv = _uvs.ToArray();
			chunk.SolidMesh.triangles = _tris.ToArray();
			chunk.SolidMesh.RecalculateNormals();
			//chunk.RecalculateCollider();

			ResetCache();

		}
		static void ResetCache() {
			_vertices.Clear();
			_uvs.Clear();
			_tris.Clear();
			_blocks = new List<BlockData>();
			_position = new Vector3();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataStart">where in the block data this chunks data starts</param>
		public static void CreateChunkMeshData(Vector3Int dataStart, Chunk chunk) {
			BlockData bd;
			Vector3Int pos = new Vector3Int();
			for (int i = 0; i < Chunk._size; i++) {
				for (int j = 0; j < Chunk._size; j++) {
					for (int k = 0; k < Chunk._size; k++) {
						pos.x = i + dataStart.x; pos.y = j + dataStart.y; pos.z = k + dataStart.z;
						bd = _currentSector.BlockData[i + dataStart.x, j + dataStart.y, k + dataStart.z];
						if (bd.Type != BlockType.AIR) {
							//get neighbours 
							BlockData[] nBlocks = GetNeighbours(pos);
							MakeCubeMesh(nBlocks, bd.Type, new Vector3Int(i,j,k));
						}
					}
				}
			}
		}
		
		public static void CreateChunkMesh(Chunk chunk) {
			//chunk = new Chunk(_position, _blocks);
			chunk.SolidMesh.vertices = _vertices.ToArray();
			chunk.SolidMesh.uv = _uvs.ToArray();
			chunk.SolidMesh.triangles = _tris.ToArray();
			chunk.SolidMesh.RecalculateNormals();
		//	chunk.RecalculateCollider();
		}
		#region make cude code
		static public void MakeCubeMesh(BlockData[] nBlocks, BlockType type, Vector3Int pos) {
			if (nBlocks[0].Type == BlockType.AIR) {//left
				MakeLeftPlane(pos);
				AddUVSet(type, BlockSide.LEFT);
			}
			if (nBlocks[1].Type == BlockType.AIR) {//right
				MakeRightPlane(pos);
				AddUVSet(type, BlockSide.RIGHT);
			}
			if (nBlocks[2].Type == BlockType.AIR) {//forward
				MakeForwardPlane(pos);
				AddUVSet(type, BlockSide.FORWARD);
			}
			if (nBlocks[3].Type == BlockType.AIR) {//back
				MakeBackPlane(pos);
				AddUVSet(type, BlockSide.BACK);
			}
			if (nBlocks[4].Type == BlockType.AIR) {//top
				MakeTopPlane(pos);
				AddUVSet(type, BlockSide.TOP);
			}
			if (nBlocks[5].Type == BlockType.AIR) {//bottom
				MakeBotPlane(pos);
				AddUVSet(type, BlockSide.BOTTOM);
			}
		}
		static public void AddUVSet(BlockType type, BlockSide side) {
			float sx = 1 / 6.0f;
			float sy = 1 / 8.0f;
			sx *= 1.01f;
			sy *= 1.01f;
			int x = (int)(side);
			int y = (int)(type);
			float w = 0.95f;

			_uvs.AddRange(new Vector2[] {
			new Vector2(((x + w) * sx), 1-((y + w) * sy)),
			new Vector2(((x + w)) * sx, 1-(y * sy)),
			new Vector2((x * sx), 1-((y + w) * sy) ),
			new Vector2((x * sx), 1-(y * sy)),
		});
		}
		static public void MakeTopPlane(Vector3Int pos) {
			float s = 1;
			_vertices.Add(new Vector3(pos.x, pos.y + s, pos.z));
			_vertices.Add(new Vector3(pos.x, pos.y + s, pos.z + s));
			_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z));
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
		static public void MakeLeftPlane(Vector3Int pos) {
			//vert 0
			float s = 1;
			_vertices.Add(new Vector3(pos.x, pos.y, pos.z + s));
			_vertices.Add(new Vector3(pos.x, pos.y + s, pos.z + s));
			_vertices.Add(new Vector3(pos.x, pos.y, pos.z));
			_vertices.Add(new Vector3(pos.x, pos.y + s, pos.z));
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
		static public void MakeRightPlane(Vector3Int pos) {
			//vert 0
			float s = 1;
			_vertices.Add(new Vector3(pos.x + s, pos.y, pos.z));
			_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z));
			_vertices.Add(new Vector3(pos.x + s, pos.y, pos.z + s));
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
		static public void MakeBotPlane(Vector3Int pos) {
			//vert 0
			float s = 1;
			_vertices.Add(new Vector3(pos.x + s, pos.y, pos.z));
			_vertices.Add(new Vector3(pos.x + s, pos.y, pos.z + s));
			_vertices.Add(new Vector3(pos.x, pos.y, pos.z));
			_vertices.Add(new Vector3(pos.x, pos.y, pos.z + s));
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
		static public void MakeForwardPlane(Vector3Int pos) {
			//vert 0
			float s = 1;
			_vertices.Add(new Vector3(pos.x + s, pos.y, pos.z + s));
			_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z + s));
			_vertices.Add(new Vector3(pos.x, pos.y, pos.z + s));
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
		static public void MakeBackPlane(Vector3Int pos) {
			//vert 0
			float s = 1;
			_vertices.Add(new Vector3(pos.x, pos.y, pos.z));
			_vertices.Add(new Vector3(pos.x, pos.y + s, pos.z));
			_vertices.Add(new Vector3(pos.x + s, pos.y, pos.z));
			_vertices.Add(new Vector3(pos.x + s, pos.y + s, pos.z));
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
		static public BlockData[] GetNeighbours(Vector3Int pos) {

			int i = 0;
			BlockData[] bData = new BlockData[6];
			Vector3Int nPos = pos + new Vector3Int(-1, 0, 0);//left
			if (CheckOutOfBounds(nPos)) {
				bData[i] = new BlockData(Vector3Int.zero, BlockType.AIR);

			}
			else {
				bData[i] = _currentSector.BlockData[nPos.x, nPos.y, nPos.z];
			}
			i++;
			nPos = pos + new Vector3Int(1, 0, 0);//right
			if (CheckOutOfBounds(nPos)) {
				bData[i] = new BlockData(Vector3Int.zero, BlockType.AIR);

			}
			else {
				bData[i] = _currentSector.BlockData[nPos.x, nPos.y, nPos.z];
			}
			i++;
			nPos = pos + new Vector3Int(0, 0, 1);//forward
			if (CheckOutOfBounds(nPos)) {
				bData[i] = new BlockData(Vector3Int.zero, BlockType.AIR);

			}
			else {
				bData[i] = _currentSector.BlockData[nPos.x, nPos.y, nPos.z];
			}
			i++;
			nPos = pos + new Vector3Int(0, 0, -1);//back
			if (CheckOutOfBounds(nPos)) {
				bData[i] = new BlockData(Vector3Int.zero, BlockType.AIR);

			}
			else {
				bData[i] = _currentSector.BlockData[nPos.x, nPos.y, nPos.z];
			}
			i++;
			nPos = pos + new Vector3Int(0, 1, 0);//top
			if (CheckOutOfBounds(nPos)) {
				bData[i] = new BlockData(Vector3Int.zero, BlockType.AIR);

			}
			else {
				bData[i] = _currentSector.BlockData[nPos.x, nPos.y, nPos.z];
			}
			i++;
			nPos = pos + new Vector3Int(0, -1, 0);//bot
			if (CheckOutOfBounds(nPos)) {
				bData[i] = new BlockData(Vector3Int.zero, BlockType.AIR);

			}
			else {
				bData[i] = _currentSector.BlockData[nPos.x, nPos.y, nPos.z];
			}


			//l r f b t b

			return bData;
		}
		static public bool CheckOutOfBounds(Vector3Int pos1) {
			//if you are checking out of bounds of the array return a
			return (pos1.x < 0 || pos1.x >= Sector._suSize ||
				pos1.y < 0 || pos1.y >= Sector._suSize ||
				pos1.z < 0 || pos1.z >= Sector._suSize);

		}
		static public int GetIndexFromPos(Vector3Int pos) {
			return (int)(pos.z + pos.y * Chunk._size + Chunk._size * Chunk._size * pos.x);

		}
		static public BlockData GetBlockFromPos(Vector3Int pos) {
			int index = (int)(pos.z + pos.y * Chunk._size + Chunk._size * Chunk._size * pos.x);
			return _blocks[index];
		}
		#endregion
		public static void CreateCube(Vector3Int pos, BlockType type, Mesh mesh, List<BlockData> blocks) {
			/*_vertices = new List<Vector3>(mesh.vertices);
			_uvs = new List<Vector2>(mesh.uv);
			_tris = new List<int>(mesh.triangles);
			_blocks = blocks;
			int index = GetIndexFromPos(pos);
			BlockData bd = _blocks[index];
			bd._on = true;
			bd._type = type;
			_blocks[index] = bd;

			ChunkGenerator.MakeCubeMesh(ChunkGenerator.GetNeighbours(pos), bd._type, pos);
			mesh.vertices = _vertices.ToArray();
			mesh.uv = _uvs.ToArray();
			mesh.triangles = _tris.ToArray();
			//_mesh.normals = normals;
			mesh.RecalculateNormals();
			ResetCache();*/
		}
		public static void DeleteCube(Vector3Int pos, Mesh mesh, List<BlockData> blocks) {

		/*	List<Vector3> vertices = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			List<int> tris = new List<int>();


			BlockData bd = blocks[GetIndexFromPos(pos)];
			bd._on = false;
			blocks[GetIndexFromPos(pos)] = bd;

			for (int x = 0; x < Chunk._size; x++) {
				for (int y = 0; y < Chunk._size; y++) {
					for (int z = 0; z < Chunk._size; z++) {
						pos.x = x; pos.y = y; pos.z = z;
						bd = GetBlockFromPos(pos);
						if (bd._on) {
							//get neighbours 
							BlockData[] nBlocks = GetNeighbours(pos);
							MakeCubeMesh(nBlocks, bd._type, pos);

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
		public Pair() {
		}
	}
}