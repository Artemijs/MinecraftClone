using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	ChunkGenerator _chunkGen;
	Chunk _chunk;
	public Material mat;
	BlockType _selectedType;
	TargetBlockData _tbd;
	public GameObject _blockBreakPlane;
	// Start is called before the first frame update
	void Start()
    {

		_tbd._deleting = false;
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
		if (_tbd._deleting) {
			DeleteBlock();
		}
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
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			_selectedType = BlockType.ROCK;
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
	public void StartDeletingBlock(Vector3 pos, Vector3 normal)
	{
		//if (_tbd._deleting) return;
		pos -= new Vector3(0.01f * normal.x, 0.01f * normal.y, 0.01f * normal.z);
		BlockSide bs = ChunkGenerator.GetBlockSide(normal);
		Vector3Int intPos = new Vector3Int((int)(pos.x), (int)(pos.y), (int)(pos.z));
		if (intPos != _tbd._position)
		{
			_tbd._position = intPos;
			CancelDeleting();
		}
		if (_tbd._targetNormal != normal) {
			_tbd._targetNormal = normal;
			CancelDeleting();
		}
		if (_tbd._deleting) return;
		BlockData block = _chunk.GetBlockfromPos(intPos);
		_tbd._deleting = true;
		_tbd._position = intPos;
		_tbd._toughness = ChunkGenerator.GetToughnessFromType(block._type);
		_tbd._type = block._type;
		Pair<Vector3, Vector3> bpd;
		GetBreakPlaneData(out bpd, bs, intPos);
		_tbd._targetNormal = normal;
		_tbd._breakPlanePos = bpd.one;
		_tbd._breakPlaneAngle = bpd.two;
		_blockBreakPlane.transform.position = bpd.one;
		_blockBreakPlane.transform.eulerAngles = bpd.two;
		_blockBreakPlane.SetActive(true);
		_blockBreakPlane.GetComponent<TextureAnimation>().SetDuration(_tbd._toughness*0.9f);
		_blockBreakPlane.GetComponent<TextureAnimation>().Play();
	}
	void DeleteBlock() {
		Debug.Log("DELETING");
		_tbd._toughness -= Time.deltaTime;
		if (_tbd._toughness <= 0) {
			_chunk.DeleteCube(_tbd._position);
			_tbd._deleting = false;
		}
	}
	void GetBreakPlaneData(out Pair<Vector3, Vector3> bpData, BlockSide bs, Vector3Int pos) {
		bpData = new Pair<Vector3, Vector3>();
		if (bs == BlockSide.LEFT) {
			bpData.one = new Vector3(pos.x - 0.01f, pos.y +0.5f, pos.z+0.5f);
			bpData.two = new Vector3(0, 0, 90);
		}
		else if (bs == BlockSide.RIGHT)
		{
			bpData.one = new Vector3(pos.x +1.01f , pos.y + 0.5f, pos.z + 0.5f);
			bpData.two = new Vector3(0, 0, -90);
		}
		else if (bs == BlockSide.FORWARD)
		{
			bpData.one = new Vector3(pos.x +0.5f, pos.y +0.5f, pos.z + 1);
			bpData.two = new Vector3(90, 0, 0);
		}
		else if (bs == BlockSide.BACK)
		{
			bpData.one = new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z - 0.01f);
			bpData.two = new Vector3(-90, 0, 0);
		}
		else if (bs == BlockSide.TOP)
		{
			bpData.one = new Vector3(pos.x+0.5f, pos.y + 1.01f, pos.z+0.5f);
			bpData.two = new Vector3(0, 0, 0);
		}
		else if (bs == BlockSide.BOTTOM)
		{
			bpData.one = new Vector3(pos.x+0.5f, pos.y-0.01f, pos.z + 0.5f);
			bpData.two = new Vector3(180, 0, 0);
		}

	}
	public void CancelDeleting() {
		_tbd._deleting = false;
		_blockBreakPlane.SetActive(false);
	}
}
public struct TargetBlockData {
	public bool _deleting;
	public float _toughness;
	public Vector3Int _position;
	public BlockType _type;
	public Vector3 _breakPlaneAngle;
	public Vector3 _breakPlanePos;
	public Vector3 _targetNormal;
};
