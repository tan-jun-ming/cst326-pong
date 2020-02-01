using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMove : MonoBehaviour
{
    public Transform paddle;
    public int player;

    string horizontal;
    string vertical;
    int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        horizontal = string.Format("P{0}_Horizontal", player);
        vertical = string.Format("P{0}_Vertical", player);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = paddle.position;
        //vec.x = Input.GetAxis(horizontal) * Time.deltaTime * speed;
        float delta_z = Input.GetAxis(vertical) * Time.deltaTime * speed;

        // define boundaries
        if (paddle.position.z >= 0)
        {
            vec.z = Mathf.Min(paddle.position.z + delta_z, 3.4f);
        } else
        {
            vec.z = Mathf.Max(paddle.position.z + delta_z, -3.4f);
        }
        paddle.position = vec;

    }
}
