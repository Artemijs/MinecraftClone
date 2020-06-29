using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController: MonoBehaviour {

	Vector3 _cDistance;
	public static Vector3Int _origin;
	
	TargetBlockData _tbd;
	Chunk _currentChunk;
	Sector _currentSector;
	public GameObject _blockBreakPlane;
	public Material _blockMaterial;
	//I PITY THE FOOL THAT READS THIS
	//U FUUKING KNEW ID HAV E TO READ THIS U CUNT 
	List<Sector> _allSectors;
	private void Start()
	{
		
		Sector._chunkGen = GetComponent<ChunkGenerator>();
		_allSectors = new List<Sector>();

		_tbd._deleting = false;


		Vector3 playerPos = GameObject.Find("PlayerModel").transform.position;
		_origin = new Vector3Int(6, 0, 6);
		playerPos += _origin;
		
		
		GameObject.Find("PlayerModel").transform.position = playerPos;

		/*for (int x = 0; x < _cSize.x; x++)
		{
			for (int y = 0; y < _cSize.y; y++)
			{
				for (int z = 0; z < _cSize.z; z++)
				{
					_chunkGen.MakeChunk(out _chunks[x, y, z], new Vector3(x*Chunk._size.x, y* Chunk._size.y, z* Chunk._size.z)+ _origin);
				}
			}
		}*/
		CreateSectors();
		_currentSector = GetSectorFromPos(Sector.F2IntVector(playerPos));
		SetCurrentChunk(playerPos);
	}
	public void CreateSectors() {
		int sectorSize = 3;
		Sector s;
		//5per chunk 5 chunks in sector 1s = 5c
		for (int x = 0; x < sectorSize; x++)
		{
			
				for (int z = 0; z < sectorSize; z++)
				{

					s = new Sector(new Vector3Int(x, 0, z), _origin);
					_allSectors.Add(s);
					s.CreateChunk();
				}
			
		}
	}
	public void CreateChunks() {
		for (int i = 0; i < _allSectors.Count; i++) {
			_allSectors[i].CreateChunk();
		}
	}
	public void SetCurrentChunk(Vector3 playerPos) {
		Vector3Int pPos = Sector.F2IntVector(playerPos);
		Sector s = GetSectorFromPos(pPos);
		if (s != _currentSector)
		{
			_currentSector = s;
		}
		//int index = (int)(pos.z + pos.y * Chunk._size.y + Chunk._size.x * Chunk._size.x * pos.x);
		Chunk chunk = _currentSector.GetChunkFromPos(pPos);
		//Debug.Log(chunkPos);
		if (_currentChunk != chunk) {
			_currentChunk = chunk;
		}

	}
	public Sector GetSectorFromPos(Vector3Int pos) {
		for (int i = 0; i < _allSectors.Count; i++) {
			if (_allSectors[i].CheckInside(pos)) {
				return _allSectors[i];
			}
		}

		return null;
	}
	public Vector3Int GetIdFromPos(Vector3Int blockPos) {
		Vector3Int chunkPos = new Vector3Int();
		blockPos -= _origin;
		chunkPos.x = (blockPos.x / Chunk._size);
		chunkPos.y = (blockPos.y / Chunk._size);
		chunkPos.z = (blockPos.z / Chunk._size);
		return chunkPos;
	}
	public Vector3Int GetIdFromPos(Vector3 blockPos)
	{
		Vector3Int chunkPos = new Vector3Int();
		blockPos -= _origin;
		chunkPos.x = (int)(blockPos.x / Chunk._size);
		chunkPos.y = (int)(blockPos.y / Chunk._size);
		chunkPos.z = (int)(blockPos.z / Chunk._size);

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
		/*for (int x = 0; x < _cSize; x++)
		{
			for (int z = 0; z < _cSize; z++)
			{
				_tbd._position = new Vector3Int(x, 2, z);
				DeleteBlock();
			}
		}*/
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
			if (_currentSector.CheckInside(blockPos))
			{
				Chunk c = _currentSector.GetChunkFromPos(blockPos);
				c.CreateCube(blockPos, bType);
			}
			else {
				Sector s = GetSectorFromPos(blockPos);
				Chunk c = s.GetChunkFromPos(blockPos);
				c.CreateCube(blockPos, bType);
			}

		}	
	}
	public bool CheckInCurrentChunk(Vector3Int pos) {
		Vector3Int pso = _currentChunk.Position;

		return (pos.x >= pso.x && pos.x < pso.x + Chunk._size &&
			pos.y >= pso.y && pos.y < pso.y + Chunk._size &&
			pos.z >= pso.z && pos.z < pso.z + Chunk._size);
	}
	public void StartDeletingBlock(Vector3 pos, Vector3 normal)
	{
		//if (_tbd._deleting) return;
		pos -= new Vector3(0.01f * normal.x, 0.01f * normal.y, 0.01f * normal.z);
		BlockSide bs = ChunkGenerator.GetBlockSide(normal);
		Vector3Int intPos = new Vector3Int((int)(pos.x), (int)(pos.y), (int)(pos.z));
		//Vector3Int intPos = new Vector3Int(16, 2, 25);
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
		Debug.Log(intPos);
		Sector s = _currentSector;
		if (!_currentSector.CheckInside(intPos)) {
			s = GetSectorFromPos(intPos);
		}
		Chunk c = _currentChunk;
		if (!_currentChunk.CheckInside(intPos)) {
			c = s.GetChunkFromPos(intPos);
		}

		BlockData block = c.GetBlockfromPos(intPos);

		_tbd._deleting = true;
		_tbd._targetChunk = c;
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
		Sector s = _currentSector;
		if (!_currentSector.CheckInside(_tbd._position))
		{
			s = GetSectorFromPos(_tbd._position);
		}
		Chunk c = _currentChunk;
		if (!_currentChunk.CheckInside(_tbd._position))
		{
			_currentChunk = s.GetChunkFromPos(_tbd._position);
		}

		c.DeleteCube(_tbd._position);
		
	}
	void DeleteBlock()
	{
		//Debug.Log("DELETING");
		_tbd._toughness -= Time.deltaTime;
		if (_tbd._toughness <= 0)
		{
			_tbd._targetChunk.DeleteCube(_tbd._position);
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
		for (int i = 0; i < _allSectors.Count; i++) {
			_allSectors[i].Draw(_blockMaterial);
		}
		
	}



}

public class Sector {
	static int _cInSector = 4;
	public static ChunkGenerator _chunkGen;
	Vector3Int _minPos;
	Vector3Int _maxPos;
	Chunk[,,] _chunks;

	public Sector(Vector3Int xyz, Vector3Int origin) {
		int x = xyz.x;
		int y = xyz.y;
		int z = xyz.z;
		_minPos = new Vector3Int(x * Chunk._size * _cInSector, y * Chunk._size * _cInSector, z * Chunk._size * _cInSector) + origin;
		//s.maxPos = new Vector3Int(x+1 * Chunk._size.x * _cInSector, y+1 * Chunk._size.y * _cInSector, (z+1) * Chunk._size.z * _cInSector) + _origin;
		_maxPos = _minPos + new Vector3Int(Chunk._size * _cInSector, Chunk._size * _cInSector, Chunk._size * _cInSector);
		_chunks = new Chunk[_cInSector, _cInSector, _cInSector];
	}
	public void CreateChunk()
	{
		int size = Chunk._size;
		for (int x = 0; x < _cInSector; x++)
		{
			for (int y = 0; y < _cInSector; y++)
			{
				for (int z = 0; z < _cInSector; z++)
				{
					_chunkGen.MakeChunk(out _chunks[x, y, z], _minPos + new Vector3Int(x * size, y * size, z * size));
				}
			}
		}
	}
	public void Draw(Material mat)
	{
		for (int x = 0; x < _cInSector; x++) {
			for (int y = 0; y < _cInSector; y++) {
				for (int z = 0; z < _cInSector; z++) {
					_chunks[x, y, z].Draw(mat);
				}
			}
		}

	}
	public bool CheckInside(Vector3Int pos) {

		return (pos.x >= _minPos.x && pos.x < _maxPos.x &&
			pos.y >= _minPos.y && pos.y < _maxPos.y &&
			pos.z >= _minPos.z && pos.z < _maxPos.z);
	}
	public Chunk GetChunkFromPos(Vector3Int pos) {

		//spawn offset sector pos offset

		Vector3Int p = pos - (  _minPos );
		p.x = (int)(p.x / Chunk._size);
		p.y = (int)(p.y / Chunk._size);
		p.z = (int)(p.z / Chunk._size);
		
		return _chunks[p.x, p.y, p.z];
	}
	public static Vector3Int F2IntVector(Vector3 v) {
		return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
	}
};

public struct TargetBlockData
{
	public bool _deleting;
	public float _toughness;
	public Vector3Int _position;
	public BlockType _type;
	public Chunk _targetChunk;
	public Vector3 _breakPlaneAngle;
	public Vector3 _breakPlanePos;
	public Vector3 _targetNormal;
};