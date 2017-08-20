using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float _destroyTime = 2f;
    public float _damage { get; private set; }
    public ShipMain _ShootShip { get; private set; }

    private void Update()
    {
        _destroyTime -= Time.deltaTime;


        if(_destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void CreateBullet(float time, ShipMain ship)
    {
        _destroyTime = time;
        _ShootShip = ship;
    }
}
