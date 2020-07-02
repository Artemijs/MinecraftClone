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

			Vector3 pPos = new Vector3(0, 0, 0);
			_map = new Node(pPos, Vector3Int.zero, 3, 0, null);
			BlockData bd = _map.GetBlock(StaticFunctions.Vector3F2Int(pPos));
			_current = (Sector)(_map.GetCurrent(10, 0, StaticFunctions.Vector3F2Int(pPos)));
			_current.InitBlockData();
			Debug.Log(bd.Type);
			MeshGenerator.GenerateMesh(_current);

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;
		}

		// Update is called once per frame
		void Update() {

			_map.Draw(_material);
			if ( !_current.IsInside(StaticFunctions.Vector3F2Int(_player.position))) {
				Sector s  = (Sector)_current.Parent.GetCurrent(10, 0, StaticFunctions.Vector3F2Int(_player.position));
				if (s == null) {
					_current = (Sector)_current.Parent.CreateOne(_current.Position, new Vector3Int(1, 0, 0));
					
				}
			}
			HandleInput();

		}
		void HandleInput() {
		}

	}

}
