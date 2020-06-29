using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// prototype version of player controller 
/// </summary>

public class ControllerTest : MonoBehaviour {

	GameObject _camera;
	//BBox[] _collisionBoxes;
	public GameObject _targetIndicator;
	bool _changedBlock = false;
	bool _lock = false;
	ChunkController _chunkCtrl;
	BlockType _selectedType;
	Vector3Int _targetPos;
	Vector3 _cursorNormal;
	void Start () {
		//_targetIndicator = GameObject.Instantiate(_targetIndicator);
		_selectedType = BlockType.DIRT;
		_camera = GameObject.Find ("Camera");
		_chunkCtrl = GameObject.Find("Main").GetComponent<ChunkController>();

	}
	// Update is called once per frame
	void Update () {
		//this.transform.position += new Vector3(0,0,-0.1f);
		HandleInput ();
		HandleTargetCursor();
		HandleMouseInput();

		//CheckBlockChange ();
	}

	private void HandleMouseInput() {
		if (Input.GetMouseButtonDown(0)) {
			if(Vector3.Distance(transform.position, _targetIndicator.transform.position)< 5)
				_chunkCtrl.CreateBlock(_targetIndicator.transform.position, _cursorNormal, _selectedType);
		}
		if (Input.GetMouseButton(1))
		{
			if (Vector3.Distance(transform.position, _targetIndicator.transform.position) < 5)
			{
				_chunkCtrl.StartDeletingBlock(_targetIndicator.transform.position, _cursorNormal);
				
				//SetTargetBlockPos(_targetIndicator.transform.position, _cursorNormal);
			}
		}
		if (Input.GetMouseButtonUp(1)) {
			_chunkCtrl.CancelDeleting();
		}
	}
	private void HandleInput(){
		float speed = 4;
		Vector3 playerDirf = Vector3.zero;
		if (Input.GetKey (KeyCode.W)) {
			
			playerDirf += _camera.transform.forward;
		}
		if (Input.GetKey (KeyCode.S)) {
			playerDirf += _camera.transform.forward*-1;
		}
		if (Input.GetKey (KeyCode.A)) {
			playerDirf += _camera.transform.right*-1;
		}
		if (Input.GetKey (KeyCode.D)) {
			playerDirf += _camera.transform.right;
		}
		if (Input.GetKey (KeyCode.Space)) {
			playerDirf.y = 1.5f;
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (Cursor.visible)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				Cursor.visible = (true);
				Cursor.lockState = CursorLockMode.None;
			}
		}
		gameObject.GetComponent<Rigidbody> ().velocity = playerDirf*speed;//
		if (playerDirf != Vector3.zero) {
			_chunkCtrl.SetCurrentChunk(transform.position);
		}
		if(!_lock)
			if (playerDirf != Vector3.zero) {
				_lock = true;
				GetComponent<Rigidbody>().useGravity = true;
			}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			_selectedType = BlockType.DIRT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			_selectedType = BlockType.DIRT_GRASS;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			_selectedType = BlockType.FROZEN_DIRT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			_selectedType = BlockType.FROZEN_ICE_DIRT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			_selectedType = BlockType.TREE_WOOD;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			_selectedType = BlockType.ROCK;
		}
	}
	void HandleTargetCursor() {
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 100.0f))
		{
			_targetIndicator.transform.position = hitInfo.point;
			_cursorNormal = hitInfo.normal;
			
		}
		else {
			_targetIndicator.transform.position = Vector3.zero;
			_cursorNormal = Vector3.zero;
		}
	}
	
	private void CheckBlockChange (){
		//legacy, will be rewritten later
		/*int newId = _world.ChunkCtrl.GetBlockId (transform.position);
		if (newId == -1)
			return;
		if (newId != _cBlockId) {
			_changedBlock = true;
			_cBlockId = newId;

		} else
			_changedBlock = false;*/
	}
}
