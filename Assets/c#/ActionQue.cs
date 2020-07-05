using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionQue : MonoBehaviour {

	static List<ActionNode> _actionQue;
	int _index;

	void Awake() {
		_actionQue = new List<ActionNode>();
		_index = 0;
	}
	private void Update() {

		//for (int i = 0; i < _actionQue.Count; i++) {
		//_actionQue[i].Execute();
		if (_actionQue.Count <= 0) return;
		if (_actionQue[0].Execute()) {
			//_index++;
			_actionQue.RemoveAt(0);
		}
		//}
		//_actionQue.RemoveRange(0, len);
		Debug.Log(_actionQue.Count);

	}
	public static void Add2Que(ActionNode an) {
		_actionQue.Add(an);
	}
	public static bool Empty() {
		return (_actionQue.Count <= 0);
	}
}