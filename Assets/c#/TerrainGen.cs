using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THIS WHOLE FUXCKING FILE IS JUST AN EXPERIMENTAL TEST FILE
/// NBOTHING HERE IS COMPLETE OR FINISHED BECAUSE I AM A VERY SAD SCRUB PRONE TO SELF DEPRICATION
/// </summary>
public static class TerrrainGen
{
	enum PerlinType
	{
		RAND_HEIGHT,
		NICE_AND_SMOOTH,
		SUPER_ROUGH,
		NOISY_HILLS,
		ROUGH_FLAT,
		TEST_LEVEL

	}
	static Vector3 _worldSize;
	static float _waterLevel;
	static float _groundLevel;
	static List<BlockData> _blocks;
	public static void SetUp()
	{
		_worldSize = new Vector3(15, Chunk._size*Sector._cInSector, 15);//stupendous elavation
		_waterLevel = (Chunk._size * Sector._cInSector )* 0.25f;
		_groundLevel = (Chunk._size * Sector._cInSector) * 0.75f;
		_blocks = new List<BlockData>();
	}
	/// <summary>
	/// the following is meant to produce a different terrain based on THE number, each is statement
	/// is essential a trial version of terrain generation
	/// </summary>
	/// <param name="pos"> idk yet</param>
	/// <returns></returns>
	public static BlockType GetBlockType(Vector3 pos)
	{

		BlockType bt = BlockType.AIR;
		float baseH = GetBaseHeight(pos);
		float rockH = GetRockHeight(pos);
		if (pos.y > baseH)
		{
			if (pos.y <= _waterLevel) {
				bt = BlockType.FROZEN_ICE_DIRT;
			}
			else 
				bt = BlockType.AIR;
		}
		else {
			if(pos.y >= rockH)
				bt = BlockType.DIRT;
			else
				bt = BlockType.ROCK;

		}
			
		//Debug.Log(Perlin.Noise(pos.x*100, pos.y*100, pos.z*100));
		//if (pos.y <= 2) return BlockType.DIRT_GRASS;
		//else return BlockType.AIR;
		return bt;
	}
	public static float GetBaseHeight(Vector3 pos) {
		
		float h = Mathf.PerlinNoise(pos.x / (_worldSize.x), pos.z / (_worldSize.z));
		h *= _groundLevel;
		float h1 = Mathf.PerlinNoise(pos.x / (50), pos.z / (50));
		h1 *= _groundLevel;
		float h3 = Mathf.PerlinNoise(pos.x / (100), pos.z / (100));
		h3 *= _groundLevel;
		h = (h + h1 + h3) / 3;
		return h;
	}
	public static float GetRockHeight(Vector3 pos)
	{
		pos += new Vector3(1125, -10, 1756);
		float h = Mathf.PerlinNoise(pos.x / (_worldSize.x), pos.z / (_worldSize.z));
		h *= _groundLevel;
		float h1 = Mathf.PerlinNoise(pos.x / (25), pos.z / (25));
		h1 *= _groundLevel;
		float h3 = Mathf.PerlinNoise(pos.x / (100), pos.z / (100));
		h3 *= _groundLevel;
		
		return ((h + h1 + h3) / 3)-3;
	}
	public static void SecondPass(List<BlockData> blocks) {
		_blocks = blocks;
		int index = 0;
		bool tree = false;
		Vector3Int blockPos = new Vector3Int();
		for (int x = 0; x < Chunk._size; x++) {
			for (int y = 0; y < Chunk._size; y++)
			{
				for (int z = 0; z < Chunk._size; z++)
				{
					blockPos = new Vector3Int(x, y, z);
					//index = ChunkGenerator.GetIndexFromPos(new Vector3Int(x, y, z));
					index = ChunkGenerator.GetIndexFromPos(blockPos);
					BlockData bd = blocks[index];
					BlockData[] nBlocks = ChunkGenerator.GetNeighbours(blockPos);

					if (nBlocks[(int)BlockSide.TOP]._type == BlockType.AIR ) {

						bd._type = BlockType.DIRT_GRASS;
						if (IsTree(blockPos) && !tree && nBlocks[(int)BlockSide.BOTTOM]._type == BlockType.DIRT_GRASS)
						{
							tree = true;
							CreateTree(blockPos);
						}
					}
					//if(y == Chunk._size - 1 && nBlocks[(int)BlockSide.BOTTOM]._type == BlockType.DIRT)
					//	bd._type = BlockType.DIRT_GRASS;
					blocks[index] = bd;
					

				}
			}
		}
	}
	public static void CreateTree(Vector3Int blockPos)
	{
		BlockData bd;
		int index = 0;
		
		for (int y = 0; y < 5; y++) {
			
			blockPos.y++;
			
			index = ChunkGenerator.GetIndexFromPos(blockPos);
			if (index >= _blocks.Count) break;
			bd = _blocks[index];
			bd._type = BlockType.TREE_WOOD;
			bd._on = true;
			_blocks[index] = bd;
			if (y > 1) CreateTreeLeaves(blockPos);
		}
	}
	public static void CreateTreeLeaves(Vector3Int pos) {
		BlockData bd;
		int index = 0;
		Vector3Int startPos = pos - new Vector3Int(2, 0, 2);
		for (; startPos.x < pos.x + 2; startPos.x++) {
			for (; startPos.z < pos.z + 2; startPos.z++)
			{
				if (startPos == pos) continue;
				if (startPos.x < 0) startPos.x = 0;
				if (startPos.z < 0) startPos.z = 0;
				index = ChunkGenerator.GetIndexFromPos(startPos);
				if (index >= _blocks.Count) continue;
				if (index < 0) continue;

				bd = _blocks[index];
				bd._type = BlockType.TREE_LEAVES;
				bd._on = true;
				_blocks[index] = bd;
			}
		}

		//create plane (start pos, end pos, type)
		//create block (start pos, end pos, type)
		//create line R(pos, len)
		//getLineR(pos, len, xyz)
		//
	}
	public static bool IsTree(Vector3Int blockPos) {
		//float treeChance = Mathf.PerlinNoise((blockPos.x+Random.Range(-100,100))/25.0f, blockPos.z/25.0f)*10;
		//return (treeChance > 5);
		return (blockPos.x == 2 && blockPos.z == 2);
		//CheckNeighboursRecursively(0, BlockType.TREE_WOOD, blockPos);

	}
	public static void CheckNeighboursRecursively(int distance, BlockType bt, Vector3Int blockPos) {

		distance++;
		//get block (block pos)
		//check if block is of bt.type
		BlockData[] nBlocks = ChunkGenerator.GetNeighbours(blockPos);
		//CheckNeighboursRecursively(distance, bt, nBlocks[(int)BlockSide.LEFT]._position);
		//CheckNeighboursRecursively(distance, bt, nBlocks[(int)BlockSide.RIGHT]._position);
		//CheckNeighboursRecursively(distance, bt, nBlocks[(int)BlockSide.FORWARD]._position);
		//CheckNeighboursRecursively(distance, bt, nBlocks[(int)BlockSide.BACKWARD]._position);

	}

}