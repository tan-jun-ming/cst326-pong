using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollide : MonoBehaviour
{
    public GameObject gamemanager_obj;

    public Transform ball;
    public AudioSource ballspeaker;

    public AudioClip[] table_hit;
    public AudioClip[] paddle_hit;
    public AudioClip ding;
    public AudioClip[] squish;

    public bool immortal = false;
    public bool immobile = false;

    private float speed;
    private float speed_delta = 0.01f;
    private Vector3 direction;
    private int delay_time;
    private bool end_state = false;
    private int player = 1;

    private GameManager gamemanager;


    // Start is called before the first frame update
    void Start()
    {
        gamemanager = (GameManager)gamemanager_obj.GetComponent(typeof(GameManager));
        reset(1);
    }

    public int get_player()
    {
        return player;
    }

    public Vector3 get_direction()
    {
        return direction;
    }

    public bool get_endstate()
    {
        return end_state;
    }

    public void delay(int frames)
    {
        delay_time += frames;
    }

    public void end_game()
    {
        reset(0);
        end_state = true;
    }

    public void reset(int dir)
    {
        ball.position = Vector3.zero;
        speed = 0.1f;
        direction = Vector3.right * dir;
        player = dir == 1 ? 1 : 2;
        release();
    }

    // Update is called once per frame
    void Update()
    {

        if (immobile)
        {
            return;
        } else if (delay_time > 0)
        {
            delay_time--;
            return;
        }

        ball.position += direction * speed;

    }

    public void release()
    {
        if (immobile)
        {
            immobile = false;
            ballspeaker.PlayOneShot(squish[1], 1f);
        }
    }


    void OnCollisionEnter(Collision collision)
    {

        ContactPoint contact = collision.GetContact(0);

        Vector3 normal = contact.normal;

        AudioClip to_play = null;
        if (contact.otherCollider.CompareTag("paddle"))
        {
            to_play = paddle_hit[Random.Range(0, paddle_hit.Length)];

            speed += speed_delta;

            float rel_pos = contact.otherCollider.transform.InverseTransformPoint(contact.point).z;
            rel_pos *= player == 1 ? 1 : -1;

            normal = Quaternion.AngleAxis((rel_pos * 45f), Vector3.up) * normal;

            PaddleMove paddlescript = (PaddleMove)contact.otherCollider.gameObject.GetComponent(typeof(PaddleMove));

            player = paddlescript.player;

            if (paddlescript.affliction == 2)
            {
                immobile = true;
                to_play = squish[0];
                paddlescript.hold();
            } else if (Mathf.Abs(rel_pos) < 0.1)
            {
                to_play = ding;
            }

        } else if (!immortal && contact.otherCollider.CompareTag("kill_wall")){
            gamemanager.round_over(contact.otherCollider.name);
        } else
        {
            to_play = table_hit[Random.Range(0, table_hit.Length)];
        }

        //print(speed);
        //if (speed > 0.2f && !immobile)
        //{
        //    to_play = ding;
        //}


        ballspeaker.pitch = speed * 10 - 0.1f;

        if (to_play != null)
        {
            ballspeaker.PlayOneShot(to_play, 1f);
        }


        direction = Vector3.Reflect(direction, normal);

        if (normal.x != 0)
        {
            direction.x = Mathf.Abs(direction.x) * normal.x > 0 ? 1 : -1;
        }

        direction = Vector3.Normalize(direction);


    }

}
