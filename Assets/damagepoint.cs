using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagepoint : MonoBehaviour {

    matchmanager matchmanager;

	// Use this for initialization
	void Start () {
        matchmanager = GameObject.FindGameObjectWithTag("matchmanager").GetComponent<matchmanager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.root != gameObject.transform.root)
        {
            matchmanager.Incrementscore(collision.gameObject);
        }
        

    }
}
