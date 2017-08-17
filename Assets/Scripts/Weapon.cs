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
    private GameObject _bullet;
    [SerializeField]
    private float _bulletSpeed;
    [SerializeField]
    private Transform _firepoint;

    private float _shootTimeBufer;

    private void Start()
    {
        _slot = transform.GetComponentInParent<Slot>();
        _MinMaxAngle_Y = _slot.GetWeaponAngleMinMax();
        _angleY = transform.localRotation.y;
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
            Shoot();
        }

        if (!_target) return;
        _angleTest = Vector3.Angle(_target.position - transform.position, _slot.transform.forward);

        Debug.Log(_angleTest);

        if (_angleTest > _MinMaxAngle_Y) return;

        Vector3 targetdirection = _target.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdirection, ((_angularSpeed * 0.1f) * Time.deltaTime), 0.0F);

        transform.rotation = Quaternion.LookRotation(newDir);

    }

    public void RotateWeapon(RotationAxis axis, float angle)
    {
        float rotationAngle = angle;

        transform.localRotation = Quaternion.Euler(0, rotationAngle, 0);
    }

    public void Shoot()
    {
        _shootTimeBufer = _reloadTime;

       GameObject bullet = Instantiate(_bullet, _firepoint.position, _firepoint.rotation);
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(_firepoint.forward * _bulletSpeed, ForceMode.Impulse);
    }
}
