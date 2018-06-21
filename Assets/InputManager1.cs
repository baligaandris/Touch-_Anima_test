using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager1 : MonoBehaviour {

    private GameObject[] bodyPartsClicked;
    private GameObject[] lineRenderers;
    public Rigidbody2D[] allBodyParts;
    private Vector3[] clickLocations;
    private LineRenderer line;
    //Vector3 currentMousePos;
    public float forceMultiplier =1;
    public GameObject linePrefab;

    // Use this for initialization
    void Start () {
        //line = gameObject.GetComponent<LineRenderer>();
        clickLocations = new Vector3[10];
        bodyPartsClicked = new GameObject[10];
        lineRenderers = new GameObject[10];

        //find all bodyparts
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<Rigidbody2D> tempList = new List<Rigidbody2D>();

        foreach (GameObject player in players)
        {
            foreach (Rigidbody2D rb in player.GetComponentsInChildren<Rigidbody2D>())
            {
                tempList.Add(rb);
            }
        }
        allBodyParts = tempList.ToArray();
            
        
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (Touch touch in Input.touches)
        {
            if (Input.touchCount > 0 && Input.touchCount<10)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log("begin " + touch.fingerId);

                    Vector3 clickLocation = Camera.main.ScreenToWorldPoint(touch.position);
                    clickLocation = new Vector3(clickLocation.x, clickLocation.y, 0);
                    Rigidbody2D closest = allBodyParts[0];
                    foreach (Rigidbody2D rb in allBodyParts)
                    {
                        if (Vector3.Distance(rb.transform.position,clickLocation) < Vector3.Distance(closest.transform.position,clickLocation))
                        {
                            closest = rb;
                        }
                    }
                    clickLocation = closest.transform.position;
                    clickLocations[touch.fingerId] = clickLocation;
                    bodyPartsClicked[touch.fingerId] = closest.gameObject;
                    lineRenderers[touch.fingerId] = Instantiate(linePrefab,new Vector3(0,0,0),Quaternion.identity);
                    
                    //Debug.Log("click");
                    //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                    //if (hit.collider != null)
                    //{
                    //    //Debug.Log(touch.fingerId);
                    //    //Debug.Log(hit.transform.gameObject.name);
                    //    bodyPartsClicked[touch.fingerId] = hit.transform.gameObject;
                    //}
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    if (clickLocations[touch.fingerId] != null)
                    {


                        clickLocations[touch.fingerId] = bodyPartsClicked[touch.fingerId].transform.position;


                        
                    }
                }
                Vector3 currentMousePos = new Vector3(Camera.main.ScreenToWorldPoint(touch.position).x, Camera.main.ScreenToWorldPoint(touch.position).y, 0);
                lineRenderers[touch.fingerId].GetComponent<LineRenderer>().SetPosition(0, bodyPartsClicked[touch.fingerId].transform.position);
                lineRenderers[touch.fingerId].GetComponent<LineRenderer>().SetPosition(1, currentMousePos);

                if (touch.phase == TouchPhase.Ended)
                {
                    Debug.Log("remove " + touch.fingerId);

                    foreach(Touch target in Input.touches)
                    {
                        if (target.fingerId == touch.fingerId)
                        {
                            Destroy(lineRenderers[touch.fingerId]);
                        }
                    }
                    

                    if (bodyPartsClicked[touch.fingerId] != null)
                    {
                        bodyPartsClicked[touch.fingerId].GetComponent<Rigidbody2D>().AddForceAtPosition((clickLocations[touch.fingerId] - currentMousePos) * forceMultiplier, clickLocations[touch.fingerId]);
                        //bodyPartsClicked = null;
                    }
                    //clickLocations[touch.fingerId] = Vector3.zero;
                    //bodyPartsClicked[touch.fingerId] = null;
                }
            }
        }
       
	}
}
