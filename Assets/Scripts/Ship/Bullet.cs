using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float _destroyTime = 5f;
    public float _damageMin { get; private set; }
    public float _damageMax { get; private set; }
    public ShipMain _ShootShip { get; private set; }
    private Transform _target;
    private float dist;
    private float _maxdist;


    private void Update()
    {
        _destroyTime -= Time.deltaTime;


        if(_destroyTime < 0)
        {
            Destroy(gameObject);
        }

        if(_target)
        {
            dist = Vector3.Distance(transform.position, _target.position);

            if(dist <= 0f)
            {
                _destroyTime = 0.5f;
            }
        }

        
    }

    public void CreateBullet(ShipMain ship, float mindamage, float maxdamage, Transform target, float dist)
    {
        _ShootShip = ship;
        _damageMin = mindamage;
        _damageMax = maxdamage;
        _target = target;
        _maxdist = dist;
    }
    public void SetTime(float time)
    {
        _destroyTime = time;
    }
    public void SetDamage(float min, float max)
    {
        _damageMin = min;
        _damageMax = max;
    }
}
