using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// prototype version of player controller 
/// </summary>
public class ControllerTest : MonoBehaviour {

	GameObject _camera;
	//BBox[] _collisionBoxes;

	bool _changedBlock = false;
	bool _lock = false;
	void Start () {
		
		
		_camera = GameObject.Find ("Camera");
		

	}
	// Update is called once per frame
	void Update () {
		HandleInput ();
		//CheckBlockChange ();
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
