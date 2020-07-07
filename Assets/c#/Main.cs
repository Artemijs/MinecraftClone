using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	Node _map;
	Chunk _chunk;
	public Material _material;
	public Transform _player;
	Sector _current;
	void Awake() {
		MeshGenerator.Init();
		TerrrainGen.SetUp();
		
	}

	// Start is called before the first frame update
	void Start() {
		Vector3Int pPos = StaticFunctions.Vector3F2Int(_player.position);

		pPos.y = 0;
		_map = new Node(pPos, new Vector3Int(-200, -200, -200), StaticFunctions._maxDepth, null);
		
		int mapSize = 2;
		_map.CreateChildArea(pPos, 3);
		_current = (Sector)(_map.Search(10, 1, pPos));
		//Vector3Int pPos = StaticFunctions.Vector3F2Int(_player.position);
		/*for (int i = -1; i < mapSize; i++) {
			if (i == 0) continue;
			for (int j = -1; j < mapSize; j++) {
				if (j == 0) continue;
				_map.CreateChildBranch(pPos + new Vector3Int(i * Sector._suSize, 0, j * Sector._suSize) +
					new Vector3Int( Sector._suSize/2, 0,  Sector._suSize/2));
			}
		} */
		///UpdateLoadArea();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}
	Vector3[] _allPs = new Vector3[] { new Vector3(-1, 25, -1), new Vector3(0, 25, 0), new Vector3(-1, 25, -1) };
	// Update is called once per frame
	int _posIndex = 0;
	void Update() {
		_map.Draw(_material);
		if (!ActionQue.Empty()) return;
		if (Input.GetKeyDown(KeyCode.N)) {
			//_player.transform.position
			//_player.transform.position = _allPs[_posIndex%_allPs.Length];
			//_posIndex++;
		}
		//im being held captive against my free will pls help me 
		
		CheckPlayerMoved();
		HandleInput();
	}
	void CheckPlayerMoved() {
		Vector3Int player = StaticFunctions.Vector3F2Int(_player.position);
		if (!_current.IsInside(player)) {
			UpdateLoadArea();
			_current = (Sector)_map.Search(100, 1, player);
			//if (_current == null) {
			//	Debug.Log("CRRENT IS NULL");
		//	}
		}
	}
	void HandleInput() {

	}
	public void UpdateLoadArea() {
		Transform go = _player.Find("load_area");
		for (int i = 0; i < go.childCount; i++) {
			Vector3Int loadPos = StaticFunctions.Vector3F2Int(go.GetChild(i).position);
			if (!_map.CheckExists(loadPos, 1)) {
				//Vector3Int dir = GetDir(_current.Position, StaticFunctions.Vector3F2Int(_player.position));
				_current.CreateNextFromPos(loadPos);
				//ActionQue.Add2Que(s);
			}
		}
	}
}


