using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetter : MonoBehaviour
{
    private Vector3 pos;
    private Quaternion rot;

    // Use this for initialization
    void Start()
    {
        InitialPositions();
    }

    void InitialPositions()
    {
        pos = transform.position;
        rot = transform.rotation;
    }

    //Reset limb to position, roation from start of instance. Reset velocity to 0
    public void ResetTransform()
    {
        transform.position = pos;
        transform.rotation = rot;
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
