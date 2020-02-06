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

    public bool immortal = false;
    public bool immobile = false;

    private float speed;
    private float speed_delta = 0.01f;
    private Vector3 direction;
    private int delay_time;
    private bool end_state = false;

    private GameManager gamemanager;


    // Start is called before the first frame update
    void Start()
    {
        gamemanager = (GameManager)gamemanager_obj.GetComponent(typeof(GameManager));
        reset(1);
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

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);

        Vector3 normal = contact.normal;

        if (contact.otherCollider.CompareTag("paddle"))
        {
            ballspeaker.clip = paddle_hit[Random.Range(0, paddle_hit.Length - 1)];

            speed += speed_delta;
            float rel_pos = contact.otherCollider.transform.InverseTransformPoint(Vector3.zero).z;
            rel_pos = -((rel_pos) * 45f);

            normal = Quaternion.AngleAxis(rel_pos, Vector3.up) * normal;

        } else if (!immortal && contact.otherCollider.CompareTag("kill_wall")){
            gamemanager.round_over(contact.otherCollider.name);
        } else
        {
            ballspeaker.clip = table_hit[Random.Range(0, table_hit.Length - 1)];
        }

        ballspeaker.Play();

        direction = Vector3.Reflect(direction, normal);

        if (normal.x != 0)
        {
            direction.x = Mathf.Abs(direction.x) * normal.x > 0 ? 1 : -1;
        }

        direction = Vector3.Normalize(direction);


    }

}
