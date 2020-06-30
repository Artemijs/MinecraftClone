using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSizeTest : MonoBehaviour {
	GameObject[] planes;
	public Vector3 size;
	public bool update = false;
	// Use this for initialization
	void Start () {
		planes = new GameObject[gameObject.transform.childCount];
		for (int i = 0; i < planes.Length; i++) {
			planes [i] = gameObject.transform.GetChild (i).gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!update)
			return;
		ChangeSize ();
		update = false;
	}
	void ChangeSize(){
		GameObject f = planes [(int)BlockSide.FORWARD];
		Vector3 pos = f.transform.position;
		Vector3 scale = f.transform.localScale;
		pos.z = size.z * 0.5f;
		scale.y = size.y;
		scale.x = size.x;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = planes [(int)BlockSide.BACK];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.z = size.z * -0.5f;
		scale.y = size.y;
		scale.x = size.x;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = planes [(int)BlockSide.LEFT];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.x = size.x * -0.5f;
		scale.x = size.z;
		scale.y = size.y;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = planes [(int)BlockSide.RIGHT];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.x = size.x * 0.5f;
		scale.x = size.z;
		scale.y = size.y;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = planes [(int)BlockSide.TOP];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.y = size.y * 0.5f;
		scale.y = size.z;
		scale.x = size.x;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = planes [(int)BlockSide.BOTTOM];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.y = size.y * -0.5f;
		scale.y = size.z;
		scale.x = size.x;
		f.transform.position = pos;
		f.transform.localScale = scale;
	}
}
