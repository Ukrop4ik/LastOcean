using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public enum RotationAxis
    {
        Horisontal,
        Vertical
    }

    [SerializeField]
    private Slot _slot;
    [SerializeField]
    private float _MinMaxAngle_Y;
    [SerializeField]
    private float _angularSpeed;
    private float _angleY;
    [SerializeField]
    private Transform _target;
    private float _angleTest = 0f;
    [SerializeField]
    private float _reloadTime = 0f;
    [SerializeField]
    private float _MaxDist;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private float _bulletSpeed;
    [SerializeField]
    private Transform _firepoint;
    [SerializeField]
    private GameObject _tower;
    public bool isCanShoot;
    [SerializeField]
    private float _damage;

    private float _shootTimeBufer;

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
        _MinMaxAngle_Y = _slot.GetWeaponAngleMinMax();
        _angleY = transform.localRotation.y;
    }
    public void SetTarget(Transform target)
    {
        _target = target;
    }
    private void Update()
    {
        if (_shootTimeBufer > 0)
        {
            _shootTimeBufer -= Time.deltaTime;
        }
        else
        {
            _shootTimeBufer = 0f;
            isCanShoot = true;
        }

        if (!_target) return;
        if (Vector3.Distance(_target.position, transform.position) > _MaxDist) return;

        RotateWeapon();
    }

    public void RotateWeapon()
    {
        //tower rotation
       // Debug.Log(Vector3.Distance(_target.position, transform.position));
        if (!TestAngle(_MinMaxAngle_Y, _target, transform)) return;
        Vector3 targetdirection = _target.transform.position - _firepoint.position;
        Vector3 newDir = Vector3.RotateTowards(_tower.transform.forward, targetdirection, ((_angularSpeed * 0.1f) * Time.deltaTime), 0.0F);
        _tower.transform.rotation = Quaternion.LookRotation(newDir);

        Shoot();
    }

    public bool TestAngle(float maxangle, Transform target, Transform self)
    {
        return  Vector3.Angle(target.position - self.position, _slot.transform.forward) < maxangle;
    }

    public void Shoot()
    {
        if (!isCanShoot) return;
        if (!_slot._isCanUse) return;

        _shootTimeBufer = _reloadTime;
        GameObject bullet = Instantiate(_bullet, _firepoint.position, Quaternion.identity);

        bullet.GetComponent<Bullet>().CreateBullet(_slot.GetSlotShip(), _damage, _target, _MaxDist);
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(_firepoint.forward * _bulletSpeed, ForceMode.Impulse);
       

        isCanShoot = false;
    }

}
