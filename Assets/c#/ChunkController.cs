using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController: MonoBehaviour {
	Chunk[,,] _chunks;
	Vector3Int _cSize; //number of chunks in the _chunks array
	Vector3 _cDistance;
	Vector3 _origin;
	ChunkGenerator _chunkGen;
	TargetBlockData _tbd;
	Chunk _currentChunk;
	public GameObject _blockBreakPlane;
	public Material _blockMaterial;
	//I PITY THE FOOL THAT READS THIS
	//U FUUKING KNEW ID HAV E TO READ THIS U CUNT 

	private void Start()
	{
		_chunkGen = GetComponent<ChunkGenerator>();


		_tbd._deleting = false;
		_cSize = new Vector3Int(2, 5, 2);
		_chunks = new Chunk[_cSize.x, _cSize.y, _cSize.z];

		for (int x = 0; x < _cSize.x; x++)
		{
			for (int y = 0; y < _cSize.y; y++)
			{
				for (int z = 0; z < _cSize.z; z++)
				{
					_chunkGen.MakeChunk(out _chunks[x, y, z], new Vector3(x*Chunk._size.x, y* Chunk._size.y, z* Chunk._size.z));
				}
			}
		}
		SetCurrentChunk(Vector3.zero);
	}
	public void SetCurrentChunk(Vector3 playerPos) {
		//int index = (int)(pos.z + pos.y * Chunk._size.y + Chunk._size.x * Chunk._size.x * pos.x);
		Vector3Int chunkPos = GetIdFromPos(playerPos);
		Debug.Log(chunkPos);
		if (_currentChunk != _chunks[chunkPos.x, chunkPos.y, chunkPos.z]) {
			_currentChunk = _chunks[chunkPos.x, chunkPos.y, chunkPos.z];
		}
	}
	public Vector3Int GetIdFromPos(Vector3Int blockPos) {
		Vector3Int chunkPos = new Vector3Int();
		chunkPos.x = (blockPos.x / Chunk._size.x);
		chunkPos.y = (blockPos.y / Chunk._size.y);
		chunkPos.z = (blockPos.z / Chunk._size.z);
		return chunkPos;
	}
	public Vector3Int GetIdFromPos(Vector3 blockPos)
	{
		Vector3Int chunkPos = new Vector3Int();
		chunkPos.x = (int)(blockPos.x / Chunk._size.x);
		chunkPos.y = (int)(blockPos.y / Chunk._size.y);
		chunkPos.z = (int)(blockPos.z / Chunk._size.z);
		return chunkPos;
	}
	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.G)) {
			DeleteTop();
		}
		if (_tbd._deleting)
		{
			DeleteBlock();
		}

		Draw();
	}
	public void DeleteTop() {
		for (int x = 0; x < _cSize.x; x++)
		{
			for (int z = 0; z < _cSize.z; z++)
			{
				_tbd._position = new Vector3Int(x, 2, z);
				DeleteBlock();
			}
		}
	}
	public void CreateBlock(Vector3 pos, Vector3 normal, BlockType bType)
	{
		//get block pos
		pos -= new Vector3(0.01f * normal.x, 0.01f * normal.y, 0.01f * normal.z);
		BlockSide bs = ChunkGenerator.GetBlockSide(normal);
		pos.x = (int)(pos.x);
		pos.y = (int)(pos.y);
		pos.z = (int)(pos.z);
		pos += (normal);

		Vector3Int blockPos = new Vector3Int((int)(pos.x), (int)(pos.y), (int)(pos.z));
		if (CheckInCurrentChunk(blockPos))
			_currentChunk.CreateCube(blockPos, bType);
		else
		{
			Vector3Int cPos = GetIdFromPos(blockPos);

			_chunks[cPos.x, cPos.y, cPos.z].CreateCube(blockPos, bType);

		}	
	}
	public bool CheckInCurrentChunk(Vector3Int pos) {
		Vector3Int pso = _currentChunk.Position;

		return (pos.x >= pso.x && pos.x < pso.x + Chunk._size.x &&
			pos.y >= pso.y && pos.y < pso.y + Chunk._size.y &&
			pos.z >= pso.z && pos.z < pso.z + Chunk._size.z);
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
		if (_tbd._targetNormal != normal)
		{
			_tbd._targetNormal = normal;
			CancelDeleting();
		}
		if (_tbd._deleting) return;
		BlockData block = _currentChunk.GetBlockfromPos(intPos);

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
		_blockBreakPlane.GetComponent<TextureAnimation>().SetDuration(_tbd._toughness * 0.9f);
		_blockBreakPlane.GetComponent<TextureAnimation>().Play();
	}
	void DeleteBlockInstantly()
	{
		
		_currentChunk.DeleteCube(_tbd._position);
		
	}
	void DeleteBlock()
	{
		Debug.Log("DELETING");
		_tbd._toughness -= Time.deltaTime;
		if (_tbd._toughness <= 0)
		{
			_currentChunk.DeleteCube(_tbd._position);
			_tbd._deleting = false;
		}
	}
	void GetBreakPlaneData(out Pair<Vector3, Vector3> bpData, BlockSide bs, Vector3Int pos)
	{
		bpData = new Pair<Vector3, Vector3>();
		if (bs == BlockSide.LEFT)
		{
			bpData.one = new Vector3(pos.x - 0.01f, pos.y + 0.5f, pos.z + 0.5f);
			bpData.two = new Vector3(0, 0, 90);
		}
		else if (bs == BlockSide.RIGHT)
		{
			bpData.one = new Vector3(pos.x + 1.01f, pos.y + 0.5f, pos.z + 0.5f);
			bpData.two = new Vector3(0, 0, -90);
		}
		else if (bs == BlockSide.FORWARD)
		{
			bpData.one = new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 1);
			bpData.two = new Vector3(90, 0, 0);
		}
		else if (bs == BlockSide.BACK)
		{
			bpData.one = new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z - 0.01f);
			bpData.two = new Vector3(-90, 0, 0);
		}
		else if (bs == BlockSide.TOP)
		{
			bpData.one = new Vector3(pos.x + 0.5f, pos.y + 1.01f, pos.z + 0.5f);
			bpData.two = new Vector3(0, 0, 0);
		}
		else if (bs == BlockSide.BOTTOM)
		{
			bpData.one = new Vector3(pos.x + 0.5f, pos.y - 0.01f, pos.z + 0.5f);
			bpData.two = new Vector3(180, 0, 0);
		}

	}
	public void CancelDeleting()
	{
		_tbd._deleting = false;
		_blockBreakPlane.SetActive(false);
	}
	public void Draw(){
		for (int x = 0; x < _cSize.x; x++) {
			for (int y = 0; y < _cSize.y; y++) {
				for (int z = 0; z < _cSize.z; z++) {
					_chunks [x, y, z].Draw (_blockMaterial);
				}
			}
		}
	}



}
public struct TargetBlockData
{
	public bool _deleting;
	public float _toughness;
	public Vector3Int _position;
	public BlockType _type;
	public Vector3 _breakPlaneAngle;
	public Vector3 _breakPlanePos;
	public Vector3 _targetNormal;
};