using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public enum BlockType
{

	ROCK = 0,
	DIRT,
	DIRT_GRASS,
	TREE_WOOD,
	FROZEN_DIRT,
	FROZEN_ICE_DIRT,
	AIR,
	GRASS


};
public class BlockFactory {
	private static BlockFactory instance;
	GameObject[] _plainTemplates;
	Material[] _materials;
	Mesh _planeTemplate;
	private BlockFactory() {
		_plainTemplates = new GameObject[6];

	}

	public void LoadMaterials(Material[] mats){
		_materials = mats;
	}
	
	public void Draw(Mesh m, Vector3 chunkpos){
		//Material mat;
		//Graphics.DrawMesh (m, Vector3.zero, Quaternion.identity, _materials[0], 0, null, 0, _mBlock, ShadowCastingMode.TwoSided);
		Graphics.DrawMesh(m, chunkpos, Quaternion.identity, _materials[0], 0);
	}
	public static BlockFactory Instance
	{
		get{
			if(instance == null){
				instance = new BlockFactory();
			}
			return instance;
		}
	}

	public bool[] SetBlockType(ref Block b){
		
		Vector3 pos = b.Position;
		//Vector3 worldSize = new Vector3 (Chunk._size.x * World._worldSize.x, Chunk._size.y * World._worldSize.y, Chunk._size.z * World._worldSize.z);
		b.Type = GetBlockType (pos);
		if (b.Type == BlockType.AIR) {
			return null;
		}
		//l r f b t b
		bool[] nBools = new bool[]{
			(GetBlockType(pos + new Vector3(-Block._size.x, 0, 0)) != BlockType.AIR ),//left
			(GetBlockType(pos + new Vector3(Block._size.x, 0, 0)) != BlockType.AIR ),//right
			(GetBlockType(pos + new Vector3(0, 0, Block._size.z)) != BlockType.AIR ),//forward
			(GetBlockType(pos + new Vector3(0, 0, -Block._size.z)) != BlockType.AIR ),//back
			(GetBlockType(pos + new Vector3(0, Block._size.y, 0)) != BlockType.AIR ),//top
			(GetBlockType(pos + new Vector3(0, -Block._size.y, 0)) != BlockType.AIR ),//bottom
		};
		return nBools;
	}
	BlockType GetBlockType(Vector3 pos){
		return ProceduralTerrainGenerator.GetBlockType (pos);
		//BlockType bt;
		//b.Type = BlockType.DIRT;
		/*if(pos.y<1)
			bt = BlockType.DIRT;
		else
			bt = BlockType.AIR;
*/
	}
	public void MakeCubeVertsNorms(ref Vector3[] verts, ref Vector3[] normals, int cubeNr, Vector3 pos){
		int nrOFSides = 6;
		for (int i = 0; i < nrOFSides; i++) {
			MakeQuadVert (ref verts, ref normals, (BlockSide)(i), i*4 +(4*nrOFSides*cubeNr), pos);
		}
	}
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
	public void CreateTris(ref List<int> tris, bool[] nBools, int blockIndex){
		
		if (nBools == null)
			return;
		for(int i =0; i < nBools.Length; i++){
			//(BlockSide)(i)
			if(!nBools[i]){
				int offset = i * 4 + blockIndex * 24;
				tris.AddRange(new int[]{
					0 + offset,
					1 + offset,
					2 + offset,

					3 + offset,
					2 + offset,
					1 + offset 
				});
			}
		}
	}
	/// <summary>
	/// this has to do with how the texture is sampled in the shader:D
	/// allowing you to keep all the textures in one file
	/// </summary>
	/// <param name="uvs"></param>
	/// <param name="b"></param>
	/// <param name="index"></param>
	public void SetUvs(ref Vector2[] uvs, Block b, int index){
		//4 uvs per plane
		/*for (int i = 0; i < uvs.Length; i += 4) {
			uvs [i] = new Vector2 (0.5f, 0);
			uvs [i+1] = new Vector2 (0.5f, 1);
			uvs [i+2] = new Vector2 (1, 0);
			uvs [i+3] = new Vector2 (1, 1);
		}*/
		int i = index * 24;
		float typeOffsetX = 0.25f;
		float typeOffsetY = 0.5f;
		int type = (int)(b.Type) -1;
		for (; i < (index+1)*24; i+=4) {
			//BlockType.AIR
			BlockSide side = (BlockSide)((int)(i/4)%6);
			int y = 0;
			if (side == BlockSide.BOTTOM || side == BlockSide.TOP || b.Type != BlockType.GRASS) {
				y = 1;
			}
			uvs [i] = new Vector2 (0 + (typeOffsetX*type) , typeOffsetY * y );
			uvs [i + 1] = new Vector2 (0  + (typeOffsetX*type) , typeOffsetY + typeOffsetY * y);
			uvs [i + 2] = new Vector2 (typeOffsetX  + (typeOffsetX*type) , typeOffsetY * y);
			uvs [i + 3] = new Vector2 (typeOffsetX  + (typeOffsetX*type) , typeOffsetY + typeOffsetY * y);
		}
	}
}
