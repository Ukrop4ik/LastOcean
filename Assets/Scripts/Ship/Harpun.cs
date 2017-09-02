using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpun : Special
{

    [SerializeField]
    private Transform FirePoint;

    [SerializeField]
    private float MaxDist;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private LineRenderer line;
    private GameObject harpunBullet;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Use();
        }

        line.SetPosition(0, FirePoint.transform.position);

        if(harpunBullet)
            line.SetPosition(1, harpunBullet.transform.position);
        else
            line.SetPosition(1, FirePoint.transform.position);
    }

    public override void Use()
    {
        harpunBullet = Instantiate(_bullet, FirePoint.position, FirePoint.rotation);
        harpunBullet.gameObject.GetComponent<Rigidbody>().AddForce(FirePoint.forward * 2f, ForceMode.Impulse);
    }


}
