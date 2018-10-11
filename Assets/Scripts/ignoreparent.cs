using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoreparent : MonoBehaviour {

	// Use this for initialization
	void Start () {

        DisableParentCols();

    }

    void DisableParentCols()
    {
        //Disable colllision with direct parent
        //EG, hand - lower arm, lower arm - upper arm, upper arm - body, etc
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), transform.parent.gameObject.GetComponent<Collider2D>(), true);
    }

}
