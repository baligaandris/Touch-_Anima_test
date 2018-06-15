﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour {

    private GameObject bodyPartClicked;
    private Vector3 clickLocation;
    private LineRenderer line;
    Vector3 currentMousePos;
    public float forceMultiplier =1;

    // Use this for initialization
    void Start () {
        line = gameObject.GetComponent<LineRenderer>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (Touch touch in Input.touches)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                clickLocation = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                clickLocation = new Vector3(clickLocation.x, clickLocation.y, 0);
                line.enabled = true;
                Debug.Log("click");
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log(hit.transform.gameObject.name);
                    bodyPartClicked = hit.transform.gameObject;
                }
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (clickLocation != null)
                {
                    line.SetPosition(0, clickLocation);
                    currentMousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y, 0);
                    line.SetPosition(1, currentMousePos);
                }
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                line.enabled = false;
                if (bodyPartClicked != null)
                {
                    bodyPartClicked.GetComponent<Rigidbody2D>().AddForceAtPosition((clickLocation - currentMousePos) * forceMultiplier, clickLocation);
                    bodyPartClicked = null;
                }


            }
        }
       
	}
}
