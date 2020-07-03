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
		Vector3[] _allPs = new Vector3[] { new Vector3(-1, 25, -1), new Vector3(0, 25, 0), new Vector3(-1, 25, -1) };
		// Update is called once per frame
		int _posIndex = 0;
		void Update() {

			if (Input.GetKeyDown(KeyCode.N)) {
				//_player.transform.position
				_player.transform.position = _allPs[_posIndex%_allPs.Length];
				_posIndex++;
			}
			//im being held captive against my free will pls help me 
			_map.Draw(_material);
			CheckPlayerMoved();
			HandleInput();
		}
		void CheckPlayerMoved() {
			Vector3Int player = StaticFunctions.Vector3F2Int(_player.position);
			if (!_current.IsInside(player)) {
				if (!_current.CheckExists(player, 666)) {
					//Vector3Int dir = GetDir(_current.Position, StaticFunctions.Vector3F2Int(_player.position));
					Sector s = (Sector)_current.CreateNextFromPos(player);
					s.InitBlockData();
					MeshGenerator.GenerateMesh(s);
					_current = s;
				}
				else {
					_current = (Sector)_map.Search(10, 666, StaticFunctions.Vector3F2Int(_player.position));
				}
			
			}
		}
		
		void HandleInput() {

		}

	}

}
