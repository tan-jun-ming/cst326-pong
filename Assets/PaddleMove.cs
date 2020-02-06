using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMove : MonoBehaviour
{
    public Transform paddle;
    public int player;

    public Transform ball;

    public int affliction = 0;
    // 1: reversed
    // 2: sticky
    // 3: long
    // 4: really short

    private float affliction_timer = 0;

    string vertical;
    string fire;
    int speed = 5;
    bool paddle_locked = false;
    bool holding = false;
    float x_pos;

    // Start is called before the first frame update
    void Start()
    {
        vertical = string.Format("P{0}_Vertical", player);
        fire = string.Format("P{0}_Fire", player);

        x_pos = paddle.position.x;
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

    public void hold()
    {
        holding = true;
    }

    public void release()
    {
        holding = false;
        ((BallCollide)ball.gameObject.GetComponent(typeof(BallCollide))).release();
    }

    public void set_affliction(int new_affliction)
    {
        set_affliction(new_affliction, 500);
    }

    public void set_affliction(int new_affliction, int duration)
    {
        affliction = new_affliction;
        affliction_timer = duration;
        if (holding && new_affliction != 2)
        {
            release();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (affliction_timer > 0)
        {
            affliction_timer--;
        }

        if (affliction_timer == 0)
        {
            set_affliction(0);
        }

        if (paddle_locked)
        {
            return;
        }

        Vector3 vec = paddle.position;
        float delta_z = Input.GetAxis(vertical);

        float final_delta_z = vec.z;

        if (affliction == 1)
        {
            delta_z = -delta_z;
        }

        delta_z = delta_z * Time.deltaTime * speed;

        vec.z = paddle.position.z + delta_z;

        Vector3 scale_vec = Vector3.one * 0.5f;

        switch (affliction)
        {
            case 3: scale_vec.z = 5f; vec.x = x_pos; break;
            case 4: scale_vec.x = 3f; vec.x = (Mathf.Abs(x_pos) - 1f) * (x_pos < 0 ? -1 : 1); break;
            default: scale_vec.z = 3f; vec.x = x_pos; break;
        }

        paddle.localScale = scale_vec;

        float boundary = 5f;

        switch (paddle.localScale.z)
        {
            case 0.5f: boundary = 4.6f; break;
            case 3f: boundary = 3.4f; break;
            case 5f: boundary = 2.4f; break;
        }

        if (vec.z > boundary)
        {
            vec.z = boundary;
        } else if (vec.z < -boundary)
        {
            vec.z = -boundary;
        }

        paddle.position = vec;

        final_delta_z = vec.z - final_delta_z;

        if (affliction == 2 && holding)
        {
            if (Input.GetAxis(fire) > 0)
            {
                release();
            } else
            {
                ball.position += Vector3.forward * final_delta_z;
            }
        }

        

    }
}
