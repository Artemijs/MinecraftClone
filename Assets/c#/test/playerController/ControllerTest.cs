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
			if(Vector3.Distance(transform.position, _targetIndicator.transform.position)< 2)
				_main.CreateBlock(_targetIndicator.transform.position);
		}
		if (Input.GetMouseButtonDown(1))
		{
			if (Vector3.Distance(transform.position, _targetIndicator.transform.position) < 2)
				_main.DeleteBlock(_targetIndicator.transform.position);
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
		}
		else {
			Debug.Log("NEVER Hanppened");
			_targetIndicator.transform.position = Vector3.zero;
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
