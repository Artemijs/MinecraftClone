using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode {

	public ActionNode(){

	}
	public abstract bool Execute();
	public abstract void AddAction(int id, ActionNode an);
}
public class CreateSectorAction : ActionNode {
	Sector _sector;
	//create chunks action
	CreateChunksAction _cChunksAction;
	//create block data action
	CreateBlockDataAction _cBlockDataAction;
	//second pass block data action
	TerraformBlockDataAction _terraformAction;
	//create mesh+bounds action
	CreateMeshAction _cMeshAction;
	public CreateSectorAction(Sector s) {
		_sector = s;
	}
	public override bool Execute() {
		return false;
	}
	public override void AddAction(int id, ActionNode an) {
		if (id == 0) {
			_cChunksAction = (CreateChunksAction)an;
		}
		if (id == 0) {
			_cBlockDataAction = (CreateBlockDataAction)an;
		}
		if (id == 0) {
			_terraformAction = (TerraformBlockDataAction)an;
		}
		if (id == 0) {
			_cMeshAction = (CreateMeshAction)an;
		}
		
	}
	public Sector Sector { get { return _sector; } set { _sector = value; } }
};
public class CreateChunksAction:ActionNode {
	CreateSectorAction _parent;
	public CreateChunksAction(ActionNode parent) {
		_parent = (CreateSectorAction)parent;
	}
	public override bool Execute() {
		/*GameObject chunkParent = new GameObject(position.x + " " + position.y + " " + position.z);
		for (int i = 0; i < _sSize; i++) {
			for (int j = 0; j < _sSize; j++) {
				for (int k = 0; k < _sSize; k++) {
					pos.x = i * Chunk._uSize;
					pos.y = j * Chunk._uSize;
					pos.z = k * Chunk._uSize;
					_chunks[i, j, k] = new Chunk(pos + position, this, chunkParent.transform);
				}
			}
		}*/
		return true;
	}
	public override void AddAction(int id, ActionNode an) {
	}
};
public class CreateBlockDataAction : ActionNode {
	//pos, index
	CreateSectorAction _parent;
	List<Pair<Vector3Int, Vector3Int>> _parameterQue;
	public CreateBlockDataAction( CreateSectorAction parent) {

		_parent = parent;
		_parameterQue = new List<Pair<Vector3Int, Vector3Int>>();
	}
	public override void AddAction(int id, ActionNode an) {
		
	}

	public override bool Execute() {
		return false;
	}
	public void Add2Que(Pair<Vector3Int, Vector3Int> args) {
		_parameterQue.Add(args);
	}
	public void CreateBlockData(Pair<Vector3Int, Vector3Int> args) {
		BlockType bt = TerrrainGen.GetBlockType(args.one);
		//if (j + _position.y > 2)
		//	bt = BlockType.AIR;
		_parent.Sector.BlockData[args.two.x, args.two.y, args.two.z] = new BlockData(args.one, bt);

	}
};
public class TerraformBlockDataAction : ActionNode {
	CreateSectorAction _parent;
	public TerraformBlockDataAction(CreateSectorAction parent) {
		_parent = parent;
	}
	public override void AddAction(int id, ActionNode an) {
		throw new System.NotImplementedException();
	}

	public override bool Execute() {
		TerrrainGen.SecondPass(_parent.Sector);
		return true;
	}
}
public class CreateMeshAction : ActionNode {
	CreateSectorAction _parent;
	public CreateMeshAction(CreateSectorAction parent) {
		_parent = parent;
	}

	public override void AddAction(int id, ActionNode an) {
		throw new System.NotImplementedException();
	}

	public override bool Execute() {
		MeshGenerator.GenerateMesh(_parent.Sector);
		return true;
	}
}