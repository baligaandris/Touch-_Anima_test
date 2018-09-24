using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleforce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.left*10);
        }
	}
}
