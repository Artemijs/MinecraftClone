using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

	public static int _size;
	public static int _uSize;
	GameObject _collider;
	Mesh _solidMesh;
	Mesh _transMesh;
	Vector3Int _position;
	public Chunk(Vector3Int position, Node parent, Transform tparent) {

		_position = position;
		_solidMesh = new Mesh();
		_transMesh = new Mesh();
		_collider = new GameObject("mesh");
		_collider.transform.SetParent(tparent);
		_collider.AddComponent<MeshCollider>();
	}
	public void RecalculateCollider() {
		if (_collider == null)
			_collider = new GameObject();
		_collider.transform.position = _position;
		_collider.GetComponent<MeshCollider>().sharedMesh = _solidMesh;
	}
	public void Draw(Material mat) {
		Graphics.DrawMesh(_solidMesh, _position, Quaternion.identity, mat, 0);
	}
	public Mesh SolidMesh {
		get { return _solidMesh; }
		set { _solidMesh = value; }
	}
	public Mesh TransparentMesh {
		get { return _transMesh; }
		set { _transMesh = value; }
	}
	public GameObject Collider {
		get { return _collider; }
		set { _collider = value; }
	}

	public Vector3Int Position { get { return _position; } set { _position = value; } }

}