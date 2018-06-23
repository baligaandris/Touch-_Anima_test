using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetter : MonoBehaviour {

    private Vector3 pos;
    private Quaternion rot;

	// Use this for initialization
	void Start () {
        pos = transform.position;
        rot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetTransform() {
        transform.position = pos;
        transform.rotation = rot;
        if (GetComponent<Rigidbody2D>()!=null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
