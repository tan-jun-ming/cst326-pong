using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollide : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            rb.velocity = new Vector3(5, 0, 3);
        }
        if (Input.GetKey(KeyCode.X))
        {
            rb.velocity = new Vector3(-5, 0, -3);
        }
    }
}
