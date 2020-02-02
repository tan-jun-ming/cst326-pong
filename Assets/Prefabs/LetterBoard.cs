﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoard : MonoBehaviour
{
    public GameObject letternix_prefab;
    public Transform me;
    public string start_message;

    public int align_style;
    // 0: Left
    // 1: Right
    // 2: Center

    private const float spacing = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        string message = "";
        if (start_message != null)
        {
            message = start_message;
        }
        display_message(message);

    }

    public void clear()
    {
        display_message("");
    }
    public void display_message(string msg)
    {
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
                    break;
                }
            }

        }

        me.localScale = old_scale;
    }
}
