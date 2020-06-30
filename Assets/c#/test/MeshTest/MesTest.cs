using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesTest : MonoBehaviour {
	public GameObject _gomesh;
	public Material _material;
	public bool _draw = false;
	Mesh _mesh;
	Block b;
	public Vector3 _boxSize;
	// Use this for initialization
	void Start () {
		b = new Block (Vector3.zero);
		_mesh = _gomesh.GetComponent<MeshFilter> ().sharedMesh;
		for (int i = 0; i < b.Plains.Length; i++) {

			b.Plains [i].Mesh = _mesh;
		}

		SetBlockPlaneData (ref b);
	}
	
	// Update is called once per frame
	void Update () {
		if (!_draw)
			return;
		Graphics.DrawMesh (_mesh, Vector3.zero, Quaternion.identity, _material, 0);
		//Matrix4x4 m = Matrix4x4.TRS ();
		/*Vector3 pos = new Vector3(1,1,1);
		Quaternion rot = Quaternion.identity;
		Vector3 scale = new Vector3 (1, 2, 1);
		Matrix4x4 m = Matrix4x4.TRS (pos, rot, scale);*/

		//Graphics.DrawMesh (_mesh, m, _material, 0);
		Plane[] planes = b.Plains;
		for (int i = 0; i < planes.Length; i++) {
			
			Graphics.DrawMesh (planes[i].Mesh, planes[i].Transform, _material, 0);
		}

		//_draw = false;
	}
	private void SetBlockPlaneData(ref Block b){
		Plane[] bPlains = b.Plains;
		//left right forward back up down
		Plane f = bPlains [(int)BlockSide.FORWARD];
		f.Position = new Vector3 (0, 0, _boxSize.z * 0.5f);
		f.Rotation = Quaternion.Euler (0, 180, 0);
		f.Scale = new Vector3 (_boxSize.x, _boxSize.y, 1);

		f = bPlains [(int)BlockSide.BACK];
		f.Position = new Vector3 (0, 0, _boxSize.z * -0.5f);
		f.Rotation = Quaternion.Euler (0, 0, 0);
		f.Scale = new Vector3 (_boxSize.x, _boxSize.y, 1);

		f = bPlains [(int)BlockSide.LEFT];
		f.Position = new Vector3 (_boxSize.x * -0.5f, 0, 0);
		f.Rotation = Quaternion.Euler (0, 90, 0);
		f.Scale = new Vector3 (_boxSize.z, _boxSize.y, 1);

		f = bPlains [(int)BlockSide.RIGHT];
		f.Position = new Vector3 (_boxSize.x * 0.5f, 0, 0);
		f.Rotation = Quaternion.Euler (0, -90, 0);
		f.Scale = new Vector3 (_boxSize.z, _boxSize.y, 1);

		f = bPlains [(int)BlockSide.TOP];
		f.Position = new Vector3 (0, _boxSize.y * 0.5f, 0);
		f.Rotation = Quaternion.Euler (90, 0, 0);
		f.Scale = new Vector3 (_boxSize.x, _boxSize.z, 1);

		f = bPlains [(int)BlockSide.BOTTOM];
		f.Position = new Vector3 (0, _boxSize.y * -0.5f, 0);
		f.Rotation = Quaternion.Euler (-90, 0, 0);
		f.Scale = new Vector3 (_boxSize.x, _boxSize.z, 1);

	}
}
