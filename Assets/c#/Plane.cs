using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this file was used in the 0.1 version where cubes were made up of plane objects
/// </summary>
public class Plane {
	Mesh _mesh;
	Vector3 _position;
	Quaternion _rotation;
	Vector3 _scale;
	public Plane(){
		_position = Vector3.zero;
		_rotation = Quaternion.identity;
		_scale = Vector3.one;
	}
	public Mesh Mesh{
		get{ return _mesh; }
		set{ _mesh = value;}
	}
	public Vector3 Position{
		get{ return _position; }
		set{ _position = value;}
	}
	public Quaternion Rotation{
		get{ return _rotation; }
		set{ _rotation = value;}
	}
	public Vector3 Scale{
		get{ return _scale; }
		set{ _scale = value;}
	}
	public Matrix4x4 Transform{
		get{ return Matrix4x4.TRS (_position, _rotation, _scale);}
	}

	/*

	//pos
	Vector3 position = m.GetColumn(3);
	//rot 
	Quaternion rotation = Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
	//scale 
	Vector3 size = new Vector3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
	//b.Draw (_material);

	*/

}
