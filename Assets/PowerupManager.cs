using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{

    public DeliciousJuice juice;
    public GameManager game_manager;
    public BallCollide ball;
    GameObject powerup;

    private int affliction = 0;

    // Start is called before the first frame update
    void Start()
    {
        powerup = gameObject;
    }

    public void show()
    {
        show(Random.Range(1, 5), Random.Range(-4.5f, 4.5f));
    }

    public void show(int color, float pos)
    {
        affliction = color;

        Color new_color = Color.HSVToRGB(juice.get_hue(color), 0.8f, 1);

        foreach (Transform child in powerup.transform)
        {
            MeshRenderer renderer = (MeshRenderer)child.gameObject.GetComponent(typeof(MeshRenderer));
            renderer.enabled = true;
            renderer.material.color = new_color;

            ((BoxCollider)child.gameObject.GetComponent(typeof(BoxCollider))).enabled = true;

        }

        powerup.transform.position = Vector3.forward * pos;


    }

    public void hide()
    {
        foreach (Transform child in powerup.transform)
        {
            ((MeshRenderer)child.gameObject.GetComponent(typeof(MeshRenderer))).enabled = false;
            ((BoxCollider)child.gameObject.GetComponent(typeof(BoxCollider))).enabled = false;

        }
    }

    void OnTriggerEnter(Collider collider)
    {
        game_manager.set_powerup(ball.get_player(), affliction);
        hide();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            show();
        }
    }
}
