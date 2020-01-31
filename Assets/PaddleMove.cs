using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMove : MonoBehaviour
{
    public Transform paddle;
    public int player;

    string horizontal;
    string vertical;

    // Start is called before the first frame update
    void Start()
    {
        horizontal = string.Format("P{0}_Horizontal", player);
        vertical = string.Format("P{0}_Vertical", player);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = new Vector3();
        int speed = 5;

        vec.x = Input.GetAxis(horizontal) * Time.deltaTime * speed;
        vec.z = Input.GetAxis(vertical) * Time.deltaTime * speed;

        paddle.position += vec;

    }
}
