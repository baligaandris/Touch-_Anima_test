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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.root != gameObject.transform.root && !hit && collision.gameObject.tag=="impactSpot")
        {
            //Start the hit process
            TriggerHit(collision.gameObject);
        }
        if (collision.gameObject.tag == "bounds" && matchmanager.deathOnWallCollision())
        {
            //Find which player won the round
            GameObject otherPlayer;
            if (gameObject.transform.root.gameObject.tag == "Player1")
                otherPlayer = GameObject.Find("Char_new 2");
            else
                otherPlayer = GameObject.Find("Char_new");
            //Pass player into the round reset call
            TriggerHit(otherPlayer);
        }
    }

    private void TriggerHit(GameObject otherPlayer)
    {
        //Add force to indicate knockout blow
        hit = true;
        //Disable body selection
        inputManager.canSelect = false;
        //player was hit, add score depending on the collisions object
        matchmanager.Incrementscore(otherPlayer);
        //Reset positions
        matchmanager.StartReset();
    }
}
