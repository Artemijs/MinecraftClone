using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour {
	/*private enum BoxSides
	{
		LEFT =0,
		RIGHT,
		FORWARD,
		BACK,
		TOP,
		BOTTOM

	};*/
	private enum BoxSides
	{
		FTL=0,
		FT,
		FTR,
		FL,
		F,
		FR,
		FBL,
		FB,
		FBR,
		BTL,
		BT,
		BTR,
		BL,
		BOT,
		BR,
		BBL,
		BB,
		BBR,
		TL,
		T,
		TR,
		R,
		RB,
		BACK,
		LB,
		L,
		END
		
	};
	public GameObject _boxTemplate;
	World _world;
	BBox _botBox;
	GameObject _camera;
	//BBox[] _collisionBoxes;

	bool _changedBlock = false;
	int _cBlockId;

	void Start () {
		
		//_world = GameObject.Find ("World").GetComponent<World> ();
		_camera = GameObject.Find ("Camera");
		//_world.ChunkCtrl.GetBlockId (transform.position);

		//GameObject box = GameObject.Instantiate (_boxTemplate);
		//_botBox = new BBox (box);
		/*_collisionBoxes = new BBox[(int)(BoxSides.END)];
		for (int i = 0; i < _collisionBoxes.Length; i++) {
			_collisionBoxes [i] = new BBox (GameObject.Instantiate (_boxTemplate));
		}
		_collisionBoxes [(int)BoxSides.BOTTOM].SetPosition (transform.position + new Vector3 (0, -Block._size.y, 0));
		_collisionBoxes [(int)BoxSides.TOP].SetPosition (transform.position + new Vector3 (0, Block._size.y, 0));
		_collisionBoxes [(int)BoxSides.FORWARD].SetPosition (transform.position + new Vector3 (0, 0, Block._size.z));
		_collisionBoxes [(int)BoxSides.BACK].SetPosition (transform.position + new Vector3 (0, 0, -Block._size.z));
		_collisionBoxes [(int)BoxSides.LEFT].SetPosition (transform.position + new Vector3 (-Block._size.x, 0, 0));
		_collisionBoxes [(int)BoxSides.RIGHT].SetPosition (transform.position + new Vector3 (Block._size.x, 0, 0));*/
		//_botBox.SetPosition (transform.position + new Vector3 (0, -Block._size.y, 0));

	}
	// Update is called once per frame
	void Update () {
		/*if (!_world.Ready)
			
			return;*/
		HandleInput ();
		//CheckBlockChange ();
		/*for(int i =0; i < _collisionBoxes.Length; i++){
			Block b = _world.ChunkCtrl.GetNeighbor ((BlockSide )i, transform.position );
			if(_changedBlock){
				if(b!= null)
					_collisionBoxes[i].SetPosition(b.Position);
			}
			if(b!=null)
				_collisionBoxes[i].Update (transform.position, b);
		}*/
		/*Block b = _world.GetNeighbor (BlockSide.BOTTOM, transform.position);
		if(_changedBlock){
			_collisionBoxes[(int)BoxSides.BOTTOM].SetPosition(b.Position);
		}
		_collisionBoxes[(int)BoxSides.BOTTOM].Update (transform.position, b);*/

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
		/// </summary> new Vector3 (speed, 0, 0);//
	}
	private void CheckBlockChange (){
		int newId = _world.ChunkCtrl.GetBlockId (transform.position);
		if (newId == -1)
			return;
		if (newId != _cBlockId) {
			_changedBlock = true;
			_cBlockId = newId;

		} else
			_changedBlock = false;
	}
}
