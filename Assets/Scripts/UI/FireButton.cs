using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private HUD _hud;
    [SerializeField]
    private bool isLeft;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isLeft)
        {
            _hud.Lfire = true;
        }
        else
        {
            _hud.Rfire = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLeft)
        {
            _hud.Lfire = false;
        }
        else
        {
            _hud.Rfire = false;
        }
    }

    private void Update()
    {

    }

   

}
