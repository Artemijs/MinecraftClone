﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THIS WHOLE FUXCKING FILE IS JUST AN EXPERIMENTAL TEST FILE
/// NBOTHING HERE IS COMPLETE OR FINISHED BECAUSE I AM A VERY SAD SCRUB PRONE TO SELF DEPRICATION
/// </summary>
public static class ProceduralTerrainGenerator {
	enum PerlinType{
		RAND_HEIGHT,
		NICE_AND_SMOOTH,
		SUPER_ROUGH,
		NOISY_HILLS,
		ROUGH_FLAT,
		TEST_LEVEL

	}
	static Vector3 _worldSize;

	public static void SetUp(){
		_worldSize = new Vector3 (100, World._worldSize.y*Chunk._size.y, 100);//stupendous elavation
	}
	/// <summary>
	/// the following is meant to produce a different terrain based on THE number, each is statement
	/// is essential a trial version of terrain generation
	/// </summary>
	/// <param name="pos"> idk yet</param>
	/// <returns></returns>
	public static  BlockType GetBlockType(Vector3 pos){

		BlockType bt = BlockType.AIR;
		//THE number
		int nr = 2;//3
		if (nr == 0) {
			bt = BlockType.DIRT;
		} else if (nr == 1) {
			//a flat plane
			if (pos.y < 1) {
			
				bt = BlockType.DIRT;
				int x = (int)(pos.x);
				int mX = (int)(Chunk._size.x); 
				if ((x % mX) >= Chunk._size.x - 1)
					bt = BlockType.GRASS;
			} else
				bt = BlockType.AIR;
		} else if (nr == 2) {
			//doesnt work properly T_T
			float h = GetBaseHeight (pos, PerlinType.TEST_LEVEL);
			if (pos.y < h) {
				bt = BlockType.DIRT;
			}
		} else if (nr == 3) {
			bt = Terraform (pos);
		}
		else {

			//splits everything into 4 latyers, lowest being rock, highest being grass
			float h = GetBaseHeight (pos, PerlinType.NOISY_HILLS);
			float grassH = h * 0.75f;
			float rockH = h * 0.5f;
			if (pos.y < h && pos.y > grassH) {
				bt = BlockType.GRASS;
			} else if (pos.y < grassH && pos.y >= rockH) {
				bt = BlockType.DIRT;
			} else if (pos.y < rockH) {
				bt = BlockType.ROCK;
			} else {
				bt = BlockType.AIR;
			}
		}
		return bt;
	}
	private static float GetBaseHeight(Vector3 pos, PerlinType nr){
		float h, h2, h3;

		if (nr == PerlinType.TEST_LEVEL)
		{
			//varying max height
			h = Mathf.PerlinNoise(pos.x / (_worldSize.x), pos.z / (_worldSize.z)) * (_worldSize.y);//very smooth hills
			h2 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.5f), pos.z / (_worldSize.z * 0.5f)) * (_worldSize.y );//smooth hills
			h3 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.25f), pos.z / (_worldSize.z * 0.25f)) * (_worldSize.y * h);//hills
			return h2*0.25f;
		}
		else if (nr == PerlinType.RAND_HEIGHT)
		{
			//varying max height
			h = Mathf.PerlinNoise(pos.x / (_worldSize.x), pos.z / (_worldSize.z));//very smooth hills
			h2 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.5f), pos.z / (_worldSize.z * 0.5f)) * (_worldSize.y * h);//smooth hills
			h3 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.25f), pos.z / (_worldSize.z * 0.25f)) * (_worldSize.y * h);//hills
			return (h2 + h3) / 2;
		}
		else if (nr == PerlinType.NICE_AND_SMOOTH)
		{
			//nice and smooth with a touch of elavation changes
			h = Mathf.PerlinNoise(pos.x / (_worldSize.x), pos.z / (_worldSize.z)) * _worldSize.y * 1;//very smooth hills
			h2 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.5f), pos.z / (_worldSize.z * 0.5f)) * (_worldSize.y * 1);//smooth hills
			h3 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.25f), pos.z / (_worldSize.z * 0.25f)) * (_worldSize.y * 1);//hills
			return (h + h2 + h3) / 3;
		}
		else if (nr == PerlinType.SUPER_ROUGH)
		{
			//super rough terrain
			h = Mathf.PerlinNoise(pos.x / (_worldSize.x), pos.z / (_worldSize.z)) * _worldSize.y * 1;//very smooth hills
			h2 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.5f), pos.z / (_worldSize.z * 0.5f)) * (_worldSize.y * 1);//smooth hills
			h3 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.25f), pos.z / (_worldSize.z * 0.25f)) * (_worldSize.y * 1);//hills
			return (h + h2 + h3) / 3;
		}
		else if (nr == PerlinType.NOISY_HILLS)
		{
			//noisy hills
			h = Mathf.PerlinNoise(pos.x / (_worldSize.x), pos.z / (_worldSize.z)) * _worldSize.y * 0.125f;
			h2 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.5f), pos.z / (_worldSize.z * 0.5f)) * (_worldSize.y * 0.25f);
			h3 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.25f), pos.z / (_worldSize.z * 0.25f)) * (_worldSize.y);
			return (h + h2 + h3);
		}
		else
		{
			//rough flat ground
			h = Mathf.PerlinNoise(pos.x / (_worldSize.x), pos.z / (_worldSize.z)) * _worldSize.y * 0.125f;
			h2 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.5f), pos.z / (_worldSize.z * 0.5f)) * (_worldSize.y * 0.25f);
			h3 = Mathf.PerlinNoise(pos.x / (_worldSize.x * 0.25f), pos.z / (_worldSize.z * 0.25f)) * (_worldSize.y);
			return (h + h2 + h3) * 0.4f;
		}
	}
	private static BlockType Terraform(Vector3 pos){
		BlockType bt = BlockType.AIR;
		float h = GetBaseHeight (pos, PerlinType.NOISY_HILLS);
		pos.x += 100;
		pos.z += 100;
		float h2 = GetBaseHeight (pos, PerlinType.NOISY_HILLS);
		float size = _worldSize.y * 0.05f;
		if (pos.y < h) {
			if (pos.y < h2 - size || pos.y > h2 + size) {
				bt = BlockType.DIRT;
			}
		}
		return bt;
	}
}
