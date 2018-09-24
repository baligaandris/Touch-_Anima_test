using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signalspawner : MonoBehaviour {
    public GameObject signalPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectsWithTag("Player").Length ==0)
        {
            Instantiate(signalPrefab, transform.position, Quaternion.Euler(0,0,0));
        }
	}
}
