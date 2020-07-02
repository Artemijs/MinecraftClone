using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Version3_1 {
public class MapData : MonoBehaviour
{


    // Start is called before the first frame update
		void Awake()
		{
			Chunk._size = 5;
			Chunk._uSize = Chunk._size * 1;

			Sector._size = 16;
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
		Vector3Int _minPos;
		public static int _size;
		public static int _uSize;
		Chunk[,] _chunks;//16x16
		BlockData[,] _blockData; 
		public Sector(Vector3Int position, int maxDepth, int depth)	
		{
			_minPos = Vector3Int.zero;
			_chunks = new Chunk[_size, _size];
			Vector3Int pos = Vector3Int.zero;
			_blockData = new BlockData[_uSize, _uSize];
			for (int i = 0; i < _size; i++)
			{
				for (int j = 0; j < _size; j++)
				{
					pos.x = i * _uSize;
					pos.z = j * _uSize;
					_chunks[i, j] = new Chunk(pos);
				}
			}
		}
		public bool IsInside(Vector3Int pos)
		{
			return (pos.x >= _minPos.x && pos.y >= _minPos.y && pos.x >= _minPos.z &&
				pos.x < _minPos.x + _uSize && pos.y < _minPos.y + _uSize && pos.x < _minPos.z + _uSize);
		}
	}
	public class Chunk {
		Vector3Int _minPos;
		public static int _size;
		public static int _uSize;
		Mesh _solidMesh;
		Mesh _transMesh;
		public Chunk(Vector3Int position)
		{
			_minPos = Vector3Int.zero;
			//_chunks = new Chunk[_size, _size];
			Vector3Int pos = Vector3Int.zero;

			
		}

	}
	public class BlockData {
		BlockType _bt;
		Vector3 _position;//world space
	};
	public class Node
	{
		Vector3Int _position;//real world position
		static int _size = 4;
		int _uSize;
		Node[,] _nodes;
		Sector _sector;
		
		public Node(Vector3Int position, int maxDepth, int depth)
		{
			_sector = null;
			_position = position;
			_uSize = Sector._uSize;
			//you are at singular block level;
			if (depth == maxDepth) {

				_sector = new Sector(position);
				_nodes = null;

				return;
			}
			
			for (int i = depth; i < maxDepth; i++)
			{
				_uSize *= _size;
			}
			_nodes = new Node[_size, _size];
			Vector3Int pos = position;
			for (int i = 0; i < _size; i++)
			{
				for (int j = 0; j < _size; j++)
				{
					pos.x = position.x + i * _uSize;
					pos.z = position.z + j * _uSize;
					depth++;
					_nodes[i, j] = new Node(pos, maxDepth, depth);
				}
			}
		}
		public BlockData GetBlock(Vector3Int pos) {
			BlockData bd = null;
			for (int i = 0; i < _nodes.Length; i++) {
				for (int j = 0; j < _nodes.Length; j++) {
					if (_nodes != null) {
						if (_nodes[i, j].IsInside(pos)) {
							bd = _nodes[i, j].GetBlock(pos);
						}
					}
					else {
						if (_sector.IsInside(pos)) {
							bd = _nodes[i, j].GetBlock(pos);
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

