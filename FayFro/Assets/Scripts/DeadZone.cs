﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadZone : MonoBehaviour
{
    static private Transform _checkPoint = null;

    public static void SetCheckPoint(Transform transform)//when triggered
    {
        _checkPoint = transform;
    }
    private void Start()
    {

        _checkPoint = GameObject.Find("CheckPoint(0)").gameObject.transform;
    }


    public delegate void Reset();
    public static event Reset OnReset;
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerReset>() != null)
        {
            collision.gameObject.GetComponent<PlayerReset>().ResetPlayer(_checkPoint);

            ResetObject[] resetObjects = FindObjectsOfType<ResetObject>();
            foreach(ResetObject obj in resetObjects)
            {
                obj.Reset();
            }

            OnReset?.Invoke();
        }
        else if(collision.gameObject.GetComponent<ResetObject>() != null)
        {
            collision.gameObject.GetComponent<ResetObject>().Reset();
        }
    }
}
