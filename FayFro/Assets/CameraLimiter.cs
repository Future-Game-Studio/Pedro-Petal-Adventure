﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimiter : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CameraController.CameraDirection _direction;
    private void OnBecameVisible()
    {
        _cameraController.SetDirectionBool(_direction, true);
    }

    private void OnBecameInvisible()
    {
        _cameraController.SetDirectionBool(_direction, false);
    }
}
