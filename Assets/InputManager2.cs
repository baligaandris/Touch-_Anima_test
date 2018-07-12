using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputManager2 : MonoBehaviour
{

    private GameObject[] bodyPartsClicked;
    private GameObject[] lineRenderers;
    public List<GameObject> selectedBodyParts = new List<GameObject>();
    public Rigidbody2D[] P1BodyParts, P2BodyParts;
    private Vector3[] clickLocations;
    private LineRenderer line;
    public float forceMultiplier = 1;
    public GameObject linePrefab;

    private float timeMoving;

    public enum GamePhases { Player1, Player2, Execute };
    GamePhases gamePhase;
    public Text gamePhaseText;


    //Input
    [System.Serializable]
    public class Pointer
    {
        public Vector3 position;
        public Vector3 initialPosition;
        public enum Phase { Start, Hold, Release }
        public Phase phase;
        public int id;
        public bool active = false;
        public bool swiped = false;
        public float holdTime = 0;
    }
    public Pointer pointer = new Pointer();
    [System.Serializable]
    public class Action
    {
        public Vector3 force;
        public GameObject objectToEffect;
        private GameObject bodypart;
        private Vector3 vector3;

        public Action(GameObject bodypart, Vector3 vector3)
        {
            this.objectToEffect = bodypart;
            this.force = vector3;
        }
    }

    public List<Action> actionsToExecute = new List<Action>();
    private Action newAction;

    // Use this for initialization
    void Start()
    {
        gamePhase = GamePhases.Execute;
        clickLocations = new Vector3[10];
        bodyPartsClicked = new GameObject[10];
        lineRenderers = new GameObject[10];

        //find all bodyparts
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        List<Rigidbody2D> tempList = new List<Rigidbody2D>();

        foreach (Rigidbody2D rb in player.GetComponentsInChildren<Rigidbody2D>())
        {
            if (rb.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                tempList.Add(rb);
            }
            
        }

        P1BodyParts = tempList.ToArray();

        foreach (Rigidbody2D rb in player2.GetComponentsInChildren<Rigidbody2D>())
        {
            if (rb.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                tempList.Add(rb);
            }
        }

        P2BodyParts = tempList.ToArray();


    }

    // Update is called once per frame
    void Update()
    {
        gamePhaseText.text = gamePhase.ToString();

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

        if (gamePhase == GamePhases.Player1)
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
                        selectedBodyParts.Clear();
                        gamePhase = GamePhases.Player2;
                    }
                    Destroy(lineRenderers[pointer.id]);
                    pointer.holdTime = 0;
                    pointer.swiped = false;
                }
            }
        }
        else if (gamePhase == GamePhases.Player2)
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
                        selectedBodyParts.Clear();
                        gamePhase = GamePhases.Execute;
                    }
                    Destroy(lineRenderers[pointer.id]);
                    pointer.holdTime = 0;
                    pointer.swiped = false;

                }
            }
        }
        else if (gamePhase == GamePhases.Execute)
        {
            if (Input.GetButtonDown("Jump"))
            {
                ResetTimeSpeed();
                gamePhase = GamePhases.Execute;
            }

            if (Time.timeScale != 0.01f)
            {
                timeMoving += Time.deltaTime;
                if (timeMoving >= 0.5f)
                {
                    Time.timeScale = 0.01f;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    timeMoving = 0;
                    gamePhase = GamePhases.Player1;
                }
            }
        }

    }

    public void ResetTimeSpeed()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        foreach (Action action in actionsToExecute)
        {
            action.objectToEffect.GetComponent<Rigidbody2D>().AddForce(action.force);
        }

        actionsToExecute.Clear();
    }
}
