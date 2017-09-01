using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMain : Photon.MonoBehaviour {

    private ShipManager _shipManager;
    [SerializeField]
    private ShipOnlineType _onlineType;
    private MoveController _movecontroller;
    [SerializeField]
    private List<Slot> _slots = new List<Slot>();
    private ShipStat Stats;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private string _shipId;
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
    private double _LastNetworkDataTime = 0f;
    public bool shipReady = false;
    PhotonView _photonView;
    public JsonData jsonData;
    [SerializeField]
    private PlayerShipData _shipData;
    [SerializeField]
    private List<string> gun = new List<string>();
    public string data_str = "";
    public string playerName = "";
    public bool OpponentReady = false;

    private void Start()
    {
        Stats = gameObject.GetComponent<ShipStat>();
        _photonView = gameObject.GetComponent<PhotonView>();
        _shipData = new PlayerShipData(_shipId, new Dictionary<int, string>());

        if (photonView.isMine)
        {
            this.photonView.RPC("CreateFromServer", PhotonTargets.OthersBuffered, photonView.name, PhotonNetwork.player.CustomProperties);
            _movecontroller = gameObject.GetComponent<MoveController>();
            _movecontroller.SetParameters(this, GetRigidbody(), Stats.GetAngularSpeed());
        }
        else
        {
            _onlineType = ShipOnlineType.Opponent;                
        }
        _shipManager = GameObject.FindGameObjectWithTag("Context").GetComponent<ShipManager>();
        _shipManager.AddShip(this);
    } 

    [PunRPC]
    public void CreateFromServer(string player, ExitGames.Client.Photon.Hashtable shipdata)
    {
        // the photonView.RPC() call is the same as without the info parameter.
        // the info.sender is the player who called the RPC.
        Debug.Log(string.Format("Info: {0} {1} ", player, shipdata));

        foreach(DictionaryEntry dict in shipdata)
        {
            if (string.IsNullOrEmpty((string)dict.Value)) continue;

            string value = dict.Key.ToString();

            if(value.Contains("slot"))
            {
                char[] delimetr = new char[] {'s','l','o','t','_'};
                string[] substrings = value.Split(delimetr);
                string Sslot = "";
                foreach (String s in substrings)
                {
                    Sslot += s;
                }

                int slotid = 0;
                Int32.TryParse(Sslot, out slotid);

                foreach (Slot slot in _slots)
                {
                    if(slot.SlotId == slotid)
                    {
                        Debug.Log((string)dict.Value);
                        GameObject weapon = Instantiate(Resources.Load((string)dict.Value) as GameObject);
                        weapon.transform.SetParent(slot.transform);
                        weapon.transform.localPosition = Vector3.zero;
                        weapon.transform.rotation = slot.transform.rotation;
                    }
                }
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        foreach (Slot slot in _slots)
        {
            if (slot.GetWeapon() != null)
                slot.GetWeapon().SetTarget(target);
        }
    }
    public MoveController GetMoveController()
    {
        return _movecontroller;
    }
    public ShipOnlineType GetOnlineType()
    {
        return _onlineType;
    }
    public void SetOnlineType(ShipOnlineType type)
    {
        _onlineType = type;
    }
    public ShipStat GetStats()
    {
        return Stats;
    }
    public Rigidbody GetRigidbody()
    {
        return gameObject.GetComponent<Rigidbody>();
    }
    public List<Weapon> GetWeaponOnSide(ShipSide side)
    {
        List<Weapon> weaponsinside = new List<Weapon>();

        foreach(Slot slot in _slots)
        {

            if (slot.GetWeapon() != null && slot.GetSlotSide() == side)
            {
                weaponsinside.Add(slot.GetWeapon());
            }
        }

        return weaponsinside;
    }
    public void SetDamage(float value)
    {
        Stats.SetHullValue(Stats.GetHullValue() - value);
        if (Stats.GetHullValue() <= 0)
            Dead();
    }
    private void Dead()
    {
        _shipManager.RemoveShip(this);



        Destroy(gameObject);
    }
    



    }



    [System.Serializable]
    public class PlayerShipData
    {
        public string ShipId;
        public Dictionary<int, string> WeaponsInSlots = new Dictionary<int, string>();

        public PlayerShipData(string _shipId, Dictionary<int, string> _weaponId)
        {
            ShipId = _shipId;
            WeaponsInSlots = _weaponId;
        }
    }


