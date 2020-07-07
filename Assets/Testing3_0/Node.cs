using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
	protected Vector3Int _position;//real world position
								   //static int _size = 2;
	protected int _depth;
	//protected int _uSize;
	protected Node[,,] _nodes;
	protected Node _parent;
	public Node(Node parent) {
		_parent = parent;
		_nodes = null;
		_depth = 1;
	}
	//next time add a depth uSize lookup table
	public Node(Vector3 pPos, Vector3Int position, int depth, Node parent) {
		_parent = parent;
		_position = position;
		Vector3Int pos = position;
		
		_depth = depth;
		depth--;
		int size = StaticFunctions._nodeChildCount[_depth];
		int uSize = StaticFunctions._uSizeArray[_depth - 1];
		_nodes = new Node[size, size, size];

		Vector3Int max = pos;
		for (int i = 0; i < size; i++) {
			pos.x = position.x + i * uSize;
			max.x = pos.x + uSize;

			for (int j = 0; j < size; j++) {
				pos.y = position.y + j * uSize;
				max.y = pos.y + uSize;

				for (int k = 0; k < size; k++) {
					//min pos of child ijk
					pos.z = position.z + k * uSize;
					max.z = pos.z + uSize;
					if (pPos.x >= pos.x && pPos.y >= pos.y && pPos.z >= pos.z &&
						pPos.x < max.x && pPos.y < max.y && pPos.z < max.z) {

						if (_depth == 2) {
							_nodes[i, j, k] = new Sector(pos, this);
						}
						else {
							///THERE IS A MASSIVE PROBLEM HERE
							_nodes[i, j, k] = new Node(pPos, pos, depth, this);
						}
					}
					//else {
					//	_nodes[i, j, k] = null;
					//}
				}
			}
		}
	}
	public Node( Vector3Int position, int depth, Node parent) {
		_parent = parent;
		_position = position;
		Vector3Int pos = position;

		_depth = depth;
		depth--;
		int size = StaticFunctions._nodeChildCount[_depth];
		int uSize = StaticFunctions._uSizeArray[_depth - 1];
		_nodes = new Node[size, size, size];
		for (int i = 0; i < size; i++) {
			pos.x = position.x + i * uSize;
			for (int j = 0; j < size; j++) {
				pos.y = position.y + j * uSize;
				for (int k = 0; k < size; k++) {
					pos.z = position.z + k * uSize;
					if (_depth == 2) {
						_nodes[i, j, k] = new Sector(pos, this);
					}
					else {
						_nodes[i, j, k] = new Node(pos, depth, this);
					}
				}
			}
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="pos"> current sector pos in world space</param>
	/// <param name="next">Normal direction vector :D</param>
	public virtual Node CreateOne(Vector3Int pos, Vector3Int next) {
		Vector3Int index = Vector3Int.zero;
		int size = StaticFunctions._nodeChildCount[_depth];
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				for (int k = 0; k < size; k++) {
					if (_nodes[i, j, k] != null)
						if (_nodes[i, j, k].IsInside(pos)) {
							index.x = i;
							index.y = j;
							index.z = k;
						}
				}
			}
		}
		Node current = _nodes[index.x, index.y, index.z];
		pos = current.Position + next * current.USize;
		index += next;
		if (_depth == 0) {
			Sector s = new Sector(pos, this);
			_nodes[index.x, index.y, index.z] = s;
			s.InitBlockData();
			MeshGenerator.GenerateMesh(s);
			return s;
		}
		else {
			//current.GetParent(10, 0)._depth;
			_nodes[index.x, index.y, index.z]
				= new Node(Vector3.zero, pos, _depth, this);
			return _nodes[index.x, index.y, index.z];
		}

	}
	public Vector3Int ChildIndex(Vector3Int pos) {

		Vector3Int index = new Vector3Int(-1, -1, -1);
		Vector3Int startPos = _position;
		Vector3Int endPos = _position;
		int childCount = this.ChildCount;

		int s = StaticFunctions._uSizeArray[_depth - 1];
		for (int i = 0; i < childCount; i++) {
			for (int j = 0; j < childCount; j++) {
				for (int k = 0; k < childCount; k++) {

					startPos = _position + new Vector3Int(i, j, k) * s;
					endPos = _position + new Vector3Int(i + 1, j + 1, k + 1) * s;

					if (pos.x >= startPos.x && pos.y >= startPos.y && pos.z >= startPos.z &&
						pos.x < endPos.x && pos.y < endPos.y && pos.z < endPos.z) {
						return new Vector3Int(i, j, k);
					}
				}
			}
		}
		return index;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="position"> in world space</param>
	/// <param name="position"> in world space</param>
	public void CreateChildBranch(Vector3Int position) {
		Vector3Int index = ChildIndex(position);
		int s = StaticFunctions._uSizeArray[_depth-1];
		if (_nodes[index.x, index.y, index.z] == null) {
			if (_depth == 2)
				_nodes[index.x, index.y, index.z] = new Sector(index * s + _position, this);
			else
				_nodes[index.x, index.y, index.z] = new Node(position, index * s + _position, _depth - 1, this);

		}
		else {
			_nodes[index.x, index.y, index.z].CreateChildBranch(position);
		}
	}
	public void CreateChildArea(Vector3Int position, int targetDepth) {

		int s = StaticFunctions._uSizeArray[_depth - 1];

		if (_depth != targetDepth) {
			Vector3Int index = ChildIndex(position);
			if (_nodes[index.x, index.y, index.z] != null) {
				_nodes[index.x, index.y, index.z].CreateChildArea(position, targetDepth);
			}
			else {
				_nodes[index.x, index.y, index.z] = new Node( index * s + _position, _depth - 1, this);
				_nodes[index.x, index.y, index.z].CreateChildArea(position, targetDepth);
			}
		}
		else {
			int childCount = ChildCount;
			for (int i = 0; i < childCount; i++) {
				for (int j = 0; j < childCount; j++) {
					for (int k = 0; k < childCount; k++) {
						if (_depth == 2) {
							_nodes[i, j, k] = new Sector(new Vector3Int(i, j, k) * s + _position, this);
						}
						else {
							_nodes[i, j, k] = new Node(new Vector3Int(i, j, k) * s + _position, _depth - 1, this);
						}
						
					}
				}
			}
		}

		/*Vector3Int index = ChildIndex(position);
		int s = StaticFunctions._uSizeArray[_depth - 1];
		if (_nodes[index.x, index.y, index.z] == null) {
			if (_depth == 2)
				_nodes[index.x, index.y, index.z] = new Sector(index * s + _position, this);
			else
				_nodes[index.x, index.y, index.z] = new Node(position, index * s + _position, _depth - 1, this);

		}
		else {
			_nodes[index.x, index.y, index.z].CreateChildBranch(position);
		}*/
	}
	/// <summary>
	/// ///////////////////////////////////////////////////////////////////////
	/// DEPTH SIZE LOOK UP TABLE BRUV :D
	/// </summary>
	/// <returns></returns>
	public Node CreateNext(Vector3Int directionNormal) {
		Vector3Int pos = _position + directionNormal * USize;

		Node parent = this.Parent;
		while (!parent.IsInside(pos)) {
			parent = parent.Parent;
		}
		parent.CreateChildBranch(directionNormal);

		return parent.Search(10, 0, pos);
	}
	public Node CreateNextFromPos(Vector3Int pos) {

		//Vector3Int pos = _position + directionNormal * _uSize;

		Node parent = this.Parent;
		while (!parent.IsInside(pos)) {
			parent = parent.Parent;
		}

		parent.CreateChildBranch(pos);

		return parent.Search(10, 0, pos);
	}
	public Node GetParent(int distance, int count) {
		count++;
		if (distance == count || this._parent == null)
			return this;
		return this.Parent.GetParent(distance, count);

	}
	public virtual void InitBlockData() {
		int children = ChildCount;
		for (int i = 0; i < children; i++) {
			for (int j = 0; j < children; j++) {
				for (int k = 0; k < children; k++) {
					//if (_nodes != null) {
					if (_nodes[i, j, k] != null) {
						_nodes[i, j, k].InitBlockData();
					}

				}
			}
		}
	}
	public bool CheckExists(Vector3Int pos, int desiredDepth) {
		//check inside 
		//get index
		//if index is null

		if (!IsInside(pos)) {

			return this.Parent.CheckExists(pos, desiredDepth);
		}
		else {
			if (_depth == (desiredDepth))
				return true;
			else {
				Vector3Int index = this.ChildIndex(pos);
				if (_nodes[index.x, index.y, index.z] != null) {
					return _nodes[index.x, index.y, index.z].CheckExists(pos, desiredDepth);
				}
				else
					return false;
			}

		}
	}
	public virtual BlockData GetBlock(Vector3Int pos) {
		BlockData bd = null;
		int childCount = ChildCount;
		for (int i = 0; i < childCount; i++) {
			for (int j = 0; j < childCount; j++) {
				for (int k = 0; k < childCount; k++) {
					//if (_nodes != null) {
					if (_nodes[i, j, k] != null) {
						if (_nodes[i, j, k].IsInside(pos)) {
							return _nodes[i, j, k].GetBlock(pos);
						}
						else {
							Debug.Log("WTF" + pos);
						}
					}
				}
			}
		}
		return bd;
	}

	public virtual bool IsInside(Vector3Int pos) {
		/*bool[] bs = new bool[] {
			(pos.x >= _position.x),
			(pos.y >= _position.y),
			(pos.x >= _position.z),

		};*/
		int uSize = USize;
		return (pos.x >= _position.x && pos.y >= _position.y && pos.z >= _position.z &&
		pos.x < _position.x + uSize && pos.y < _position.y + uSize && pos.z < _position.z + uSize);
	}

	public Node Search(int maxDepth, int depth, Vector3Int pos) {
		if (_nodes == null) return this;
		if (depth == maxDepth) return this;
		int childCount = ChildCount;
		for (int i = 0; i < childCount; i++) {
			for (int j = 0; j < childCount; j++) {
				for (int k = 0; k < childCount; k++) {

					if (_nodes[i, j, k] != null) {
						if (_nodes[i, j, k].IsInside(pos)) {
							depth++;
							return _nodes[i, j, k].Search(maxDepth, depth, pos);
						}
					}
				}
			}
		}
		return null;
	}
	public virtual void Draw(Material mat) {
		return;
		int childCount = ChildCount;
		for (int i = 0; i < childCount; i++) {
			for (int j = 0; j < childCount; j++) {
				for (int k = 0; k < childCount; k++) {
					if (_nodes[i, j, k] != null) {
						_nodes[i, j, k].Draw(mat);
					}
				}
			}
		}
	}
	public Vector3Int Position { get => _position; set => _position = value; }
	public Node Parent { get => _parent; set => _parent = value; }
	public int USize {
		get { return StaticFunctions._uSizeArray[this._depth]; }
	}
	public int ChildCount {
		get { return StaticFunctions._nodeChildCount[this._depth]; }
	}
}
