using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	ChunkGenerator _chunkGen;
	Chunk _chunk;
	public Material mat;
	BlockType _selectedType;
	// Start is called before the first frame update
	void Start()
    {
		
		_chunkGen = GetComponent<ChunkGenerator>();
		//_chunkGen.MakeChunk(out _chunk, new Vector3(10, 0 ,0));
		_chunkGen.MakeChunk(out _chunk, Vector3.zero);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
		_selectedType = BlockType.DIRT;

	}

    // Update is called once per frame
    void Update()
    {
		_chunk.Draw(mat);
		HandleInput();

	}
	void HandleInput() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			_selectedType = BlockType.DIRT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			_selectedType = BlockType.DIRT_GRASS;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			_selectedType = BlockType.FROZEN_DIRT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			_selectedType = BlockType.FROZEN_ICE_DIRT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			_selectedType = BlockType.TREE_WOOD;
		}
		
	}
	public void CreateBlock(Vector3 pos, Vector3 normal) {
		//get block pos
		pos -= new Vector3(0.01f*normal.x, 0.01f*normal.y, 0.01f*normal.z) ;
		BlockSide bs = ChunkGenerator.GetBlockSide(normal);
		pos.x = (int)(pos.x);
		pos.y = (int)(pos.y);
		pos.z = (int)(pos.z);
		pos += (normal);
		//pos = new Vector3Int((int)(pos.y), (int)(pos.y), (int)(pos.z));
		Vector3Int intPos = new Vector3Int((int)(pos.x), (int)(pos.y), (int)(pos.z));
		
		//intPos += normal;
		//Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(pos.x), (int)(pos.y), Mathf.RoundToInt(pos.z));
		Debug.Log("original " + pos  +" side "+bs.ToString());
		Debug.Log("int " + intPos);
		_chunk.CreateCube(intPos, _selectedType);
	}
	public void DeleteBlock(Vector3 pos, Vector3 normal)
	{
		//get block pos
		pos -= new Vector3(0.01f * normal.x, 0.01f * normal.y, 0.01f * normal.z);
		BlockSide bs = ChunkGenerator.GetBlockSide(normal);
		pos.x = (int)(pos.x);
		pos.y = (int)(pos.y);
		pos.z = (int)(pos.z);
		//pos = new Vector3Int((int)(pos.y), (int)(pos.y), (int)(pos.z));
		Vector3Int intPos = new Vector3Int((int)(pos.x), (int)(pos.y), (int)(pos.z));
		_chunk.DeleteCube(intPos);
	}
}
