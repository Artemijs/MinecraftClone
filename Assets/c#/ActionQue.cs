using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

	public class ActionQue : MonoBehaviour {
		static Action<Sector> _action;
		static List<Sector> _queParameters;
		public int _actionsPerFrame;
		private void Start() {
			_queParameters = new List<Sector>();
			_action = this.CreateMesh;
		}
		private void Update() {
			if (_queParameters.Count <= 0) return;
			int id = SpeedTests.Instance.StartTest(TestName.ACTION);
			int len = getActionsPerFrameVar();
			for (int i = 0; i < len; i++) {
				_action.Invoke(_queParameters[i]);
			}
			_queParameters.RemoveRange(0, len);
			Debug.Log(_queParameters.Count);
			SpeedTests.Instance.EndTest(TestName.ACTION, id);
		}
		int getActionsPerFrameVar() {
			if (_queParameters.Count < _actionsPerFrame) {
				return _queParameters.Count;
			}
			else return _actionsPerFrame;
		}
		public static void Add2Que(Sector s) {
			_queParameters.Add(s);
		}
		public void CreateMesh(Sector s) {
			s.InitBlockData();
			MeshGenerator.GenerateMesh(s);
		}
	}