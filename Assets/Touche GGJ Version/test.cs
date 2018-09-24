using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount>0)
        {
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.touches[0].position).x, Camera.main.ScreenToWorldPoint(Input.touches[0].position).y,0);
        }
	}
}
