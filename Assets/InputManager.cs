using System.Collections;
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
        //Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

        //click happens
        if (Input.GetMouseButtonDown(0))
        {
            //find and assign mouse position
            currentMousePos = Input.mousePosition;
            Debug.Log("Pressed primary button.");

            //translate mouse position to world coordinates
            clickLocation = Camera.main.ScreenToWorldPoint(currentMousePos);
            clickLocation = new Vector3(clickLocation.x, clickLocation.y, 0);
            line.enabled = true;

            //cast ray and store body part affected
            RaycastHit2D hit = Physics2D.Raycast(clickLocation, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.transform.gameObject.name);
                bodyPartClicked = hit.transform.gameObject;
            }
        }
        //click and drag Touch moved position
        if (Input.GetButton("Fire1") && Input.mousePosition != clickLocation)
        {
            line.SetPosition(0, clickLocation);
            currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            line.SetPosition(1, currentMousePos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            line.enabled = false;
            if (bodyPartClicked != null)
            {
                bodyPartClicked.GetComponent<Rigidbody2D>().AddForceAtPosition((clickLocation - currentMousePos) * forceMultiplier, clickLocation);
                bodyPartClicked = null;
            }
        }
        //#endif
        //////Unity Documentation
        //Check if we are running on iOS, Android, Windows Phone 8 or iPhone
        //#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        //Check if Input has registered more than zero touches
        //if (Input.touchCount > 0)
        //{
        //    //Store the first touch detected.
        //    Touch myTouch = Input.touches[0];

        //    //Check if the phase of that touch equals Began
        //    if (myTouch.phase == TouchPhase.Began)
        //    {
        //        //If so, set touchOrigin to the position of that touch
        //        touchOrigin = myTouch.position;
        //    }

        //    //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
        //    else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
        //    {
        //        //Set touchEnd to equal the position of this touch
        //        Vector2 touchEnd = myTouch.position;

        //        //Calculate the difference between the beginning and end of the touch on the x axis.
        //        float x = touchEnd.x - touchOrigin.x;

        //        //Calculate the difference between the beginning and end of the touch on the y axis.
        //        float y = touchEnd.y - touchOrigin.y;

        //        //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
        //        touchOrigin.x = -1;

        //        //Check if the difference along the x axis is greater than the difference along the y axis.
        //        if (Mathf.Abs(x) > Mathf.Abs(y))
        //            //If x is greater than zero, set horizontal to 1, otherwise set it to -1
        //            horizontal = x > 0 ? 1 : -1;
        //        else
        //            //If y is greater than zero, set horizontal to 1, otherwise set it to -1
        //            vertical = y > 0 ? 1 : -1;
        //    }
        //}

        ////////////Andras version///////////////////////
        for (int i=0; i<Input.touchCount; i++)
        {
            //Touch action begins here
            if (Input.touchCount > 0 && Input.GetTouch(i).phase == TouchPhase.Began)
            {
                clickLocation = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                clickLocation = new Vector3(clickLocation.x, clickLocation.y, 0);
                line.enabled = true;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log(hit.transform.gameObject.name);
                    bodyPartClicked = hit.transform.gameObject;
                }
            }
            //Touch moved position
            if (Input.touchCount > 0 && Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                if (clickLocation != null)
                {
                    line.SetPosition(0, clickLocation);
                    currentMousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position).y, 0);
                    line.SetPosition(1, currentMousePos);
                }
            }
            //Touch action ends here
            if (Input.touchCount > 0 && Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                line.enabled = false;
                if (bodyPartClicked != null)
                {
                    bodyPartClicked.GetComponent<Rigidbody2D>().AddForceAtPosition((clickLocation - currentMousePos) * forceMultiplier, clickLocation);
                    bodyPartClicked = null;
                }
            }
        }

#endif //End of mobile platform dependendent compilation section started above with #elif
    }
}
