using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signalfollower : MonoBehaviour {

    public GameObject boneImIn;
    private Vector3 bonePosition;
    private Vector3 offset;
    public int numberOfBonesImIn = 0;

    public Vector3 lastPosition;
    public Vector3 direction;
    bool inPortal = false;

    GameObject signal;

    // Use this for initialization
    void Start () {
        lastPosition = transform.position;
        signal = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position != lastPosition)
        {
            direction = (transform.position - lastPosition).normalized;
            lastPosition = transform.position;
        }
        if (signal==null)
        {
            signal = GameObject.FindGameObjectWithTag("Player");
        }
        if (signal!=null)
        {
            transform.position = signal.transform.position;
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Portal")
        {
            boneImIn = collision.gameObject;

        }

        numberOfBonesImIn++;
        if (collision.gameObject.tag == "Portal")
        {
            inPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        numberOfBonesImIn--;
        if (numberOfBonesImIn == 0)
        {
            boneImIn.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * 100, transform.position);
            Destroy(signal);
        }

        if (!inPortal)
        {
            boneImIn.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * 100, transform.position);
            Destroy(signal);
        }
        if (collision.gameObject.tag == "Portal")
        {
            inPortal = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
        }
    }
}
