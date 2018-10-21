using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager2 : MonoBehaviour
{
    private GameObject[] bodyPartsClicked;
    private GameObject[] lineRenderers;
    private Vector3[] clickLocations;
    private LineRenderer line;
    private float timeMoving;

    public GamePhases gamePhase;
    public List<GameObject> selectedBodyParts = new List<GameObject>();
    public Rigidbody2D[] P1BodyParts, P2BodyParts;
    public float forceMultiplier = 1;
    public GameObject linePrefab;
    public bool canSelect;
    public float timeScale;

    public enum GamePhases { Player1, Player2, Execute };

    //Input
    [System.Serializable]
    public class Pointer
    {
        //Pos of mouse or finger on touch input
        public Vector3 position;
        //Where initial click was on drag
        public Vector3 initialPosition;
        //Phase of contact/input
        public enum Phase { Start, Hold, Release }
        public Phase phase;
        public int id;
        public bool active = false;
        public bool swiped = false;
        public float holdTime = 0;

        public void Reset()
        {
            phase = Phase.Start;
            active = false;
            swiped = false;
        }
    }
    public Pointer pointer = new Pointer();
    [System.Serializable]
    public class Action
    {
        public Vector3 force;
        public GameObject objectToEffect;

        public Action(GameObject bodypart, Vector3 force)
        {
            this.objectToEffect = bodypart;
            this.force = force;
        }
    }

    public List<Action> actionsToExecute = new List<Action>();
    private Action newAction;

    // Use this for initialization
    void Start()
    {
        gamePhase = GamePhases.Execute;
        //Set up click locations for future use during gameplay
        clickLocations = new Vector3[10];
        bodyPartsClicked = new GameObject[10];
        lineRenderers = new GameObject[10];

        //Get body part gameobjects
        GetPlayerParts();

        canSelect = true;
    }

    // Update is called once per frame
    void Update()
    {
        timeScale = Time.timeScale;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            pointer.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

            pointer.id = 0;
            if (Input.GetMouseButtonDown(0))
            {
                pointer.phase = Pointer.Phase.Start;
                pointer.active = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                pointer.phase = Pointer.Phase.Release;
                pointer.active = true;
            }
            else if (Input.GetMouseButton(0))
            {
                pointer.phase = Pointer.Phase.Hold;
                pointer.active = true;
            }
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                pointer.active = true;
                foreach (Touch touch in Input.touches)
                {
                    pointer.position.x = touch.position.x;
                    pointer.position.y = touch.position.y;
                    pointer.id = touch.fingerId;
                    if (touch.phase == TouchPhase.Began)
                    {
                        pointer.phase = Pointer.Phase.Start;
                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        pointer.phase = Pointer.Phase.Hold;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        pointer.phase = Pointer.Phase.Release;
                    }
                }
            }

        }

        if (gamePhase == GamePhases.Player1 && canSelect)
        {
            if (pointer.active)
            {
                if (pointer.phase == Pointer.Phase.Start)
                {

                    Vector3 clickLocation = pointer.position;

                    pointer.initialPosition = clickLocation;
                    Rigidbody2D closest = P1BodyParts[0];
                    foreach (Rigidbody2D rb in P1BodyParts)
                    {
                        if (Vector3.Distance(rb.transform.position, clickLocation) < Vector3.Distance(closest.transform.position, clickLocation))
                        {
                            closest = rb;
                        }
                    }
                    //clickLocation = closest.transform.position;
                    clickLocations[pointer.id] = clickLocation;
                    bodyPartsClicked[pointer.id] = closest.gameObject;
                    lineRenderers[pointer.id] = Instantiate(linePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                }

                if (pointer.phase == Pointer.Phase.Hold)
                {
                    if (clickLocations[pointer.id] != null)
                    {
                        clickLocations[pointer.id] = bodyPartsClicked[pointer.id].transform.position;
                    }

                    if ((pointer.position - pointer.initialPosition).magnitude > 0.1)
                    {
                        pointer.swiped = true;
                    }
                }

                if (lineRenderers[pointer.id] != null)
                {
                    lineRenderers[pointer.id].GetComponent<LineRenderer>().SetPosition(0, pointer.initialPosition);
                    lineRenderers[pointer.id].GetComponent<LineRenderer>().SetPosition(1, pointer.position);
                }


                if (pointer.phase == Pointer.Phase.Release)
                {
                    //No longer giving instruction
                    pointer.active = false;

                    //If player wasn't swiping
                    if (pointer.swiped == false)
                    {
                        //For registering if body parts are selected or not (white/red)
                        bool alreadyselected = false;


                        for (int i = selectedBodyParts.Count - 1; i >= 0; i--)
                        {
                            //Check list of selected body parts against body part just clicked
                            if (selectedBodyParts[i] == bodyPartsClicked[pointer.id])
                            {
                                //Body part was already selected
                                selectedBodyParts.Remove(selectedBodyParts[i]);
                                if (bodyPartsClicked[pointer.id].GetComponent<SpriteRenderer>() != null)
                                {
                                    bodyPartsClicked[pointer.id].GetComponent<SpriteRenderer>().color = Color.white;
                                }
                                alreadyselected = true;
                            }
                        }

                        //Else if the body part wasn't already selected
                        if (alreadyselected == false)
                        {
                            //Add to selected parts and change colour to red
                            selectedBodyParts.Add(bodyPartsClicked[pointer.id]);
                            if (bodyPartsClicked[pointer.id].GetComponent<SpriteRenderer>() != null)
                            {
                                bodyPartsClicked[pointer.id].GetComponent<SpriteRenderer>().color = Color.red;
                            }
                        }

                    }
                    //If action was a swipe
                    else
                    {
                        foreach (GameObject bodypart in selectedBodyParts)
                        {
                            //Creating an action for each selected body part, using the force calculated by drag length and multiplier
                            newAction = new Action(bodypart, (pointer.initialPosition - pointer.position) * forceMultiplier);
                            //Reset parts to white
                            if (bodypart.GetComponent<SpriteRenderer>() != null)
                            {
                                bodypart.GetComponent<SpriteRenderer>().color = Color.white;
                            }
                            //Testing debug logs
                            Debug.Log(bodypart.name);
                            Debug.Log(((pointer.initialPosition - pointer.position) * forceMultiplier).ToString());
                            Debug.Log(newAction.objectToEffect.name);
                            Debug.Log(newAction.force.ToString());
                            //End
                            //Actions which are cycled through when executing the turn
                            actionsToExecute.Add(newAction);
                        }
                        //Give turn to player 2
                        gamePhase = GamePhases.Player2;
                        GetComponent<UIController>().ChangeTurn(2);
                        //Clear the selected parts
                        selectedBodyParts.Clear();
                    }
                    //Remove line renderer after release
                    Destroy(lineRenderers[pointer.id]);
                    //Reset the pointer
                    pointer.holdTime = 0;
                    pointer.swiped = false;
                }
            }
        }
        else if (gamePhase == GamePhases.Player2 && canSelect)
        {
            if (pointer.active)
            {
                if (pointer.phase == Pointer.Phase.Start)
                {

                    Vector3 clickLocation = pointer.position;

                    pointer.initialPosition = clickLocation;
                    Rigidbody2D closest = P2BodyParts[0];
                    foreach (Rigidbody2D rb in P2BodyParts)
                    {
                        if (Vector3.Distance(rb.transform.position, clickLocation) < Vector3.Distance(closest.transform.position, clickLocation))
                        {
                            closest = rb;
                        }
                    }
                    clickLocation = closest.transform.position;
                    clickLocations[pointer.id] = clickLocation;
                    bodyPartsClicked[pointer.id] = closest.gameObject;
                    lineRenderers[pointer.id] = Instantiate(linePrefab, new Vector3(0, 0, 0), Quaternion.identity);

                }
                if (pointer.phase == Pointer.Phase.Hold)
                {
                    if (clickLocations[pointer.id] != null)
                    {
                        clickLocations[pointer.id] = bodyPartsClicked[pointer.id].transform.position;
                    }

                    if ((pointer.position - pointer.initialPosition).magnitude > 0.1)
                    {
                        pointer.swiped = true;
                    }
                }

                if (lineRenderers[pointer.id] != null)
                {
                    lineRenderers[pointer.id].GetComponent<LineRenderer>().SetPosition(0, pointer.initialPosition);
                    lineRenderers[pointer.id].GetComponent<LineRenderer>().SetPosition(1, pointer.position);
                }

                if (pointer.phase == Pointer.Phase.Release)
                {
                    pointer.active = false;


                    if (pointer.swiped == false)
                    {
                        bool alreadyselected = false;


                        for (int i = selectedBodyParts.Count - 1; i >= 0; i--)
                        {
                            if (selectedBodyParts[i] == bodyPartsClicked[pointer.id])
                            {
                                selectedBodyParts.Remove(selectedBodyParts[i]);
                                if (bodyPartsClicked[pointer.id].GetComponent<SpriteRenderer>() != null)
                                {
                                    bodyPartsClicked[pointer.id].GetComponent<SpriteRenderer>().color = Color.white;
                                }
                                alreadyselected = true;
                            }
                        }

                        if (alreadyselected == false)
                        {
                            selectedBodyParts.Add(bodyPartsClicked[pointer.id]);
                            if (bodyPartsClicked[pointer.id].GetComponent<SpriteRenderer>() != null)
                            {
                                bodyPartsClicked[pointer.id].GetComponent<SpriteRenderer>().color = Color.red;
                            }

                        }

                    }
                    else
                    {
                        foreach (GameObject bodypart in selectedBodyParts)
                        {
                            newAction = new Action(bodypart, (pointer.initialPosition - pointer.position) * forceMultiplier);

                            if (bodypart.GetComponent<SpriteRenderer>() != null)
                            {
                                bodypart.GetComponent<SpriteRenderer>().color = Color.white;
                            }

                            Debug.Log(bodypart.name);
                            Debug.Log(((pointer.initialPosition - pointer.position) * forceMultiplier).ToString());
                            Debug.Log(newAction.objectToEffect.name);
                            Debug.Log(newAction.force.ToString());
                            actionsToExecute.Add(newAction);
                        }
                        
                        gamePhase = GamePhases.Execute;
                        GetComponent<UIController>().ChangeTurn(3);
                        selectedBodyParts.Clear();
                    }
                    Destroy(lineRenderers[pointer.id]);
                    pointer.holdTime = 0;
                    pointer.swiped = false;

                }
            }
        }
        //Execture phase
        else if (gamePhase == GamePhases.Execute)
        {
            if (Time.timeScale != 0.01f)
            {
                timeMoving += Time.deltaTime;
                if (timeMoving >= 0.5f)
                {
                    Time.timeScale = 0.01f;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    timeMoving = 0;
                    gamePhase = GamePhases.Player1;
                    GetComponent<UIController>().ChangeTurn(1);

                    //reset the pointer to avoid rogue body selection on execute button click
                    pointer.Reset();
                    selectedBodyParts.Clear();
                }
            }
        }

    }

    private void GetPlayerParts()
    {
        //find all bodyparts
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        List<Rigidbody2D> tempRBList = new List<Rigidbody2D>();

        //Collect player 1 body parts
        foreach (Rigidbody2D rb in player.GetComponentsInChildren<Rigidbody2D>())
        {
            if (rb.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                tempRBList.Add(rb);
            }
        }
        //Send to p1 body parts array
        P1BodyParts = tempRBList.ToArray();

        //Clear temp list 
        tempRBList.Clear();
        //Repeat process for player 2
        foreach (Rigidbody2D rb in player2.GetComponentsInChildren<Rigidbody2D>())
        {
            if (rb.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                tempRBList.Add(rb);
            }
        }
        P2BodyParts = tempRBList.ToArray();
    }

    public void ResetTimeSpeed()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;


        //Executing each action for both players. Move to seperate method
        foreach (Action action in actionsToExecute)
        {
            //Adding the force to the action's rigid body
            action.objectToEffect.GetComponent<Rigidbody2D>().AddForce(action.force);
        }

        actionsToExecute.Clear();
    }

}
