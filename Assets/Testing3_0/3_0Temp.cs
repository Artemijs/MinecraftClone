using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp3_0 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
/// <summary>
/// 3.0
///  whilst writing this version i came up with 3.1 so this is depricated beforei t got finished :D
/// </summary>
namespace Version3_0 {
	public class Map {
		Vector3Int _minPos;
		static int _size;
		public static int _uSize;
		Continent[,] _continents;//4x4
		public Map() {
			InitSizes();
			_minPos = Vector3Int.zero;
			_continents = new Continent[_size, _size];
			Vector3Int pos = Vector3Int.zero;

			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					pos.x = i * _uSize;
					pos.z = j * _uSize;
					_continents[i, j] = new Continent(pos);
				}
			}
		}
		public void InitSizes() {
			Chunk._size = 5;
			Chunk._uSize = Chunk._size * 1;

			Sector._size = 16;
			Sector._uSize = Sector._size * Chunk._uSize;

			Region._size = 4;
			Region._uSize = Region._size * Sector._uSize;

			Area._size = 4;
			Area._uSize = Area._size * Region._uSize;

			Continent._size = 4;
			Continent._uSize = Continent._size * Area._uSize;

			Map._size = 4;
			Map._uSize = Map._size * Continent._uSize;
		}
	}
	public class Continent {
		Vector3Int _minPos;
		public static int _size;
		public static int _uSize;
		Area[,] _areas;//4x4
		public Continent(Vector3Int position) {
			_minPos = Vector3Int.zero;
			_areas = new Area[_size, _size];
			Vector3Int pos = Vector3Int.zero;

			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					pos.x = i * _uSize;
					pos.z = j * _uSize;
					_areas[i, j] = new Area(pos);
				}
			}
		}
	}
	public class Area {
		Vector3Int _minPos;
		public static int _size;
		public static int _uSize;
		Region[,] _regions;//4x4
		public Area(Vector3Int position) {
			_minPos = Vector3Int.zero;
			_regions = new Region[_size, _size];
			Vector3Int pos = Vector3Int.zero;

			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					pos.x = i * _uSize;
					pos.z = j * _uSize;
					_regions[i, j] = new Region(pos);
				}
			}
		}
	}
	public class Region {
		Vector3Int _minPos;
		public static int _size;
		public static int _uSize;
		Sector[,] _sectors;//16x16
		public Region(Vector3Int position) {
			_minPos = Vector3Int.zero;
			_sectors = new Sector[_size, _size];
			Vector3Int pos = Vector3Int.zero;

			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					pos.x = i * _uSize;
					pos.z = j * _uSize;
					_sectors[i, j] = new Sector(pos);
				}
			}
		}
	}


	public class Sector {
		Vector3Int _minPos;
		public static int _size;
		public static int _uSize;
		Chunk[,] _chunks;//16x16
		BlockData[,] _blockData;
		public Sector(Vector3Int position) {
			_minPos = Vector3Int.zero;
			_chunks = new Chunk[_size, _size];
			Vector3Int pos = Vector3Int.zero;
			_blockData = new BlockData[_uSize, _uSize];
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					pos.x = i * _uSize;
					pos.z = j * _uSize;
					_chunks[i, j] = new Chunk(pos);
				}
			}
		}
		public bool IsInside(Vector3Int pos) {
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
		public Chunk(Vector3Int position) {
			_minPos = Vector3Int.zero;
			//_chunks = new Chunk[_size, _size];
			Vector3Int pos = Vector3Int.zero;


		}

	}
}
