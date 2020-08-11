﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimiter : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CameraController.CameraDirection _direction;
    private void OnBecameVisible()
    {
        Debug.Log(_direction + " visible");
        _cameraController.SetDirectionBool(_direction, true);
    }

    private void OnBecameInvisible()
    {
        Debug.Log(_direction + " invisible");
        _cameraController.SetDirectionBool(_direction, false);
    }

    public void Invert()
    {
        //########################################
        switch (_direction)
        {
            case CameraController.CameraDirection.Right:
                {
                    _direction = CameraController.CameraDirection.Left;
                    break;
                }
            case CameraController.CameraDirection.Left:
                {
                    _direction = CameraController.CameraDirection.Right;
                    break;
                }
        }
    }
}
