using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum TestName {
	UpdateLoadArea = 0,
	UpdateLoadOneLoop,
	SectorCreateTotal,
	SectorCreate,
	SectorBlockCreate,
	SecondPass,
	ACTION,
	END
};

public class SpeedTests {
	static SpeedTests _instance;
	List<List<Pair<DateTime, DateTime>>> _testData;
	public static SpeedTests Instance {
		get {
			if (_instance == null) {
				_instance = new SpeedTests();
			}
			return _instance;
		}
	}
	private SpeedTests() {
		_testData = new List<List<Pair<DateTime, DateTime>>>();
		for (int i = 0; i < (int)TestName.END; i++) {
			_testData.Add(new List<Pair<DateTime, DateTime>>());
		}

	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="testName"></param>
	/// <returns>returns test id</returns>
	public int StartTest(TestName testName) {
		_testData[(int)(testName)].Add(new Pair<DateTime, DateTime>(DateTime.Now, DateTime.Now));
		return _testData[(int)(testName)].Count-1;
	}
	public void EndTest(TestName testName, int id) {
		_testData[(int)(testName)][id].two = DateTime.Now;
	}
	public void PrintResults() {
		for (int i = 0; i < (int)TestName.END; i++) {
			List<Pair<DateTime, DateTime>> current = _testData[i];
			TimeSpan total = new TimeSpan();
			for (int j = 0; j < current.Count; j++) {
				total += (current[j].two.Subtract( current[j].one));
			}
			Debug.Log((TestName)(i)+" "+(total.TotalMilliseconds/ current.Count));
		}
		//_testData.Clear();
	}

}
