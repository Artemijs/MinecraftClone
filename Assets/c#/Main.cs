using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Version3_1 {
	public class Main : MonoBehaviour {

		Node _map;
		Chunk _chunk;
		public Material _material;
		public Transform _player;
		Sector _current;
		void Awake() {
			MeshGenerator.Init();
			TerrrainGen.SetUp();
			Chunk._size = 5;
			Chunk._uSize = Chunk._size * 1;

			Sector._sSize = 10;
			Sector._suSize = Sector._sSize * Chunk._uSize;
		}

		// Start is called before the first frame update
		void Start() {
			Vector3Int pPos = StaticFunctions.Vector3F2Int(_player.position);

			pPos.y = 0;
			_map = new Node(_player.position, new Vector3Int(-1000, -1000, -1000), StaticFunctions._maxDepth, 0, null);
			BlockData bd = _map.GetBlock(pPos);
			_current = (Sector)(_map.Search(10, 0, pPos));
			_current.InitBlockData();
			Debug.Log(bd.Type);
			MeshGenerator.GenerateMesh(_current);

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;
		}

		// Update is called once per frame
		void Update() {

			_map.Draw(_material);
			CheckPlayerMoved();
			HandleInput();

		}
		void CheckPlayerMoved() {
			if (!_current.IsInside(StaticFunctions.Vector3F2Int(_player.position))) {
				Node n = _current.Parent.Parent.Search(10, 0, StaticFunctions.Vector3F2Int(_player.position));
				if (n != null) {
					_current = (Sector)n;
					return;
				}
				//Vector3Int dir = GetDir(_current.Position, StaticFunctions.Vector3F2Int(_player.position));
				Sector s = (Sector)_current.CreateNextFromPos(StaticFunctions.Vector3F2Int(_player.position));
				s.InitBlockData();
				MeshGenerator.GenerateMesh(s);
				_current = s;
			}
		}
		
		void HandleInput() {

		}

	}

}
