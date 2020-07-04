using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Version3_1 {
	public class Node {
		protected Vector3Int _position;//real world position
		static int _size = 2;
		protected int _depth;
		protected int _uSize;
		protected Node[,,] _nodes;
		protected Node _parent;
		public Node(Node parent) {
			_parent = parent;
			_nodes = null;
		}
		//next time add a depth uSize lookup table
		public Node(Vector3 pPos, Vector3Int position, int maxDepth, int depth, Node parent) {
			_parent = parent;
			_position = position;
			_uSize = Sector._suSize;
			Vector3Int pos = position;
			depth++;
			_depth = maxDepth - depth;
			_nodes = new Node[_size, _size, _size];
			//u start at depth +=1 so _uSize is 1 less than it should be
			for (int i = depth; i < maxDepth; i++) {
				_uSize *= _size;
			}
			//which means that it is currently the uSize of the child node 
			Vector3Int max = pos;
			for (int i = 0; i < _size; i++) {
				pos.x = position.x + i * _uSize;
				max.x = pos.x + _uSize;

				for (int j = 0; j < _size; j++) {
					pos.y = position.y + j * _uSize;
					max.y = pos.y + _uSize;

					for (int k = 0; k < _size; k++) {
						//min pos of child ijk
						pos.z = position.z + k * _uSize;
						max.z = pos.z + _uSize;
						if (pPos.x >= pos.x && pPos.y >= pos.y && pPos.z >= pos.z &&
							pPos.x < max.x && pPos.y < max.y && pPos.z < max.z) {

							if (depth == maxDepth) {
								_nodes[i, j, k] = new Sector(pos, this);
							}
							else {
								///THERE IS A MASSIVE PROBLEM HERE
								_nodes[i, j, k] = new Node(pPos, pos, maxDepth, depth, this);
							}
						}
						//else {
						//	_nodes[i, j, k] = null;
						//}
					}
				}
			}
			//and now its the current nodes uSize
			_uSize *= _size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pos"> current sector pos in world space</param>
		/// <param name="next">Normal direction vector :D</param>
		public virtual Node CreateOne(Vector3Int pos, Vector3Int next) {
			Vector3Int index = Vector3Int.zero;
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _size; k++) {
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
			pos = current.Position + next * current._uSize;
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
					= new Node(Vector3.zero, pos, current.GetParent(10, 0)._depth, _depth, this);
				return _nodes[index.x, index.y, index.z];
			}

		}
		public Vector3Int ChildIndex(Vector3Int pos) {
			Vector3Int index = new Vector3Int(-1, -1, -1);
			Vector3Int startPos = _position;
			Vector3Int endPos = _position;
			int s = GetChildUSize();
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _size; k++) {

						startPos = _position + new Vector3Int(i, j, k) * s;
						endPos = _position + new Vector3Int(i + 1, j + 1, k + 1) * s;
						//startPos.x = i * s; startPos.y = j * s; startPos.z = k * s;
						//endPos.x = (i + 1) * s; endPos.y = (j + 1) * s; endPos.z = (k + 1) * s;

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
			int s = GetChildUSize();
			if (_nodes[index.x, index.y, index.z] == null) {
				if (_depth == 0)
					_nodes[index.x, index.y, index.z] = new Sector(index * s + _position, this);
				else
					_nodes[index.x, index.y, index.z] = new Node(position, index * s + _position, StaticFunctions._maxDepth, _depth + 1, this);

			}
			else {
				_nodes[index.x, index.y, index.z].CreateChildBranch(position);
			}
		}

		public int GetChildUSize() {
			foreach (Node n in _nodes) {
				if (n != null)
					return n._uSize;
			}
			return -1;
		}
		public Node CreateNext(Vector3Int directionNormal) {
			Vector3Int pos = _position + directionNormal * _uSize;

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
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _size; k++) {
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
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _size; k++) {
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
			return (pos.x >= _position.x && pos.y >= _position.y && pos.z >= _position.z &&
			pos.x < _position.x + _uSize && pos.y < _position.y + _uSize && pos.z < _position.z + _uSize);
		}

		public Node Search(int maxDepth, int depth, Vector3Int pos) {
			if (_nodes == null) return this;
			if (depth == maxDepth) return this;

			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _size; k++) {

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
			for (int i = 0; i < _size; i++) {
				for (int j = 0; j < _size; j++) {
					for (int k = 0; k < _size; k++) {
						if (_nodes[i, j, k] != null) {
							_nodes[i, j, k].Draw(mat);
						}
					}
				}
			}
		}
		public Vector3Int Position { get => _position; set => _position = value; }
		public Node Parent { get => _parent; set => _parent = value; }
	}
}