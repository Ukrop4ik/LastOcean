using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class InputAxisShared : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private AxisShared _sharedAxis;
    [SerializeField]
    private float _targetValue;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sharedAxis.SetTargetValue(_targetValue);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _sharedAxis.SetTargetValue(0);
    }


}
