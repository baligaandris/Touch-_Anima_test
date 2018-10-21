using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagepoint : MonoBehaviour
{
    private InputManager2 inputManager;
    private matchmanager matchmanager;
    //To stop mutliple collisions
    private bool hit = false;
    //Reset hit
    public void ResetHit() { hit = false; }


    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        //Find the match manager script
        matchmanager = GameObject.FindGameObjectWithTag("matchmanager").GetComponent<matchmanager>();
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager2>();
    }

    //Collision on this head
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.root != gameObject.transform.root && !hit)
        {
            //Add force to indicate knockout blow
            hit = true;
            //Disable body selection
            inputManager.canSelect = false;
            //player was hit, add score depending on the collisions object
            matchmanager.Incrementscore(collision.gameObject);
            //Reset positions
            matchmanager.StartReset();
        }
    }
}
