using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    [SerializeField]
    private SlotType _slotType;

    [SerializeField]
    private float _weaponAngleMax_Y;

    public float GetWeaponAngleMinMax()
    {
        return _weaponAngleMax_Y;
    }
}
