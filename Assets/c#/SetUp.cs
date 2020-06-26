using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// setup of the v0.1, a version that used plain objects 
/// </summary>
public class SetUp : MonoBehaviour {
	public GameObject[] _plains;
	public Material[] _materials;
	public Vector3 _boxSize = new Vector3 (1, 1, 1);
	void Awake(){
		//BlockFactory.Instance;
		//load block plains
		LoadBlockPlains();
		//load block materials
		LoadBlockTextures();
		Block._size = _boxSize;
		ProceduralTerrainGenerator.SetUp ();
	}
	void LoadBlockPlains(){
		//change position and _boxSize
		/*GameObject f = _plains [(int)BlockSide.FORWARD];
		Vector3 pos = f.transform.position;
		Vector3 scale = f.transform.localScale;
		pos.z = _boxSize.z * 0.5f;
		scale.y = _boxSize.y;
		scale.x = _boxSize.x;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = _plains [(int)BlockSide.BACK];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.z = _boxSize.z * -0.5f;
		scale.y = _boxSize.y;
		scale.x = _boxSize.x;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = _plains [(int)BlockSide.LEFT];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.x = _boxSize.x * -0.5f;
		scale.x = _boxSize.z;
		scale.y = _boxSize.y;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = _plains [(int)BlockSide.RIGHT];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.x = _boxSize.x * 0.5f;
		scale.x = _boxSize.z;
		scale.y = _boxSize.y;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = _plains [(int)BlockSide.TOP];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.y = _boxSize.y * 0.5f;
		scale.y = _boxSize.z;
		scale.x = _boxSize.x;
		f.transform.position = pos;
		f.transform.localScale = scale;

		f = _plains [(int)BlockSide.BOTTOM];
		pos = f.transform.position;
		scale = f.transform.localScale;
		pos.y = _boxSize.y * -0.5f;
		scale.y = _boxSize.z;
		scale.x = _boxSize.x;
		f.transform.position = pos;
		f.transform.localScale = scale;*/

		BlockFactory.Instance.LoadPlains (_plains);
	}
	void LoadBlockTextures(){
		BlockFactory.Instance.LoadMaterials (_materials);
	}
}
