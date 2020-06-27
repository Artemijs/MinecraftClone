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
	Main _main;

	Vector3Int _targetPos;
	Vector3 _cursorNormal;
	void Start () {
		//_targetIndicator = GameObject.Instantiate(_targetIndicator);
		
		_camera = GameObject.Find ("Camera");
		_main = GameObject.Find("Main").GetComponent<Main>();

	}
	// Update is called once per frame
	void Update () {
		HandleInput ();
		HandleTargetCursor();
		HandleMouseInput();

		//CheckBlockChange ();
	}

	private void HandleMouseInput() {
		if (Input.GetMouseButtonDown(0)) {
			if(Vector3.Distance(transform.position, _targetIndicator.transform.position)< 5)
				_main.CreateBlock(_targetIndicator.transform.position, _cursorNormal);
		}
		if (Input.GetMouseButton(1))
		{
			if (Vector3.Distance(transform.position, _targetIndicator.transform.position) < 5)
			{
				_main.StartDeletingBlock(_targetIndicator.transform.position, _cursorNormal);
				
				//SetTargetBlockPos(_targetIndicator.transform.position, _cursorNormal);
			}
		}
		if (Input.GetMouseButtonUp(1)) {
			_main.CancelDeleting();
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
		if(!_lock)
			if (playerDirf != Vector3.zero) {
				_lock = true;
				GetComponent<Rigidbody>().useGravity = true;
			}

	}
	void HandleTargetCursor() {
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 100.0f))
		{
			Debug.Log("Hanppened");
			_targetIndicator.transform.position = hitInfo.point;
			_cursorNormal = hitInfo.normal;
			
		}
		else {
			Debug.Log("NEVER Hanppened");
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
