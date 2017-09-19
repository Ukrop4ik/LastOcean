﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Photon.MonoBehaviour {

    public enum RotationAxis
    {
        Horisontal,
        Vertical
    }

    public string weaponid;
    #region("PARAMS")

    [SerializeField]
    private float _MaxDist;
    [SerializeField]
    private float _bulletSpeed;
    [SerializeField]
    private float _damageMIN;
    [SerializeField]
    private float _damageMAX;
    public float _reloadspeed;
    [SerializeField]
    public int _ammocountMax;
    [SerializeField]
    private float _reloadTime = 0f;
    [SerializeField]
    private float _angularSpeed;

    #endregion

    [SerializeField]
    private Slot _slot;
    [SerializeField]
    private float _MinMaxAngle_Y;

    private float _angleY;
    [SerializeField]
    private Transform _target;
    private float _angleTest = 0f;


    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private Transform _firepoint;
    [SerializeField]
    private GameObject _tower;
    public bool isCanShoot;

    [SerializeField]
    private PhotonView _photonView;

    public float _reloadspeedCurr;

    [HideInInspector]
    public int _ammocount;

    private float _shootTimeBufer;
    [SerializeField]
    private bool _targetinline;

    public bool _isReloading = false;

    private bool _testAngle;

    public bool _netPlayerShoot = false;

    Vector3 targetPos;

    public GameObject GetTower()
    {
        return _tower;
    }

    public Slot GetMySlot()
    {
        return _slot;
    }
    private void Start()
    {


        _slot = transform.GetComponentInParent<Slot>();
        _photonView = _slot.GetSlotShip().GetComponent<PhotonView>();
        _photonView.ObservedComponents.Add(_tower.GetComponent<ServerObj>());
        _slot.GetSlotShip().gameObject.GetComponent<PhotonView>().ObservedComponents.Add(this);


        if (_slot.GetSlotShip().GetOnlineType() != ShipOnlineType.Bot)
        {
            GameDB.WeaponDate param = GameDB.Instance().GetWeaponDate(weaponid);
            _MaxDist = param.DIST;
            _bulletSpeed = param.BULLETSPEED;
            _damageMIN = param.MINDAMAGE;
            _damageMAX = param.MAXDAMAGE;
            _reloadspeed = param.RELOADSPEED;
            _ammocountMax = (int)param.AMMO;
            _reloadTime = param.RPM;
            _angularSpeed = param.ANGULARSPEED;
        }
        _ammocount = _ammocountMax;
        _reloadspeedCurr = _reloadspeed;
        _MinMaxAngle_Y = _slot.GetWeaponAngleMinMax();
        _angleY = transform.localRotation.y;

    }
    public void SetTarget(Transform target)
    {
        _target = target;
    }
    private void Update()
    {
        if (_photonView.isMine)
        {
            if(_isReloading)
            {
                _reloadspeedCurr -= Time.deltaTime;
            }
            else
            {
                _reloadspeedCurr = _reloadspeed;
            }
            if (_shootTimeBufer > 0)
            {
                _shootTimeBufer -= Time.deltaTime;
            }
            else
            {
                _shootTimeBufer = 0f;
                isCanShoot = true;
            }

            if (_ammocount <= 0 && !_isReloading)
            {
                _isReloading = true;
                StartCoroutine(Reloading());
            }

            if (!_target) return;
            if (Vector3.Distance(_target.position, transform.position) > _MaxDist) return;

            RaycastHit hit;

            if (Physics.Raycast(_tower.transform.position, _tower.transform.forward, out hit, _MaxDist))
            {
                if (hit.transform == _target)
                {
                    _targetinline = true;
                    targetPos = hit.point;
                }
                else
                {
                    _targetinline = false;
                }
            }
            else
            {
                _targetinline = false;
            }
        }
        else
        {
            if (_targetinline)
            {
                _tower.transform.rotation = Quaternion.LookRotation(targetPos - _firepoint.position);
            }
        }

        RotateWeapon();
    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(_reloadspeed);

        _ammocount = _ammocountMax;

        _isReloading = false;
    }

    public void RotateWeapon()
    {
        //tower rotation
        // Debug.Log(Vector3.Distance(_target.position, transform.position));
        if (_photonView.isMine)
        {
            _testAngle = TestAngle(_MinMaxAngle_Y, _target, transform);
            if (!_testAngle) return;
            Vector3 targetdirection = _target.transform.position - _firepoint.position;
            Vector3 newDir = Vector3.RotateTowards(_tower.transform.forward, targetdirection, ((_angularSpeed * 0.1f) * Time.deltaTime), 0.0F);
            _tower.transform.rotation = Quaternion.LookRotation(newDir);
            Shoot();
        }

    }

    public bool TestAngle(float maxangle, Transform target, Transform self)
    {
        return Vector3.Angle(target.position - self.position, _slot.transform.forward) < maxangle;
    }

    public void Shoot()
    {
        if (_photonView.isMine)
        {
            if (!isCanShoot) return;
            if (!_slot._isCanUse) return;
            if (_isReloading) return;
            if (!_testAngle) return;

            _netPlayerShoot = true;
        }

        _shootTimeBufer = _reloadTime;
        GameObject bullet = Instantiate(_bullet, _firepoint.position, Quaternion.identity);

        bullet.GetComponent<Bullet>().CreateBullet(_slot.GetSlotShip(), _damageMIN, _damageMAX, _target, _MaxDist);
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(_firepoint.forward * _bulletSpeed, ForceMode.Impulse);

        _ammocount--;
        isCanShoot = false;


    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.Serialize(ref _targetinline);
        stream.Serialize(ref targetPos);

        if(stream.isWriting)
        {
            stream.SendNext(_netPlayerShoot);

            _netPlayerShoot = false;
        }
        else
        {
            _netPlayerShoot = (bool)stream.ReceiveNext();

            if(_netPlayerShoot)
            {
                Shoot();
            }
        }

    }


}
