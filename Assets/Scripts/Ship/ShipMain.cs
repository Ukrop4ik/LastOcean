using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMain : Photon.MonoBehaviour {

    private ShipManager _shipManager;
    [SerializeField]
    private ShipOnlineType _onlineType;
    private MoveController _movecontroller;
    [SerializeField]
    private List<Slot> _slots = new List<Slot>();
    private ShipStat Stats;
    [SerializeField]
    private Transform _target;

    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
    private double _LastNetworkDataTime = 0f;

    



    private void Start()
    {
        if (photonView.isMine)
        {
            _shipManager = GameObject.FindGameObjectWithTag("Context").GetComponent<ShipManager>();
            _shipManager.AddShip(this);
            Stats = gameObject.GetComponent<ShipStat>();
            _movecontroller = gameObject.GetComponent<MoveController>();
            _movecontroller.SetParameters(this, GetRigidbody(), Stats.GetAngularSpeed());
        }

    }


    public void SetTarget(Transform target)
    {
        _target = target;
        foreach (Slot slot in _slots)
        {
            if (slot.GetWeapon() != null)
                slot.GetWeapon().SetTarget(target);
        }
    }
    public MoveController GetMoveController()
    {
        return _movecontroller;
    }
    public ShipOnlineType GetOnlineType()
    {
        return _onlineType;
    }
    public void SetOnlineType(ShipOnlineType type)
    {
        _onlineType = type;
    }
    public ShipStat GetStats()
    {
        return Stats;
    }
    public Rigidbody GetRigidbody()
    {
        return gameObject.GetComponent<Rigidbody>();
    }
    public List<Weapon> GetWeaponOnSide(ShipSide side)
    {
        List<Weapon> weaponsinside = new List<Weapon>();

        foreach(Slot slot in _slots)
        {
            if (slot.GetWeapon() != null && slot.GetSlotSide() == side)
                weaponsinside.Add(slot.GetWeapon());
        }

        return weaponsinside;
    }
    public void SetDamage(float value)
    {
        Stats.SetHullValue(Stats.GetHullValue() - value);
        if (Stats.GetHullValue() <= 0)
            Dead();
    }
    private void Dead()
    {
        _shipManager.RemoveShip(this);
        Destroy(gameObject);
    }

   
}
