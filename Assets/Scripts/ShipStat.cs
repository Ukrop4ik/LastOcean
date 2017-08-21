﻿using System.Collections;
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
    [SerializeField]
    private float _hullMaximumHP;
    [SerializeField]
    private float _hullCurrentHP;
    [SerializeField]
    private float _armorMaximumHP;
    [SerializeField]
    private float _armorCurrentHP;

    private void Start()
    {
        _hullCurrentHP = _hullMaximumHP;
        _armorCurrentHP = _armorMaximumHP;
    }

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
    public float GetHullValue(bool isMaximum = false)
    {
        return isMaximum ? _hullMaximumHP : _hullCurrentHP;
    }
    public float GetArmorValue(bool isMaximum = false)
    {
        return isMaximum ? _armorMaximumHP : _armorCurrentHP;
    }
    public void SetHullValue(float value, bool isMaximum = false)
    {
        if (isMaximum)
            _hullMaximumHP = value;
        else
            _hullCurrentHP = value;
    }
    public void SetArmorValue(float value, bool isMaximum = false)
    {
        if (isMaximum)
            _armorMaximumHP = value;
        else
            _armorCurrentHP = value;
    }

}
