using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	ChunkGenerator _chunkGen;
	Chunk _chunk;
	public Material mat;
	// Start is called before the first frame update
	void Start()
    {
		
		_chunkGen = GetComponent<ChunkGenerator>();
		//_chunkGen.MakeChunk(out _chunk, new Vector3(10, 0 ,0));
		_chunkGen.MakeChunk(out _chunk, Vector3.zero);

	}

    // Update is called once per frame
    void Update()
    {
		_chunk.Draw(mat);
    }
	public void CreateBlock(Vector3 pos) {
		Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(pos.x), (int)(pos.y), Mathf.RoundToInt(pos.z));
		Debug.Log("original " + pos);
		Debug.Log("int " + intPos);
		intPos.y += 1;
		_chunk.CreateCube(intPos);
	}
	public void DeleteBlock(Vector3 pos)
	{
		Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(pos.x), (int)(pos.y), Mathf.RoundToInt(pos.z));
		//intPos.y -= 1;
		_chunk.DeleteCube(intPos);
	}
}
