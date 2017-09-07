using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreLinq;

public class BotAI : MonoBehaviour {

    public enum AiLogick
    {
        AttackPlayer,
        AttackAll,
        DefencePlayer
    }
    public enum BotState
    {
        Idle
    }
    public AiLogick Logick;
    public BotState State;

    [SerializeField]
    private ShipMain _target;
    private ShipMain _ship;
    [SerializeField]
    private MoveController _movecontroller;
    [SerializeField]
    private List<BotTargetData> _targets = new List<BotTargetData>();
    [SerializeField]
    private List<Weapon> _myWeapons = new List<Weapon>();
    private bool enable = false;
    BotTargetData closesttarget;
    [SerializeField]
    private float _agroRange;

    private void Start()
    {
        _ship = gameObject.GetComponent<ShipMain>();

        if (_ship.GetOnlineType() == ShipOnlineType.Bot)
        {

            _movecontroller = gameObject.GetComponent<MoveController>();

            enable = true;

            foreach (Slot slot in _ship.GetSlots())
            {
                slot.SetUse(true);
            }
            
            StartCoroutine(BotUpdateStatus());

            if(Logick == AiLogick.DefencePlayer)
              _target = ShipManager.GetPlayerShip();
        }
    }

    private void Update()
    {
        if (!enable) return;
        if (!_target) return;

        
        switch(Logick)
        {
            case AiLogick.AttackPlayer:
                if(CheckAgroRange())
                {
                    Rotation();
                    Move();
                }
                break;
            case AiLogick.AttackAll:
                {
                    Rotation();
                    Move();
                }
                break;
            case AiLogick.DefencePlayer:
                {
                    Rotation();
                    Move();
                }
                break;

            default:
                break;
        }

    }

    private bool CheckAgroRange()
    {
        return closesttarget.Dist < _agroRange;
    }

    private void Rotation()
    {
        float angle = Vector3.Angle(transform.transform.forward, _target.transform.position - transform.transform.position);
        Vector3 croos = Vector3.Cross(transform.transform.forward, _target.transform.position - transform.transform.position);

        if (croos.y > 0)
            angle = -angle;

        if (closesttarget.Dist < 25)
        {
            _movecontroller.RotationKeyboard(angle > 45 ? -1 : 1);
        }
        if (closesttarget.Dist < 18)
        {
            _movecontroller.RotationKeyboard(angle > 90 ? -1 : 1);
        }
        else
        {
            _movecontroller.RotationKeyboard(angle > 0 ? -1 : 1);
        }
    }
    private void Move()
    {
        if (closesttarget.Dist > 40)
            _movecontroller.SetEnginPower(1);
        if (closesttarget.Dist < 30)
            _movecontroller.SetEnginPower(1f);
        if (closesttarget.Dist < 15)
            _movecontroller.SetEnginPower(0.5f);
    }
    private IEnumerator BotUpdateStatus()
    {
        yield return new WaitForSeconds(0.5f);

        _targets.Clear();


        switch (Logick)
        {
            case AiLogick.AttackPlayer:
                if (ShipManager.GetPlayersShips().Count > 0)
                {
                    foreach (ShipMain ship in ShipManager.GetPlayersShips())
                    {
                        if (ship)
                            _targets.Add(new BotTargetData(ship, Vector3.Distance(this.transform.position, ship.transform.position)));
                    }

                    if (_targets.Count > 0)
                    {

                        closesttarget = _targets.MinBy(x => x.Dist);
                        _target = closesttarget.Ship;
                        _ship.SetTarget(_target.transform);
                        foreach (Weapon weapon in _myWeapons)
                        {
                            weapon.SetTarget(_target.transform);
                        }
                    }
                }
                break;
            case AiLogick.AttackAll:
                if (ShipManager.GetAllShips().Count > 0)
                {
                    foreach (ShipMain ship in ShipManager.GetAllShips())
                    {
                        if (ship == _ship) continue;

                        if (ship)
                            _targets.Add(new BotTargetData(ship, Vector3.Distance(this.transform.position, ship.transform.position)));
                    }

                    if (_targets.Count > 0)
                    {

                        closesttarget = _targets.MinBy(x => x.Dist);
                        _target = closesttarget.Ship;
                        _ship.SetTarget(_target.transform);
                        foreach (Weapon weapon in _myWeapons)
                        {
                            weapon.SetTarget(_target.transform);
                        }
                    }
                }
                break;
            case AiLogick.DefencePlayer:
                if (ShipManager.GetAllShips().Count > 0)
                {
                    foreach (ShipMain ship in ShipManager.GetAllShips())
                    {
                        if (ship == _ship) continue;
                        if (ship.GetOnlineType() == ShipOnlineType.Player) continue;

                        if (ship)
                            _targets.Add(new BotTargetData(ship, Vector3.Distance(this.transform.position, ship.transform.position)));
                    }

                    if (_targets.Count > 0)
                    {

                        closesttarget = _targets.MinBy(x => x.Dist);

                        if(CheckAgroRange())
                        {
                            if (!_target) break;
                            if (_target.GetOnlineType() != ShipOnlineType.Player)
                            {
                                _target = closesttarget.Ship;
                                _ship.SetTarget(_target.transform);
                            }
                        }
                        else
                        {
                            _target = ShipManager.GetPlayerShip();
                        }

                        foreach (Weapon weapon in _myWeapons)
                        {   if (!_target) break;

                            if(_target.GetOnlineType() == ShipOnlineType.Bot)
                                 weapon.SetTarget(_target.transform);
                        }
                    }
                }
                break;

            default:
                break;
        }

       
        StartCoroutine(BotUpdateStatus());
    }

    [System.Serializable]
    private class BotTargetData
    {
        public ShipMain Ship;
        public float Dist;

        public BotTargetData(ShipMain ship, float dist)
        {
            Ship = ship;
            Dist = dist;
        }
    }
}
