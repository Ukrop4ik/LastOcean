using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AxisShared : MonoBehaviour
{
    [SerializeField]
    private string _axis;
    [SerializeField]
    private float _sens;
    private float _targetValue;

    public void SetTargetValue(float targetValue)
    {
        _targetValue = targetValue;
    }

    private void Update ()
    {

        float newAxisValue = Mathf.MoveTowards(CrossPlatformInputManager.GetAxis(_axis),
                                                _targetValue,
                                                Time.deltaTime * _sens);
        CrossPlatformInputManager.SetAxis(_axis, newAxisValue);

    }
}
