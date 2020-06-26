using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// this file makes a block mesh 
/// </summary>
public class fourquad : MonoBehaviour {
	public Material _material;
	Mesh _mesh;
	Chunk _c;
	MaterialPropertyBlock _mBlock;
	// Use this for initialization
	void Start () {
		MaterialPropertyBlock _mBlock;
		_mesh = MakeCubes ();
		//_c = new Chunk (new Vector3 (0, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
		Graphics.DrawMesh (_mesh, Vector3.zero, Quaternion.identity, _material, 0, null, 0, _mBlock, true, true);
		//_c.Draw();
	}
	Mesh MakeCubes(){
		int nrOfCubes = 1;//(int)(Chunk._size.x * Chunk._size.y * Chunk._size.z);
		int trisPerSide = 6;
		Mesh mesh = new Mesh();
										//number of sides of cube * number of vertices per side * number of cubes
		Vector3[] vertices = new Vector3[6*4*nrOfCubes];
		Vector2[] uvs = new Vector2[vertices.Length];
		//number of sides of cube * number of cubes * number of triangles per side
		int[] tris = new int[(6*nrOfCubes)*trisPerSide];
		//tri pattern 0 1 2 || 3 2 1
		Vector3[] normals = new Vector3[vertices.Length];
		int index = 0;
		for (int i = 0; i < 1; i++) {
			for (int j = 0; j < 1; j++) {
				for (int k = 0; k < 1; k++) {
					BuildCube (ref vertices, ref normals, index, new Vector3(k*1, i*1, j*1));
					index++;
				}
			}
		}
		MakeUVs (ref uvs);
		MakeTris (ref tris);
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = tris;
		mesh.normals = normals;
		return mesh;
	}
	void BuildCube(ref Vector3[] verts, ref Vector3[] normals, int cubeNr, Vector3 pos){
		int nrOFSides = 6;
		for (int i = 0; i < nrOFSides; i++) {
			MakeQuadVert (ref verts, ref normals, (BlockSide)(i), i*4 +(4*nrOFSides*cubeNr), pos);
		}
	}
	//add 4 to index
	void MakeQuadVert(ref Vector3[] verts, ref Vector3[] normals, BlockSide bSide, int index, Vector3 offset){
		Vector3 s = new Vector3 (0.5f, 0.5f, 0.5f);//Block._size;
		if(bSide == BlockSide.BACK){
			verts [index] = new Vector3(-s.x, -s.y, -s.z) + offset;
			verts [index+1] = new Vector3(-s.x, s.y, -s.z) + offset;
			verts [index+2] = new Vector3(s.x, -s.y, -s.z) + offset;
			verts [index+3] = new Vector3(s.x, s.y, -s.z) + offset;
			for (int i = index; i < index + 4; i++) {
				normals [i] = new Vector3 (0, 0, -1);
			}
		}
		if(bSide == BlockSide.FORWARD){
			verts [index] = new Vector3(s.x, -s.y, s.z) + offset;
			verts [index+1] = new Vector3(s.x, s.y, s.z) + offset;
			verts [index+2] = new Vector3(-s.x, -s.y, s.z) + offset;
			verts [index+3] = new Vector3(-s.x, s.y, s.z) + offset;
			for (int i = index; i < index + 4; i++) {
				normals [i] = new Vector3 (0, 0, 1);
			}
		}
		if(bSide == BlockSide.LEFT){
			verts [index] = new Vector3(-s.x, -s.y, s.z) + offset;
			verts [index+1] = new Vector3(-s.x, s.y, s.z) + offset;
			verts [index+2] = new Vector3(-s.x, -s.y, -s.z) + offset;
			verts [index+3] = new Vector3(-s.x, s.y, -s.z) + offset;
			for (int i = index; i < index + 4; i++) {
				normals [i] = new Vector3 (-1, 0, 0);
			}
		}
		if(bSide == BlockSide.RIGHT){
			verts [index] = new Vector3(s.x, -s.y, -s.z) + offset;
			verts [index+1] = new Vector3(s.x, s.y, -s.z) + offset;
			verts [index+2] = new Vector3(s.x, -s.y, s.z) + offset;
			verts [index+3] = new Vector3(s.x, s.y, s.z) + offset;
			for (int i = index; i < index + 4; i++) {
				normals [i] = new Vector3 (1, 0, 0);
			}
		}
		if(bSide == BlockSide.TOP){
			verts [index] = new Vector3(-s.x, s.y, -s.z) + offset;
			verts [index+1] = new Vector3(-s.x, s.y, s.z) + offset;
			verts [index+2] = new Vector3(s.x, s.y, -s.z) + offset;
			verts [index+3] = new Vector3(s.x, s.y, s.z) + offset;
			for (int i = index; i < index + 4; i++) {
				normals [i] = new Vector3 (0, 1, 0);
			}
		}
		if(bSide == BlockSide.BOTTOM){
			verts [index] = new Vector3(s.x, -s.y, -s.z) + offset;
			verts [index+1] = new Vector3(s.x, -s.y, s.z) + offset;
			verts [index+2] = new Vector3(-s.x, -s.y, -s.z) + offset;
			verts [index+3] = new Vector3(-s.x, -s.y, s.z) + offset;
			for (int i = index; i < index + 4; i++) {
				normals [i] = new Vector3 (0, -1, 0);
			}
		}
	}
	void MakeUVs( ref Vector2[] uvs){
		for (int i = 0; i < uvs.Length; i += 4) {
			uvs [i] = new Vector2 (0.5f, 0);
			uvs [i+1] = new Vector2 (0.5f, 1);
			uvs [i+2] = new Vector2 (1, 0);
			uvs [i+3] = new Vector2 (1, 1);
		}
	}
	void MakeTris(ref int[] tris){
		for (int i = 0; i < tris.Length; i += 6) {
			int faceNr = i / 6;
			tris [i] = 0 + faceNr * 4;
			tris [i + 1] = 1 + faceNr * 4;
			tris [i + 2] = 2 + faceNr * 4;

			tris [i + 3] = 3 + faceNr * 4;
			tris [i + 4] = 2 + faceNr * 4;
			tris [i + 5] = 1 + faceNr * 4;
		}
	}
}
