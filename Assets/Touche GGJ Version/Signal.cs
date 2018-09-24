using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour {

    public GameObject boneImIn;
    private Vector3 bonePosition;
    private Vector3 offset;
    public int numberOfBonesImIn = 0;
    public float vSpeed = 0;
    public float hSpeed = 0;
    public Vector3 lastPosition;
    public Vector3 direction;


    bool inPortal = false;
	// Use this for initialization
	void Start () {
        lastPosition = transform.position;
        //boneImIn = GameObject.FindGameObjectWithTag("Head");
        transform.parent= GameObject.FindGameObjectWithTag("Head").transform;
        //offset = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position!=lastPosition)
        {
            direction = (transform.position - lastPosition).normalized;
            lastPosition = transform.position;
        }

        //if (bonePosition!=boneImIn.transform.position)
        //{
        //    offset = bonePosition - boneImIn.transform.position;
        //    bonePosition = boneImIn.transform.position;
        //}

        vSpeed = Input.GetAxis("Vertical")*Time.deltaTime;
        hSpeed = Input.GetAxis("Horizontal")*Time.deltaTime;
        transform.Translate(new Vector3(hSpeed, vSpeed));
        //transform.Translate(offset);
        //offset = new Vector3(0, 0, 0);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Portal")
        {

            transform.parent = collision.gameObject.transform;
        }

    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{

    //    numberOfBonesImIn--;
    //    if (numberOfBonesImIn == 0)
    //    {
    //        boneImIn.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * 100, transform.position);
    //        Destroy(gameObject);
    //    }

    //    if (!inPortal)
    //    {
    //        boneImIn.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * 100, transform.position);
    //        Destroy(gameObject);
    //    }
    //    if (collision.gameObject.tag == "Portal")
    //    {
    //        inPortal = false;
    //    }
    //}




}
