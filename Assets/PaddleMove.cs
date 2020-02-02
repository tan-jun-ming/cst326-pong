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
    bool paddle_locked = false;

    // Start is called before the first frame update
    void Start()
    {
        horizontal = string.Format("P{0}_Horizontal", player);
        vertical = string.Format("P{0}_Vertical", player);

    }

    public void reset()
    {
        Vector3 vec = paddle.position;
        vec.z = 0;
        paddle.position = vec;
    }

    public void lock_paddle()
    {
        reset();
        paddle_locked = true;
    }

    public void unlock_paddle()
    {
        paddle_locked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (paddle_locked)
        {
            return;
        }

        Vector3 vec = paddle.position;
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
