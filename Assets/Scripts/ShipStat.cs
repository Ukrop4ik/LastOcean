using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStat : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 10f)]
    private float _maximumSpeed;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _angularspeed;

    public float GetSpeed()
    {
        return _maximumSpeed;
    }
    public float GetAcceleration()
    {
        return _acceleration;
    }
    public float GetAngularSpeed()
    {
        return _angularspeed;
    }


}
