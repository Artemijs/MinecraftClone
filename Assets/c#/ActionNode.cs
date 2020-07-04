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
	List<ActionNode> _allNodes;
	int _index;
	public CreateSectorAction(Sector s) {
		_sector = s;
		_allNodes = new List<ActionNode>();
		_index = 0;
	}
	public override bool Execute() {
		if (_allNodes[_index].Execute()) {
			_index++;
			if (_index >= _allNodes.Count) {
				return true;
			}
		}
		return false;
	}
	public override void AddAction(int id, ActionNode an) {
		if (id == 0) {
			_cChunksAction = (CreateChunksAction)an;
		}
		if (id == 1) {
			_cBlockDataAction = (CreateBlockDataAction)an;
		}
		if (id == 2) {
			_terraformAction = (TerraformBlockDataAction)an;
		}
		if (id == 3) {
			_cMeshAction = (CreateMeshAction)an;
		}
		_allNodes.Add(an);
	}
	public Sector Sector { get { return _sector; } set { _sector = value; } }
};
namespace Parameters {
	public class ChunkActionData {
		public Vector3Int _position;
		public int _size;
		public ChunkActionData(Vector3Int pos, int size) {
			_position = pos;
			_size = size;
		}
	};
};
public class CreateChunksAction:ActionNode {
	CreateSectorAction _parent;
	Parameters.ChunkActionData _parameters;
	public CreateChunksAction(ActionNode parent, Parameters.ChunkActionData data) {
		_parent = (CreateSectorAction)parent;
		_parameters = data;
	}
	public override bool Execute() {
		Vector3Int pos = Vector3Int.zero;
		GameObject chunkParent = new GameObject(_parameters._position.x + " " + _parameters._position.y + " " + _parameters._position.z);
		Sector s = _parent.Sector;
		for (int i = 0; i < _parameters._size; i++) {
			for (int j = 0; j < _parameters._size; j++) {
				for (int k = 0; k < _parameters._size; k++) {
					pos.x = i * Chunk._uSize;
					pos.y = j * Chunk._uSize;
					pos.z = k * Chunk._uSize;
					s.Chunks[i, j, k] = new Chunk(pos + _parameters._position, s, chunkParent.transform);
				}
			}
		}
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

	public override bool Execute() {

		for (int i = 0; i < _parameterQue.Count; i++) {
			this.CreateBlockData(_parameterQue[i]);
		}
		return true;
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
	public override void AddAction(int id, ActionNode an) {
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
