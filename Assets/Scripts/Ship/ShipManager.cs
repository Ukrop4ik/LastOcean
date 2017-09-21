using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {

    [SerializeField]
    private static List<ShipMain> _ships = new List<ShipMain>();

    public static int  _shipcount = 0;

    private static HUD _HUD;

    private static ShipMain _playership;

    private void Awake()
    {
        _HUD = GameObject.Find("HUD").GetComponent<HUD>();
    }

    private void Start()
    {
        StartCoroutine(CheckVisibleShips());
    }

    public static float CheckDistToPlayership(Vector3 vect)
    {
         if(_playership) { return Vector3.Distance(vect, _playership.transform.position); } else return 1000f; 
    }

    private IEnumerator CheckVisibleShips()
    {
        yield return new WaitForSeconds(0.1f);

        if(_playership)
        foreach(ShipMain ship in _ships)
        {
                if (ship)
                {
                    if (Vector3.Distance(ship.transform.position, _playership.transform.position) < 50f)
                    {
                        ship.Visibility(true);
                    }
                    else
                    {
                        ship.Visibility(false);
                    }
                }
        }

        StartCoroutine(CheckVisibleShips());
    }

    public static List<ShipMain> GetAllShips()
    {
        return _ships;
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
                _HUD.CreateTargetText(ship, ship.transform);
            }
            _ships.Add(ship);
            if (ship.GetOnlineType() == ShipOnlineType.Player)
                _playership = ship;
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
    public static List<ShipMain> GetPlayersShips()
    {
        List<ShipMain> ships = new List<ShipMain>();

        foreach (ShipMain ship in _ships)
        {
            if (ship.GetOnlineType() != ShipOnlineType.Bot)
                ships.Add(ship);
        }

        return ships;
    }
}
