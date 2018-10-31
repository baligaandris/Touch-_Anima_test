using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdLevelPoints : MonoBehaviour {
    float minVelocityX, minVelocityY;
    //How many turns this piece is disabled for
    public float disabled = 0;
    //is selected
    bool selected;
    public void setSelected(bool newSelected) { selected = newSelected; }

    private void Start()
    {
        matchmanager mm = GameObject.Find("MatchManager").GetComponent<matchmanager>();
        minVelocityX = mm.getMinVelocityX();
        minVelocityY = mm.getMinVelocityY();
    }

    private void Update()
    {
        UpdateSpotColor();
    }

    private void UpdateSpotColor()
    {
        switch ((int)disabled)
        {
            case 0:
                if(selected)
                    GetComponent<SpriteRenderer>().color = Color.green;
                else
                    GetComponent<SpriteRenderer>().color = Color.white;
                break;
            default:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If collision between this hand/foot and another hand/foot
        //Check no impact between parts on the same character & that the colliding part is not disabled
        if(collision.gameObject.tag == "impactSpot" && transform.root != collision.transform.root && collision.transform.GetComponent<thirdLevelPoints>().disabled <= 0)
        {
            
            //Get velocity of collision
            float colVelocityX, colVelocityY;
            //Get velocity from collision obj
            colVelocityX = collision.transform.GetComponent<Rigidbody2D>().velocity.x;
            colVelocityY = collision.transform.GetComponent<Rigidbody2D>().velocity.y;
            print(gameObject.name + colVelocityX);
            //Check for a collision where the velocity passes the required speed
            if (colVelocityX >= minVelocityX || colVelocityX <= -minVelocityX || 
                colVelocityY >= minVelocityY || colVelocityY <= -minVelocityY)
            {
                //Passed
                //Set this part to now disabled
                disabled = 2;
            }
        }
    }
}
