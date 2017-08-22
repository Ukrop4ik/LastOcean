using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour {

    [SerializeField]
    private Transform _ship;
    private Transform _self;

    [SerializeField]
    private Vector2 offsets;

    public void Settings(Transform ship)
    {
        _ship = ship;
        _self = transform;
    }

    private void Update()
    {
        if (!_self || !_ship) return;

        _self.position = new Vector3(_ship.position.x + offsets.x, _self.position.y, _ship.position.z + offsets.y);

        _self.rotation = Quaternion.Euler(new Vector3(0, _ship.rotation.y, 0));
    }
}
