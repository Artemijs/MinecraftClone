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
		/*_plains = new Plane[6];
		for (int i = 0; i < _plains.Length; i++) {
			_plains [i] = new Plane ();
		}*/
	}
	public void SetLiveArray(){

		/*int[] arr = new int[]{-1, -1, -1, -1, -1, -1 };
		int cunt = 0;
		int i = 0;
		for (; i < _plains.Length; i++) {
			if (_plains [i] != null) {
				arr [cunt] = i;
				cunt++;

			}
		}
		i = 0;
		_livePlanes = new Plane[cunt];
		while(cunt > 0){
			cunt--;
			_livePlanes [i] =_plains[ arr [i] ];
			i++;
		}*/


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
