using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : Photon.MonoBehaviour {

    [SerializeField]
    private ShipMain _ship;
    [SerializeField]
    private PhotonView view;
    float value;

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


        Bullet _bullet;

        if (other.gameObject.tag == "Bullet")
        {
            _bullet = other.gameObject.GetComponent<Bullet>();

            if (_bullet._ShootShip == null) return;

            if (_ship != _bullet._ShootShip)
            {
                _bullet.DamageFX();

                if (view.isMine)
                {
                    value = Random.Range(_bullet._damageMin, _bullet._damageMax);
                    _ship.SetDamage(value);
                    _bullet.SetTime(0.5f);
                    _bullet.SetDamage(0f, 0f);
                    _bullet.gameObject.tag = "Trash";
                    _ship.Lastdamageship = _bullet._ShootShip;
                  
                }

                  _ship.ShowDamage(value);
            }

           
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.Serialize(ref value);
    }
}
