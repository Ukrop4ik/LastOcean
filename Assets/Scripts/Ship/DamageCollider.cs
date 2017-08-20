using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour {

    [SerializeField]
    private ShipMain _ship;

    private void Start()
    {
        _ship = transform.root.GetComponent<ShipMain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet _bullet;

        if (other.gameObject.tag == "Bullet")
        {
            _bullet = other.gameObject.GetComponent<Bullet>();

            if (_bullet._ShootShip == null) return;

            if (_ship != _bullet._ShootShip)
            {
                Debug.Log("HIT!" + transform.root.gameObject.name);
            }

           
        }
    }
}
