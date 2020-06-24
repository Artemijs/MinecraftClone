using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
	public float _maxHeight = 50;
	public float _maxSquares = 50;
	float _bSize = 0;
	public GameObject _cube;
	bool _next;
	Texture2D _texture;
	public int _maxSize;
	int _cX = 0;
	int _cY = 0;
	bool _finished = false;
	// Use this for initialization
	void Start () {
		_next = false;
		_bSize = _maxHeight/_maxSquares;
		//_bSize = 1;
		_cube.transform.localScale = new Vector3(1, _bSize, 1);

	}
	public void Begin(Texture2D tex){
		_texture = tex;
		while (!_finished) {
			GenOne ();
		}
	
	}
	// Update is called once per frame
	void Update () {
		/*if (_finished)
			return;
		_next = true;
		if (Input.GetKey (KeyCode.A)) {
			_next = true;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			_next = true;
		}
		if (_next) {
			GenOne ();
		}
		if (_next)
			_next = false;*/
	}
	private void GenOne(){
		if (_cY >= _maxSize) {
			_cX++;
			_cY = 0;
		}
		if (_cX >= _maxSize) {
			_finished = true;
			return;
		}

		//do the thing
		CreateCube (new Vector3 (_cX, GetHeight(_texture.GetPixel(_cX, _cY).r), _cY), 0);

		_cY++;

	}
	private void CreateCubes(Vector3 pos, float offX){
		int nrOfCubes = (int)(pos.y/_bSize);
		Vector3 position = pos;
		pos.y = 0;
		for (int i = 0; i < nrOfCubes; i++) {
			pos.y = i * _bSize;
			CreateCube (pos, offX);
		}
	}

	private void CreateCube(Vector3 pos, float offX){
		GameObject terrain = GameObject.Find("terrain");
		GameObject go = GameObject.Instantiate (_cube);
		go.transform.position = new Vector3 (pos.x , pos.y, pos.z );
		go.transform.parent = terrain.transform;
	}
	float GetHeight(float pixVal){


		int mV = (int)((pixVal * _maxHeight) / _bSize);
		return (float)(mV*_bSize);
	}
	public void GenerateTerrain(Texture2D hmap){

		for(int x = 0; x < 100; x++){
			for(int y = 0; y < 100; y++){

				CreateCube (new Vector3 (x, GetHeight(hmap.GetPixel(x, y).r), y), 0);
				//GameObject go = GameObject.Instantiate (_cube);
				//go.transform.position = new Vector3 (x-100, (float)hmap.GetPixel(x, y).r*100 -75, y-100);
				//go.transform.position = ;
			}	
		}
	}
	public void GenerateTerrain1(Texture2D hmap){
		//last y
		//if distance from last y and this y is < bSize
		//add more cubes
		float offset =0;
		for(int x = 0; x < 10; x++){
			float lastHX = -1;
			for(int y = 0; y < 10; y++){

				float h = GetHeight(hmap.GetPixel(x, y).r);

				if(lastHX == -1)lastHX = h;
				float dist = h - lastHX; //number of squares to add

				int nrOfNewCubes = (int)(dist/_bSize);
				for(int i =0; i < Mathf.Abs(nrOfNewCubes); i++){
					//GameObject goTemp = GameObject.Instantiate (_cube);
					//goTemp.transform.position = new Vector3 (x - 100,  (lastH + _bSize*(i+1))-75, y-100);
					int offdir= 1;
					if (nrOfNewCubes < 0)
						offdir = -1;
					CreateCube (new Vector3 (x, (lastHX + (_bSize*(i+1) * offdir)), y), offset);
				}

				CreateCube (new Vector3 (x, h, y), offset);
			}	
		}
		offset = 11;
		for(int x = 0; x < 10; x++){
			float lastHX = -1;
			for(int y = 0; y < 10; y++){

				float h = GetHeight(hmap.GetPixel(x, y).r);

				if(lastHX == -1)lastHX = h;
				float dist = h - lastHX; //number of squares to add

				int nrOfNewCubes = (int)(dist/_bSize);
				for(int i =0; i < Mathf.Abs(nrOfNewCubes); i++){
					//GameObject goTemp = GameObject.Instantiate (_cube);
					//goTemp.transform.position = new Vector3 (x - 100,  (lastH + _bSize*(i+1))-75, y-100);
					int offdir= 1;
					if (nrOfNewCubes < 0)
						offdir = -1;
					//CreateCube (new Vector3 (x, (lastHX + (_bSize*(i+1) * offdir)), y), offset);
				}

				CreateCube (new Vector3 (x, h, y), offset);
			}	
		}
	}
}
