using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollide : MonoBehaviour
{

    private float speed = 0.1f;
    //private float angle = -120f;
    private Vector3 direction = Vector3.left;

    public Transform ball;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 new_vector = Vector3.forward;
        //new_vector = Quaternion.AngleAxis(angle, Vector3.up) * new_vector * speed;
        //ball.position += new_vector;

        ball.position += direction * speed;

    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);

        Vector3 normal = contact.normal;
        if (contact.otherCollider.CompareTag("paddle"))
        {
            //speed += 0.005f;
            float rel_pos = contact.otherCollider.transform.InverseTransformPoint(Vector3.zero).z;
            rel_pos = -((rel_pos) * 45f);

            normal = Quaternion.AngleAxis(rel_pos, Vector3.up) * normal;


            //print(normal);
        } else
        {
            
        }
        Vector3 debugpoint = contact.point + Vector3.up;
        Debug.DrawRay(debugpoint, direction, Color.green, 50);
        direction = Vector3.Reflect(direction, normal);

        if (normal.x != 0)
        {
            direction.x = Mathf.Abs(direction.x) * normal.x > 0 ? 1 : -1;
        }

        direction = Vector3.Normalize(direction);
        Debug.DrawRay(debugpoint, normal, Color.red, 50);
        Debug.DrawRay(debugpoint, direction, Color.blue, 50);


        //print(contact.otherCollider.transform.position);

        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
        //    print(contact.point);
        //    print(contact.normal);


        //    // Visualize the contact point
        //    Debug.DrawRay(contact.point, contact.normal, Color.red);
        //}


    }

}
