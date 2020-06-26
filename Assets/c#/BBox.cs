using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this file was used to check collision between player and the world :D
/// </summary>
public class BBox {
	bool _locked;
	Collider _boxCol;
	public BBox(GameObject box){
		//box.transform.position = transform.position + new Vector3 (0, -Block._size.y, 0);
		box.transform.localScale = Block._size;
		_boxCol = box.GetComponent<BoxCollider> ();
	}
	public void Update(Vector3 playerPos, Block b){
		if (b.Type == BlockType.AIR) {
			_locked = false;
			_boxCol.isTrigger = true;
		} else {
			_locked = true;
			SetPosition (b.Position);
			_boxCol.isTrigger = false;
		}
		if (!_locked && _boxCol.gameObject.transform.position != b.Position)
			SetPosition (b.Position);
	}
	public void SetPosition(Vector3 pos){
		_boxCol.gameObject.transform.position = pos;
	}

}
