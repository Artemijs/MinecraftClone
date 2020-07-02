using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Version3_1 {
public class MapData : MonoBehaviour
{
		Node _map;
		public Material _material;

    // Start is called before the first frame update
	void Awake()
	{
		MeshGenerator.Init();
			TerrrainGen.SetUp();
		Chunk._size = 5;
		Chunk._uSize = Chunk._size * 1;

		Sector._sSize = 10;
		Sector._suSize = Sector._sSize * Chunk._uSize;
	}
	void Start() {
			//Vector3 pPos = new Vector3(UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(300, 1000));
			Vector3 pPos = new Vector3(0, 0, 0);
			_map = new Node(pPos, Vector3Int.zero, 3, 0, null);
			BlockData bd = _map.GetBlock(StaticFunctions.Vector3F2Int(pPos));
			Sector current = (Sector)(_map.Search(10, 0, StaticFunctions.Vector3F2Int(pPos)));
			_map.InitBlockData();
			Debug.Log(bd.Type);
			MeshGenerator.GenerateMesh(current);
	}

	// Update is called once per frame
	void Update()
	{
			_map.Draw(_material);
	}
	//GetBlockFromPosition()
	public BlockData GetBlockFromPosition(Vector3 pos) {

		return null;
	}
}
	public class Sector : Node
	{

		public static int _sSize;
		public static int _suSize;
		Chunk[,,] _chunks;
		BlockData[,,] _blockData;
		/// <summary>
		/// CREATE THE MESHES HERE THIS TIME AND GIVE IT TO CHUNK
		/// </summary>
		/// <param name="position"> IN WORDLD SPACE </param>
		public Sector(Vector3Int position, Node parent): base(parent) {
			base._uSize = _suSize;
			_position = position;
			_chunks = new Chunk[_sSize, _sSize, _sSize];
			Vector3Int pos = Vector3Int.zero;
			_blockData = new BlockData[_suSize, _suSize, _suSize];
			for (int i = 0; i < _sSize; i++) {
				for (int j = 0; j < _sSize; j++) {
					for (int k = 0; k < _sSize; k++) {
						pos.x = i * Chunk._uSize;
						pos.y = j * Chunk._uSize;
						pos.z = k * Chunk._uSize;
						_chunks[i, j, k] = new Chunk(pos + position, this);
					}
				}
			}
			_depth = 666;
			CreateBlockData();
		}
		private void CreateBlockData() {
			for (int i = 0; i < _suSize; i++) {
				for (int j = 0; j < _suSize; j++) {
					for (int k = 0; k < _suSize; k++) {
						BlockType bt = TerrrainGen.GetBlockType(_position + new Vector3Int(i, j, k));
						//if (j + _position.y > 2)
						//	bt = BlockType.AIR;
						_blockData[i, j, k] = new BlockData(_position + new Vector3Int(i, j, k), bt);
					}
				}
			}
			
		}
		public override BlockData GetBlock(Vector3Int pos) {
			pos -= _position;
			if (pos.x < 0 || pos.y < 0 || pos.z < 0 ||
				pos.x >= Sector._suSize || pos.y >= Sector._suSize || pos.z >= Sector._suSize)
				return null;
			return _blockData[pos.x, pos.y, pos.z];
		}
		public override bool IsInside(Vector3Int pos) {
			return (pos.x >= _position.x && pos.y >= _position.y && pos.z >= _position.z &&
			pos.x < _position.x + _uSize && pos.y < _position.y + _uSize && pos.z < _position.z + _uSize);
		}
		public Chunk[,,] Chunks { get { return _chunks; } set { _chunks = value; } }
		public BlockData[,,] BlockData { get { return _blockData; } set { _blockData = value; } }

		public override void Draw(Material mat) {
			for (int i = 0; i < _sSize; i++) {
				for (int j = 0; j < _sSize; j++) {
					for (int k = 0; k < _sSize; k++) {
						_chunks[i, j, k].Draw(mat);
					}
				}
			}
		}
		public override void InitBlockData() {
			TerrrainGen.SecondPass(this);
		}
	}

	public class Chunk {
		
		public static int _size;
		public static int _uSize;
		GameObject _collider;
		Mesh _solidMesh;
		Mesh _transMesh;
		Vector3Int _position;
		public Chunk(Vector3Int position, Node parent) {
			_position = position;
			_solidMesh = new Mesh();
			_transMesh = new Mesh();
			_collider = new GameObject();
			_collider.AddComponent<MeshCollider>();
		}
		public void RecalculateCollider() {
			if (_collider == null)
				_collider = new GameObject();
			_collider.transform.position = _position;
			_collider.GetComponent<MeshCollider>().sharedMesh = _solidMesh;
		}
		public void Draw(Material mat) {
			Graphics.DrawMesh(_solidMesh, _position, Quaternion.identity, mat, 0);
		}
		public Mesh SolidMesh {
			get { return _solidMesh; }
			set { _solidMesh = value; }
		}
		public Mesh TransparentMesh {
			get { return _transMesh; }
			set { _transMesh = value; }
		}
		public GameObject Collider {
			get { return _collider; }
			set { _collider = value; }
		}
		
		public Vector3Int Position { get { return _position; } set { _position = value; } }

	}
	public class BlockData {
		BlockType _bt;
		Vector3 _position;//world space
		public BlockData(Vector3 pos, BlockType bt) {
			_position = pos;
			_bt = bt;
		}
		public BlockType Type { get { return _bt; }set { _bt = value; } }
	};
	
	public static class StaticFunctions {
		public static int _maxDepth = 5;
		public static Vector3Int Vector3F2Int(Vector3 pos) {
			return new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
		}
	};

}


