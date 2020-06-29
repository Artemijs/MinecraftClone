using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	
	ChunkController _chunkCtrl;
	Chunk _chunk;
	
	
	
	
	// Start is called before the first frame update
	void Start()
    {

		
		
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
		

	}

    // Update is called once per frame
    void Update()
    {
		
		HandleInput();
		
	}
	void HandleInput() {
		

	}
	
}

