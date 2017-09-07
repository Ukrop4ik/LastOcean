using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private int Gold = 0;
    public string NickName;

    private static Player instance;
    public static Player Instance() { return instance; }

    public PlayerDB Data { get; private set; }
    [SerializeField]
    private GameObject _ship;
    [SerializeField]
    private ShipDecorator ship_decorator;
    private ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();

    public ExitGames.Client.Photon.Hashtable GetPlayerShipProp()
    {
        return prop;
    }
    public void SetPlayerShipprop(ExitGames.Client.Photon.Hashtable _prop)
    {
        prop = _prop;
    }

    public int GetPlayerGold()
    {
        return Gold;
    }

    public void SetPlayerGold(int value)
    {
        Gold += value;
    }

    private void Awake()
    {
        instance = this;
        NickName = PlayerPrefs.GetString("NickName");
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
        Data = GetComponent<PlayerDB>();
    }

    public ShipDecorator GetShipDecorator()
    {
        return ship_decorator;
    }
    public void SetShipDecorator(ShipDecorator decorator)
    {
        ship_decorator = decorator;
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
