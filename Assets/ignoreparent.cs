using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoreparent : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), transform.parent.gameObject.GetComponent<Collider2D>(), true);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject == transform.parent.gameObject)
    //    {
    //        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), transform.parent.gameObject.GetComponent<Collider2D>());
    //    }
    //}
}
