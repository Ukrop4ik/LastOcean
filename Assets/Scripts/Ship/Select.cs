using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour {

    private ShipMain ship;

    private void Start()
    {
        ship = gameObject.transform.root.GetComponent<ShipMain>();
    }
    public ShipMain GetSelectedShip()
    {
        return ship;
    }
}
