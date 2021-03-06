﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoard : MonoBehaviour
{
    public GameObject letternix_prefab;
    public Transform me;
    public Material material = null;

    public int align_style;
    // 0: Left
    // 1: Right
    // 2: Center

    private const float spacing = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in me)
        {
            if (child.name == "Placeholder")
            {
                GameObject.Destroy(child.gameObject);
                break;
            }
        }
    }

    public void clear()
    {
        display_message("");
    }

    public void display_message(string msg)
    {
        display_message(msg, Color.black);
    }

    public void display_message(string msg, Color text_color)
    {
        msg = msg.ToLower();

        foreach (Transform child in me)
        {
            GameObject.Destroy(child.gameObject);
        }

        Vector3 old_scale = me.localScale;

        me.localScale = Vector3.one;

        int message_length = msg.Length - 1;
        float offset = 0;

        switch (align_style)
        {
            case 1: offset = (spacing * ((float)message_length + 0.5f)) ; break;
            case 2: offset = ((spacing * (float)message_length) / (float)2); break;
            default: break;
        }

        int counter = -1;
        foreach (char c in msg)
        {
            counter++;

            if (c == " "[0])
            {
                continue;
            }
            GameObject letternix = Instantiate(letternix_prefab);
            letternix.transform.SetParent(me);

            letternix.transform.position = me.transform.position + (Vector3.left * offset) + (Vector3.right * counter * spacing);


            for (int i = 0; i < letternix.transform.childCount; i++)
            {
                GameObject child = letternix.transform.GetChild(i).gameObject;

                if (child.transform.name == string.Format("letter_{0}", c))
                {
                    child.SetActive(true);
                    ((MeshRenderer)child.GetComponent(typeof(MeshRenderer))).material.color = text_color;

                    break;
                }
            }

        }

        me.localScale = old_scale;
    }
}
