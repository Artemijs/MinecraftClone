﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sector : Node {

	public static int _sSize;
	public static int _suSize;
	Chunk[,,] _chunks;
	BlockData[,,] _blockData;
	/// <summary>
	/// CREATE THE MESHES HERE THIS TIME AND GIVE IT TO CHUNK
	/// </summary>
	/// <param name="position"> IN WORDLD SPACE </param>
	public Sector(Vector3Int position, Node parent) : base(parent) {

		
		//base._uSize = _suSize;
		_position = position;
		_chunks = new Chunk[_sSize, _sSize, _sSize];
		Vector3Int pos = Vector3Int.zero;
		_blockData = new BlockData[_suSize, _suSize, _suSize];

		CreateSectorAction csa = new CreateSectorAction(this);

		Parameters.ChunkActionData sad = new Parameters.ChunkActionData(position, _sSize);

		CreateChunksAction cChunk = new CreateChunksAction(csa, sad);
		CreateBlockDataAction cbda = new CreateBlockDataAction(csa);
		TerraformBlockDataAction tfbda = new TerraformBlockDataAction(csa);

		for (int i = 0; i < _suSize; i++) {
			for (int j = 0; j < _suSize; j++) {
				for (int k = 0; k < _suSize; k++) {
					cbda.Add2Que(new Pair<Vector3Int, Vector3Int>(_position, new Vector3Int(i, j, k)));
				}
			}
		}

		csa.AddAction(0, cChunk);
		csa.AddAction(1, cbda);
		csa.AddAction(2, tfbda);
		csa.AddAction(3, new CreateMeshAction(csa));
		ActionQue.Add2Que(csa);
		/*
		GameObject chunkParent = new GameObject(position.x + " " + position.y + " " + position.z);
		for (int i = 0; i < _sSize; i++) {
			for (int j = 0; j < _sSize; j++) {
				for (int k = 0; k < _sSize; k++) {
					pos.x = i * Chunk._uSize;
					pos.y = j * Chunk._uSize;
					pos.z = k * Chunk._uSize;
					_chunks[i, j, k] = new Chunk(pos + position, this, chunkParent.transform);
				}
			}
		}
		_depth = 666;

		
		//CreateBlockData();

		*/
	}
	private void CreateBlockData() {
		for (int i = 0; i < _suSize; i++) {
			for (int j = 0; j < _suSize; j++) {
				for (int k = 0; k < _suSize; k++) {
					BlockType bt = TerrrainGen.GetBlockType(_position + new Vector3Int(i, j, k));
					_blockData[i, j, k] = new BlockData(_position + new Vector3Int(i, j, k), bt);
				}
			}
		}
	}
	public override BlockData GetBlock(Vector3Int pos) {
		pos -= _position;
		if (pos.x < 0 || pos.y < 0 || pos.z < 0 ||
			pos.x >= Sector._suSize || pos.y >= Sector._suSize || pos.z >= Sector._suSize)
			return null;
		return _blockData[pos.x, pos.y, pos.z];
	}
	public override bool IsInside(Vector3Int pos) {
		int usize = USize;
		return (pos.x >= _position.x && pos.y >= _position.y && pos.z >= _position.z &&
		pos.x < _position.x + usize && pos.y < _position.y + usize && pos.z < _position.z + usize);
	}
	public Chunk[,,] Chunks { get { return _chunks; } set { _chunks = value; } }
	public BlockData[,,] BlockData { get { return _blockData; } set { _blockData = value; } }

	public override void Draw(Material mat) {
		for (int i = 0; i < _sSize; i++) {
			for (int j = 0; j < _sSize; j++) {
				for (int k = 0; k < _sSize; k++) {
					if(_chunks[i, j, k] != null)
						_chunks[i, j, k].Draw(mat);
				}
			}
		}
	}
	public override void InitBlockData() {
		int testId = SpeedTests.Instance.StartTest(TestName.SecondPass);
		TerrrainGen.SecondPass(this);
		SpeedTests.Instance.EndTest(TestName.SecondPass, testId);
	}
}
public class BlockData {
	BlockType _bt;
	Vector3 _position;//world space
	public BlockData(Vector3 pos, BlockType bt) {
		_position = pos;
		_bt = bt;
	}
	public BlockType Type { get { return _bt; } set { _bt = value; } }
};

public static class StaticFunctions {
	public static int _maxDepth = 5;
	public static List<int> _nodeChildCount;
	public static List<int> _uSizeArray;
	public static Vector3Int Vector3F2Int(Vector3 pos) {
		return new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
	}
};