using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float _destroyTime = 0.5f;

    public void SetTime(float time)
    {
        _destroyTime = time;
    }

    private void Update()
    {
        _destroyTime -= Time.deltaTime;


        if(_destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
