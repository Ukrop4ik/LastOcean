using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private static Player instance;
    public static Player Instance() { return instance; }


    [SerializeField]
    private GameObject _ship;
    [SerializeField]
    private ShipDecorator ship_decorator;

    private void Awake()
    {
        instance = this;
    }

    public ShipDecorator GetShipDecorator()
    {
        return ship_decorator;
    }
    public void SetShipDecorator(ShipDecorator decorator)
    {
        ship_decorator = decorator;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public GameObject GetShip()
    {
        return _ship;
    }
    public void SetShip(GameObject ship)
    {
        _ship = ship;
    }



}
