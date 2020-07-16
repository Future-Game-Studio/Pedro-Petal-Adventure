﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D), typeof(PlatformEffector2D))]
public class Platform : MonoBehaviour
{
    private Collider2D collider = null;


    private void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(collision.gameObject.GetComponent<CharacterController2D>() != null)
            {
                if (collider.isTrigger == false)
                {
                    collider.isTrigger = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterController2D>() != null)
        {
            collider.isTrigger = false;
        }
    }

}
