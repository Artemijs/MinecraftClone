using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Version3_1 {
public class MapData : MonoBehaviour
{


    // Start is called before the first frame update
	void Awake()
	{
		Chunk._size = 5;
		Chunk._uSize = Chunk._size * 1;

		Sector._size = 10;
		Sector._uSize = Sector._size * Chunk._uSize;
	}
	void Start() {
			//Vector3 pPos = new Vector3(UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(300, 1000));
			Vector3 pPos = new Vector3(0, 0, 0);
			Node n = new Node(pPos, Vector3Int.zero, 5, 0, null);
			BlockData bd = n.GetBlock(StaticFunctions.Vector3F2Int(pPos));
			Debug.Log(bd.Data);
	}

	// Update is called once per frame
	void Update()
	{
        
	}
	//GetBlockFromPosition()
	public BlockData GetBlockFromPosition(Vector3 pos) {

		return null;
	}
}
	public class Sector : Node
	{

		public static int _size;
		public static int _uSize;
		
		BlockData[,,] _blockData;
		/// <summary>
		/// CREATE THE MESHES HERE THIS TIME AND GIVE IT TO CHUNK
		/// </summary>
		/// <param name="position"> IN WORDLD SPACE </param>
		public Sector(Vector3Int position, Node parent): base(parent) {
			
			_position = position;
			_nodes = new Chunk[_size, _size, _size];
			Vector3Int pos = Vector3Int.zero;
			_blockData = new BlockData[_uSize, _uSize, _uSize];
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _size; k++) {
						pos.x = i * _uSize;
						pos.z = j * _uSize;
						_nodes[i, j, k] = new Chunk(pos, this);
					}
				}
			}
			CreateBlockData();
		}
		private void CreateBlockData() {
			for (int i = 0; i < _uSize; i++) {
				for (int j = 0; j < _uSize; j++) {
					for (int k = 0; k < _uSize; k++) {
						BlockType bt = BlockType.ROCK;
						if (j + _position.y > 2)
							bt = BlockType.AIR;
						_blockData[i, j, k] = new BlockData(_position + new Vector3Int(i, j, k), bt);
					}
				}
			}
		}
		public override BlockData GetBlock(Vector3Int pos) {
			pos -= _position;
			return _blockData[pos.x, pos.y, pos.z];
		}
	}

	public class Chunk : Node {
		
		public static int _size;
		public static int _uSize;
		Mesh _solidMesh;
		Mesh _transMesh;
		public Chunk(Vector3Int position, Node parent) : base(parent) {
			_position = position;
			_nodes = null;
		}

	}
	public class BlockData {
		BlockType _bt;
		Vector3 _position;//world space
		public BlockData(Vector3 pos, BlockType bt) {
			_position = pos;
			_bt = bt;
		}
		public BlockType Data { get { return _bt; } }
	};
	public class Node
	{
		protected Vector3Int _position;//real world position
		static int _size = 2;
		int _uSize;
		protected Node[,,] _nodes;
		protected Node _parent;
		public Node(Node parent) {
			_parent = parent;
		}
		public Node(Vector3 pPos, Vector3Int position, int maxDepth, int depth, Node parent)
		{
			_parent = parent;
			_position = position;
			_uSize = Sector._uSize;
			Vector3Int pos = position;
			depth++;
			_nodes = new Node[_size, _size, _size];
			//u start at depth +=1 so _uSize is 1 less than it should be
			for (int i = depth; i < maxDepth; i++) {
				_uSize *= _size;
			}
			//which means that it is currently the uSize of the child node 
			Vector3Int max = pos;
			for (int i = 0; i < _size; i++) {
				pos.x = position.x + i * _uSize;
				max.x = pos.x + _uSize;

				for (int j = 0; j < _size; j++) {
					pos.y = position.y + j * _uSize;
					max.y = pos.y + _uSize;

					for (int k = 0; k < _size; k++) {
						//min pos of child ijk
						pos.z = position.z + k * _uSize;
						max.z = pos.z + _uSize;
						if (pPos.x >= pos.x && pPos.y >= pos.y && pPos.z >= pos.z &&
							pPos.x < max.x && pPos.y < max.y && pPos.z < max.z) {

							if (depth == maxDepth) {
								_nodes[i, j, k] = new Sector(pos, this);
							}
							else {
								_nodes[i, j, k] = new Node(pPos, pos, maxDepth, depth, this);
							}	
						}
						else {
							_nodes[i, j, k] = null;
						}
					}
				}
			}
			//and now its the current nodes uSize
			_uSize *= _size;
		}
		public virtual BlockData GetBlock(Vector3Int pos) {
			BlockData bd = null;
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _size; k++) {
						//if (_nodes != null) {
						if (_nodes[i, j, k] != null) {
							if (_nodes[i, j, k].IsInside(pos)) {
								bd = _nodes[i, j, k].GetBlock(pos);
							}
							else {
								Debug.Log("WTF");
							}
						}
						
					}
				}
			}
			return bd;
		}
		public bool IsInside(Vector3Int pos) {
			bool b = (pos.x >= _position.x && pos.y >= _position.y && pos.x >= _position.z &&
				pos.x < _position.x + _uSize && pos.y < _position.y + _uSize && pos.z < _position.z + _uSize);
			if (pos.x >= _position.x && pos.y >= _position.y && pos.x >= _position.z &&
				pos.x < _position.x + _uSize && pos.y < _position.y + _uSize && pos.z < _position.z + _uSize) {
				Debug.Log("???????????????");
			}
				return b;
		}
	}
	public static class StaticFunctions {
		public static Vector3Int Vector3F2Int(Vector3 pos) {
			return new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
		}
	};

}


