﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    [SerializeField]
    private SlotType _slotType;
    [SerializeField]
    private ShipSide _slotShipSide;
    [SerializeField]
    private ShipMain _ship;

    [SerializeField]
    private float _weaponAngleMax_Y;

    public bool _isCanUse { get; private set; }

    private void Start()
    {
       _ship = transform.root.GetComponent<ShipMain>();
    }

    public void SetUse(bool isCan)
    {
        _isCanUse = isCan;
    }
    public ShipSide GetSlotSide()
    {
        return _slotShipSide;
    }
    public ShipMain GetSlotShip()
    {
        return _ship;
    }
    public float GetWeaponAngleMinMax()
    {
        return _weaponAngleMax_Y;
    }

    public Weapon GetWeapon()
    {
        if (transform.childCount > 0)
            return transform.GetChild(0).gameObject.GetComponent<Weapon>();
        else
            return null;
    }
}
