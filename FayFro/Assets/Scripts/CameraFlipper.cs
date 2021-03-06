﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlipper : MonoBehaviour
{
    [SerializeField] private Transform _apllicationTransform;
    [SerializeField] private CharacterController2D _characterController;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private bool _flip = false;
    [SerializeField] private Transform _cameraLimiters;

    private void Start()
    {
        int startFlip = PlayerPrefs.GetInt("CameraFlip");
        if (startFlip == 0)
            StartCoroutine(StartFlip());
        if(_flip == true)
            StartCoroutine(StartFlip());
    }

    private IEnumerator StartFlip()
    {
        yield return new WaitForEndOfFrame();


        DefaultFlip();
    }
    
    private void DefaultFlip()
    {
        _apllicationTransform.Rotate(0f, 180f, 0f);
        _characterController.CameraFlip();
        _cameraTransform.Rotate(0f, 180f, 0f);
        _cameraLimiters.localScale = new Vector3(_cameraLimiters.localScale.x * -1, _cameraLimiters.localScale.y, _cameraLimiters.localScale.z);

        CameraLimiter[] limiters = GameObject.FindObjectsOfType<CameraLimiter>();
        foreach(CameraLimiter limiter in limiters)
        {
            limiter.Invert();
        }
    }
    public void Flip()
    {

        DefaultFlip();
        int oldFlip = PlayerPrefs.GetInt("CameraFlip");
        if(oldFlip == 0)
            PlayerPrefs.SetInt("CameraFlip", 1);
        else
            PlayerPrefs.SetInt("CameraFlip", 0);
    }
}
