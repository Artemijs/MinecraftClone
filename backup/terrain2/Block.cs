using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
	public static Vector3 _size = new Vector3 (1, 0.5f, 1);
	BlockType _type;
	GameObject[] _plains;
	GameObject _goBlock;
	//Transform _transform;
	public Block(Vector3 pos){
		_goBlock = new GameObject ("block");
		_plains = new GameObject[6];
		//_transform = new Transform ();
		//_transform.position = pos;
		//_transform.localRotation = Quaternion.identity;
		_goBlock.transform.position = pos;
	}
	public void ParentMesh(Transform chunkParent){
		for (int i = 0; i < _plains.Length; i++) {
			if (_plains [i] != null)
				_plains [i].transform.SetParent (_goBlock.transform);
		}
		_goBlock.transform.SetParent (chunkParent);
	}
	public Transform Parent{
		get{ return _goBlock.transform; }
	}
	public Vector3 Position{
		get{ return _goBlock.transform.position; }
		set{ _goBlock.transform.position = value; }
	}
	public BlockType Type{
		get{ return _type;}
		set{ _type = value;}
	}
	public GameObject[] Plains{
		get{ return _plains; }
		set{ _plains = value; }
	}

}
