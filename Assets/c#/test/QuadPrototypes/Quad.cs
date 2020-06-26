using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this file creates a quad mesh manually
/// </summary>
public class Quad : MonoBehaviour {
	public Material _material;
	Mesh _mesh;
	// Use this for initialization
	void Start () {
		_mesh = BuildQuad (1, 1);
		_mesh.name = "quad";

	}
	
	// Update is called once per frame
	void Update () {
		Graphics.DrawMesh (_mesh, Matrix4x4.TRS (Vector3.zero, Quaternion.identity, Vector3.one), _material, 0);
	}
	private Mesh BuildQuad(float w, float h){
		Mesh mesh = new Mesh ();
		Vector3[] newVertices = new Vector3[12];
		float halfH = h ;
		float halfW = w ;

		//setup verts
		newVertices[0] = new Vector3(-halfW, -halfH, 0);//bot left
		newVertices[1] = new Vector3(-halfW, halfH, 0);//top left
		newVertices[2] = new Vector3(halfW, -halfH, 0);//bot right
		newVertices[3] = new Vector3(halfW, halfH, 0);//top right

		newVertices[4] = new Vector3(-halfW + 1, -halfH, 0);//far bot lefd
		newVertices[5] = new Vector3(-halfW + 1, halfH, 0);//far top left
		newVertices[6] = new Vector3(halfW + 1, -halfH, 0);//far bot right
		newVertices[7] = new Vector3(halfW + 1, halfH, 0);//far top right

		newVertices[8] = new Vector3(-halfW + 2, -halfH, 0);//far bot lefd
		newVertices[9] = new Vector3(-halfW + 2, halfH, 0);//far top left
		newVertices[10] = new Vector3(halfW + 2, -halfH, 0);//far bot right
		newVertices[11] = new Vector3(halfW + 2, halfH, 0);//far top right

		//setup uvs
		Vector2[] newUVs = new Vector2[newVertices.Length];
		newUVs [0] = new Vector2 (0.5f, 0);
		newUVs [1] = new Vector2 (0.5f, 1);
		newUVs [2] = new Vector2 (1, 0);
		newUVs [3] = new Vector2 (1, 1);

		newUVs [8] = new Vector2 (0, 0);
		newUVs [9] = new Vector2 (0, 1);
		newUVs [10] = new Vector2 (0.5f, 0);
		newUVs [11] = new Vector2 (0.5f, 1);


		int[] newTris = new int[]{0, 1, 2,
								3, 2, 1,
								8, 9, 10,
								11, 10, 9  };

		Vector3[] newNormals = new Vector3[newVertices.Length];
		for (int i = 0; i < newNormals.Length; i++) {
			newNormals [i] = Vector3.forward;
		}

		//create quad
		mesh.vertices = newVertices;
		mesh.uv = newUVs;
		mesh.triangles = newTris;
		mesh.normals = newNormals;

		return mesh;

	}
}
