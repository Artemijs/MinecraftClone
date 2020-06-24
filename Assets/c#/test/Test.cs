using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
	int _width = 256;
	int _height = 256;

	public float _xOffset = 0;
	public float _yOffset = 0;

	public float _scale = 1;
	public float[] _scales;
	public float[] _colorOffsets;

	public bool _makeTerrain;
	TerrainGenerator _terrainGen;
	// Use this for initialization
	void Start () {
		_terrainGen = GameObject.Find ("terrain").GetComponent<TerrainGenerator> ();
		Renderer rend = GetComponent<Renderer> ();
		Texture2D tex = GenerateTexture();
		rend.material.mainTexture =  (tex);
		if (!_makeTerrain)
			return;
		//GenerateTerrain1 (tex);
		_terrainGen.Begin(tex);
		//Debug.Log (GameObject.Find ("terrain").transform.childCount);
	}
	
	// Update is called once per frame
	void Update () {
		if (_makeTerrain)
			return;
		Renderer rend = GetComponent<Renderer> ();
		rend.material.mainTexture = GenerateTexture ();

	}
	Texture2D GenerateTexture(){
		
		Texture2D tex = new Texture2D (_width, _height);
		for (int x = 0; x < _width; x++) {
			for (int y = 0; y < _height; y++) {
				Color color = CalculateColor (x, y);
				tex.SetPixel (x, y, color);
			}	
		}
		tex.Apply ();
		return tex;
	}
	Color CalculateColor(int x, int y){
		float s1 = _scale;
		int itr = _scales.Length;
		Color col = new Color (0.0f, 0.0f, 0.0f);
		for (int i = 0; i < itr; i++) {
			s1 = _scales [i];
			float xCord = (float)x/_width * s1 + _xOffset;
			float yCord = (float)y/_height * s1 + _yOffset;
			float sample = Mathf.PerlinNoise (xCord, yCord)*_colorOffsets[i];
			//s1 *= i + 1;
			//Color col = new Color (sample, sample, sample);
			col.r += sample;
			col.g += sample;
			col.b += sample;
		}
		col.r/=itr;
		col.g/=itr;
		col.b/=itr;
		/*float xCord = (float)x/_width * _scale + _xOffset;
		float yCord = (float)y/_height * _scale + _yOffset;
		float sample = Mathf.PerlinNoise (xCord, yCord);
		Color col = new Color (sample, sample, sample);*/
		return col;
	}
	public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
	{
		byte[] _bytes =_texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(_fullPath, _bytes);
		Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + _fullPath);
	}
	//i must isolate this




	/*
	issues 
			holes in the ground
			editor lagg
			height variation too small
	*/
}
