using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour {

    private GameObject[] bodyPartsClicked;
    private Vector3[] clickLocations;
    private LineRenderer line;
    //Vector3 currentMousePos;
    public float forceMultiplier =1;
    public LineRenderer line0, line1;

    // Use this for initialization
    void Start () {
        //line = gameObject.GetComponent<LineRenderer>();
        clickLocations = new Vector3[4];
        bodyPartsClicked = new GameObject[4];
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (Touch touch in Input.touches)
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[touch.fingerId].phase == TouchPhase.Began)
                {
                    Debug.Log(touch.fingerId);

                    Vector3 clickLocation = Camera.main.ScreenToWorldPoint(Input.touches[touch.fingerId].position);
                    clickLocation = new Vector3(clickLocation.x, clickLocation.y, 0);
                    clickLocations[touch.fingerId] = clickLocation;

                    if (touch.fingerId==0)
                    {
                        line0.enabled = true;
                    }
                    else if (touch.fingerId == 1)
                    {
                        line1.enabled = true;
                    }
                    
                    //Debug.Log("click");
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.touches[touch.fingerId].position), Vector2.zero);
                    if (hit.collider != null)
                    {
                        //Debug.Log(hit.transform.gameObject.name);
                        bodyPartsClicked[touch.fingerId] = hit.transform.gameObject;
                    }
                }
                else if (Input.touches[touch.fingerId].phase == TouchPhase.Moved)
                {
                    if (clickLocations[touch.fingerId] != null)
                    {
                        Vector3 currentMousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.touches[touch.fingerId].position).x, Camera.main.ScreenToWorldPoint(Input.touches[touch.fingerId].position).y, 0);
                        if (touch.fingerId == 0)
                        {
                            line0.SetPosition(0, clickLocations[touch.fingerId]);
                            line0.SetPosition(1, currentMousePos);
                        }
                        else if (touch.fingerId == 1)
                        {
                            line1.SetPosition(0, clickLocations[touch.fingerId]);
                            line1.SetPosition(1, currentMousePos);
                        }
                    }
                }

                else if (Input.touches[touch.fingerId].phase == TouchPhase.Ended)
                {
                    Vector3 currentMousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.touches[touch.fingerId].position).x, Camera.main.ScreenToWorldPoint(Input.touches[touch.fingerId].position).y, 0);
                    if (touch.fingerId == 0)
                    {
                        line0.enabled = false;
                    }
                    else if (touch.fingerId == 1)
                    {
                        line1.enabled = false;
                    }

                    if (bodyPartsClicked[touch.fingerId] != null)
                    {
                        bodyPartsClicked[touch.fingerId].GetComponent<Rigidbody2D>().AddForceAtPosition((clickLocations[touch.fingerId] - currentMousePos) * forceMultiplier, clickLocations[touch.fingerId]);
                        bodyPartsClicked = null;
                    }
                    //clickLocations[touch.fingerId] = Vector3.zero;
                    //bodyPartsClicked[touch.fingerId] = null;
                }
            }
        }
       
	}
}
