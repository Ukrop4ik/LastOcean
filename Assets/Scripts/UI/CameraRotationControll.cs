using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotationControll : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject _cameraRotator;


    private void OnEnable()
    {
        _cameraRotator = Camera.main.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {


    
    }
}
