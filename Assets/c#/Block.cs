using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
	public static Vector3 _size = new Vector3 (2, 1, 2);
	BlockType _type;
	//Plane[] _plains;
	//Plane[] _livePlanes;
	Vector3 _position;
	public Block(Vector3 pos){
		_position = pos;
		
	}
	
	public Vector3 Position{
		get{ return _position; }
		set{ _position = value; }
	}
	public BlockType Type{
		get{ return _type;}
		set{ _type = value;}
	}
	public Plane[] Plains{
		get{ return null; }

	}
	public Plane[] LivePlanes{
		get{ return null; }
	}

}
