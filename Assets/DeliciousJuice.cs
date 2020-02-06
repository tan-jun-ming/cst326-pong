using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliciousJuice : MonoBehaviour
{
    public ParticleSystem particle_system;
    public GameObject ball;

    public Camera[] cameras;
    public GameObject[] paddles;

    private int camera_count = 0;

    public bool particles_juice = false;
    public bool ball_spin_juice = false;

    private BallCollide ballcollide;
    private Transform balltransform;

    private float hue = 0;

    private float pulsate_delta = 0.01f;
    private float pulsate_level = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ballcollide = (BallCollide)ball.GetComponent(typeof(BallCollide));
        balltransform = ball.transform;

        swap_camera();

    }

    void swap_camera()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = i == camera_count;
        }
    }
    // Update is called once per frame
    void Update()
    {
        cameras[2].gameObject.transform.RotateAround(Vector3.zero, Vector3.up, -1);
        cameras[3].gameObject.transform.RotateAround(Vector3.zero, Vector3.left, -1);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            camera_count++;
            if (camera_count >= cameras.Length)
            {
                camera_count = 0;
            }

            swap_camera();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            particles_juice = !particles_juice;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ball_spin_juice = !ball_spin_juice;
        }

        if (particles_juice && !ballcollide.get_endstate())
        {
            DoEmit();

        }

        if (ball_spin_juice && !ballcollide.get_endstate())
        {
            DoBallSpin();
        }

        if (pulsate_level >= 1 || pulsate_level <= 0.5)
        {
            pulsate_delta = -pulsate_delta;
        }

        pulsate_level += pulsate_delta;

        foreach (GameObject paddle in paddles)
        {
            Color paddle_color = Color.white;
            int affliction = ((PaddleMove)paddle.gameObject.GetComponent(typeof(PaddleMove))).affliction;

            if (affliction != 0)
            {
                float hue = 0f;
                switch (affliction)
                {
                    case 1: hue = 0.7f; break;
                    case 2: hue = 0.25f; break;
                    case 3: hue = 0.6f; break;
                    case 4: hue = 0f; break;
                }
                
                paddle_color = Color.HSVToRGB(hue, pulsate_level, 1);
            }

            ((MeshRenderer)paddle.gameObject.GetComponent(typeof(MeshRenderer))).material.color = paddle_color;
        }

    }

    float random_angle(float num)
    {
        return Random.Range(-num, num);
    }
    void DoEmit()
    {
        for (int i=0; i<10; i++)
        {
            ParticleSystem.EmitParams emit_params = new ParticleSystem.EmitParams();
        
            Vector3 base_vel = -ballcollide.get_direction();
            Vector3 new_vel = Quaternion.Euler(0, random_angle(15), random_angle(15)) * base_vel;
            emit_params.position = base_vel * 0.3f + balltransform.position;
            emit_params.velocity = new_vel * 10;
            emit_params.startLifetime = 0.5f;
            emit_params.startSize3D = Vector3.one * 0.5f;

            emit_params.startColor = Random.ColorHSV(hue, hue, 0.9f, 1f, 0.5f, 1f);
            particle_system.Emit(emit_params, 1);


        }

        hue += 0.01f;
        if (hue >= 1)
        {
            hue = 0;
        }

    }

    void DoBallSpin()
    {
        balltransform.Rotate(0, -5, 0);
    }
}
