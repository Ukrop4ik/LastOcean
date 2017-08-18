using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class InputAxisShared : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private AxisShared _sharedAxis;
    [SerializeField]
    private float _targetValue;

    public void OnPointerDown(PointerEventData data)
    {
        _sharedAxis.SetTargetValue(_targetValue);
    }


    public void OnPointerUp(PointerEventData data)
    {
        _sharedAxis.SetTargetValue(0); // no input
    }
}
