using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour {

    [SerializeField]
    private ShipMain _ship;
    [SerializeField]
    private PhotonView view;

    private void Start()
    {
        _ship = transform.root.GetComponent<ShipMain>();
        view = _ship.gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!view.isMine) return;

        Bullet _bullet;

        if (other.gameObject.tag == "Bullet")
        {
            _bullet = other.gameObject.GetComponent<Bullet>();

            if (_bullet._ShootShip == null) return;

            if (_ship != _bullet._ShootShip)
            {
                Debug.Log("HIT!" + transform.root.gameObject.name);
                _ship.SetDamage(_bullet._damage);
                _bullet.SetTime(0.5f);
                _bullet.SetDamage(0f);
                _bullet.gameObject.tag = "Trash";
                _ship.Lastdamageship = _bullet._ShootShip;
            }

           
        }
    }
}
