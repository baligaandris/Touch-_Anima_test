using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagepoint : MonoBehaviour
{
    matchmanager matchmanager;

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        matchmanager = GameObject.FindGameObjectWithTag("matchmanager").GetComponent<matchmanager>();
    }

    //Collision on this head
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.root != gameObject.transform.root)
        {
            matchmanager.Incrementscore(collision.gameObject);
            //Add force to indicate knockout blow

        }


    }
}
