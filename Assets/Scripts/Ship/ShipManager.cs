using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {

    [SerializeField]
    private List<ShipMain> _ships = new List<ShipMain>();

    public void AddShip(ShipMain ship)
    {
        if (!_ships.Contains(ship))
            _ships.Add(ship);
    }
    public void RemoveShip(ShipMain ship)
    {
        if (_ships.Contains(ship))
            _ships.Remove(ship);
    }
    public ShipMain GetPlayerShip()
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
