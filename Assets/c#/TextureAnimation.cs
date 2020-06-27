using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
	public int _nrOfFrames;
	public int _nrOfLines;
	public float _totalDuration;
	public bool _live;

	float _timePerFrame;
	float _scale;
	Material _material;
	float _time;
	public int _currentFrame;
	public bool _looping;
    // Start is called before the first frame update
    void Start()
    {
		_scale = 1.0f / _nrOfFrames;
		_material = GetComponent<Renderer>().material;
		_time = 0;
		_timePerFrame = _totalDuration / _nrOfFrames;
		_currentFrame = 0;
		_material.mainTextureScale = new Vector2(_scale, 1);

	}

	// Update is called once per frame
	void Update()
    {
		if (!_live) return;
		_time += Time.deltaTime;
		if (_time >= _timePerFrame) {
			_time = 0;
		//if(Input.GetKeyDown(KeyCode.N))
			NextFrame();
		}
    }
	void NextFrame() {
		if (_currentFrame == _nrOfFrames) {
			if (!_looping) {
				_live = false;
				gameObject.SetActive(false);
			}
		}
		_currentFrame++;
		Vector2 off = _material.mainTextureOffset;
		off.x = (_currentFrame % _nrOfFrames) * _scale;
		_material.mainTextureOffset = off;
	}
	public void Play() {
		_time = 0;
		_live = true;
		_currentFrame = 0;
	}
	public void SetDuration(float duration) {
		_totalDuration = duration;
		_timePerFrame = _totalDuration / _nrOfFrames;
		_material.mainTextureOffset = Vector2.zero;
	}
}
