using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {

    [SerializeField]
    private static List<ShipMain> _ships = new List<ShipMain>();

    public static int  _shipcount = 0;

    private static HUD _HUD;

    private void Awake()
    {
        _HUD = GameObject.Find("HUD").GetComponent<HUD>();
    }

    private void Start()
    {
       
    }

    public static List<ShipMain> GetOtherShips()
    {
        List<ShipMain> ships = new List<ShipMain>();

        foreach(ShipMain ship in _ships)
        {
            if(ship.GetOnlineType() != ShipOnlineType.Player)
            ships.Add(ship);
        }

        return ships;
    }

    public static void AddShip(ShipMain ship)
    {
        if (!_ships.Contains(ship))
        {
            if(ship.GetOnlineType() != ShipOnlineType.Player)
            {
                _HUD.CreateNavigationArrow(ship.transform);
            }
            _ships.Add(ship);
            _shipcount++;
        }
           
    }
    public static void RemoveShip(ShipMain ship)
    {
        if (_ships.Contains(ship))
            _ships.Remove(ship);
    }
    public static ShipMain GetPlayerShip()
    {
        ShipMain _ship = null;

        foreach (ShipMain ship in _ships)
        {
            if (ship.GetOnlineType() == ShipOnlineType.Player)
            {
                _ship = ship;
            }

        }
        return _ship;
    }
}
