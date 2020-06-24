using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockType
{
	AIR=0,
	ROCK,
	GRASS,
	DIRT
};
public class BlockFactory {
	private static BlockFactory instance;
	GameObject[] _plainTemplates;
	Material[] _materials;

	private BlockFactory() {
		_plainTemplates = new GameObject[6];

	}

	public void LoadMaterials(Material[] mats){
		_materials = mats;
	}
	public void LoadPlains(GameObject[] plains){
		_plainTemplates = plains;
	}
	public void SetBlockMesh(ref Block b, ref Block[] neighbors){
		//GameObject[] bPlains = new GameObject[6];
		if(b.Type == BlockType.AIR) return;
		GameObject[] bPlains = b.Plains;
		for (int i = 0; i < neighbors.Length; i++) {
			if (neighbors [i] == null) {
				//must instantiate this
				//bPlains [i] = _plainTemplates [i];
				bPlains [i] = GameObject.Instantiate(_plainTemplates[i], b.Parent);
				continue;
			}
			if (neighbors [i].Type == BlockType.AIR) {
				bPlains [i] = GameObject.Instantiate(_plainTemplates[i], b.Parent);
			} else
				bPlains[i] = null;
		}
	}
	public void SetBlockTexture(ref Block b){
		if (b.Type == BlockType.AIR)
			return;
		GameObject[] plains = b.Plains;
		for (int i = 0; i < plains.Length; i++) {
			if (plains [i] != null) {
				plains [i].GetComponent<Renderer> ().material = _materials [(i + (((int)(b.Type) - 1) * 6))];

			}
		}
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
	/*turns out grass is only a top layer of dirt*/
	public void SetBlockType(ref Block b, Vector3 chunkpos){
		Vector3 pos = b.Position + chunkpos;
		Vector3 worldSize = new Vector3 (Chunk._size.x * World._worldSize.x, Chunk._size.y * World._worldSize.y, Chunk._size.z * World._worldSize.z);
		/*if(pos.y<Chunk._size.y)
			b.Type = BlockType.DIRT;
		else
			b.Type = BlockType.AIR;*/


		float h = GetBaseHeight (pos, worldSize);

		float grassH = h * 0.75f;//(Chunk._size.y*0.25f);
		float rockH = h * 0.5f;//(Chunk._size.y*0.25f);
		if (pos.y >= grassH && pos.y < h) { //h && b.Position.y > grassH) {
			b.Type = BlockType.GRASS;
		} else if (pos.y < grassH && pos.y >= rockH) {
			b.Type = BlockType.DIRT;
		} else if (pos.y < rockH) {
			b.Type = BlockType.ROCK;
		}
		else {
			b.Type = BlockType.AIR;
		}
	}
	private float GetBaseHeight(Vector3 pos, Vector3 worldSize){
		/*float h = (Mathf.PerlinNoise (pos.x / (worldSize.x), pos.z / (worldSize.y)) +
			Mathf.PerlinNoise (pos.x / (worldSize.x*0.5f), pos.z / (worldSize.y*0.5f)) +
			Mathf.PerlinNoise (pos.x / (worldSize.x*0.25f), pos.z / (worldSize.y*0.25f)))/3;*/
		//float h = Mathf.PerlinNoise (pos.x / (worldSize.x*2), pos.z / (worldSize.y*2));
		float h = Mathf.PerlinNoise (pos.x / (worldSize.x*2), pos.z / (worldSize.y*2)) * worldSize.y;
		float h2 = Mathf.PerlinNoise (pos.x / (worldSize.x), pos.z / (worldSize.y)) * (worldSize.y*0.5f);
		float h3 = Mathf.PerlinNoise (pos.x / (worldSize.x*0.5f), pos.z / (worldSize.y*0.5f)) * (worldSize.y*0.1f);
		return (h+h2+h3)/2;
		//h = Mathf.PerlinNoise (pos.x / (worldSize.z), pos.y / (worldSize.y)) * worldSize.y;
		//return h;
	}
}
