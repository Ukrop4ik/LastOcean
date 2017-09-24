using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBody : MonoBehaviour {

    ShipMain _ship;

	// Use this for initialization
	void Start () {
        _ship = transform.root.gameObject.GetComponent<ShipMain>();
	}

    private void OnBecameVisible()
    {
        _ship.TargetbarEnable(true);
    }
    private void OnBecameInvisible()
    {
        _ship.TargetbarEnable(false);
    }
}
