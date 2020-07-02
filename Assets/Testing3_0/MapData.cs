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
		
		BlockData[,] _blockData;
		/// <summary>
		/// CREATE THE MESHES HERE THIS TIME AND GIVE IT TO CHUNK
		/// </summary>
		/// <param name="position"> IN WORDLD SPACE </param>
		public Sector(Vector3Int position, Node parent): base(parent) {
			
			_position = position;
			_nodes = new Chunk[_size, _size, _size];
			Vector3Int pos = Vector3Int.zero;
			_blockData = new BlockData[_uSize, _uSize];
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _uSize; k++) {
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
						_blockData[i, j] = new BlockData(_position + new Vector3Int(i, j, k), bt);
					}
				}
			}
		}
	}

	public class Chunk : Node {
		Vector3Int _minPos;
		public static int _size;
		public static int _uSize;
		Mesh _solidMesh;
		Mesh _transMesh;
		public Chunk(Vector3Int position, Node parent) : base(parent) {
			_minPos = Vector3Int.zero;
			//_chunks = new Chunk[_size, _size];
			Vector3Int pos = Vector3Int.zero;
		}

	}
	public class BlockData {
		BlockType _bt;
		Vector3 _position;//world space
		public BlockData(Vector3 pos, BlockType bt) {
			_position = pos;
			_bt = bt;
		}
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
		public Node(Vector3Int position, int maxDepth, int depth, Node parent)
		{
			_parent = parent;
			_position = position;
			_uSize = Sector._uSize;
			Vector3Int pos = position;
			//you are at singular block level;

			if (depth == maxDepth) {
				_nodes = new Node[_size, _size, _size];
				for (int i = 0; i < _size; i++) {
					for (int j = 0; j < _size; j++) {
						for (int k = 0; k < _uSize; k++) {
							pos.x = position.x + i * _uSize;
							pos.y = position.y + j * _uSize;
							pos.z = position.z + k * _uSize;
							_nodes[i, j, k] = new Sector(pos, this);
						}
					}
				}

				return;
			}

			for (int i = depth; i < maxDepth; i++) {
				_uSize *= _size;
			}
			_nodes = new Node[_size, _size, _size];
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _uSize; k++) {
						pos.x = position.x + i * _uSize;
						pos.y = position.y + j * _uSize;
						pos.z = position.z + k * _uSize;
						depth++;
						_nodes[i, j, k] = new Node(pos, maxDepth, depth, this);
					}
				}
			}
		}
		public BlockData GetBlock(Vector3Int pos) {
			BlockData bd = null;
			for (int i = 0; i < _nodes.Length; i++) {
				for (int j = 0; j < _nodes.Length; j++) {
					for (int k = 0; k < _nodes.Length; k++) {
						if (_nodes[i, j, k].IsInside(pos)) {
							bd = _nodes[i, j, k].GetBlock(pos);
						}
					}
				}
			}
			return bd;
		}
		public bool IsInside(Vector3Int pos) {
			return (pos.x >= _position.x && pos.y >= _position.y && pos.x >= _position.z &&
				pos.x < _position.x + _uSize && pos.y < _position.y + _uSize && pos.x < _position.z + _uSize);
		}
	}
}

