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
    [SerializeField]
    PhotonView _photonView;
    public JsonData jsonData;
    [SerializeField]
    private PlayerShipData _shipData;
    [SerializeField]
    private List<string> gun = new List<string>();
    public string data_str = "";
    public string playerName = "";
    public bool OpponentReady = false;
    public ShipMain Lastdamageship;
    [SerializeField]
    private int goldToHead;
    private void Start()
    {
        Stats = gameObject.GetComponent<ShipStat>();

        if (_onlineType == ShipOnlineType.Player & photonView.isMine)
        {
            Stats.SetHullValue(PlayerDB.Instance()._currentShipDecorator._stats.GetHullValue(true), true);
            Stats.SetArmorValue(PlayerDB.Instance()._currentShipDecorator._stats.GetArmorValue(true), true);
            Stats.SetAcceleration(PlayerDB.Instance()._currentShipDecorator._stats.GetAcceleration());
            Stats.SetAngularSpeed(PlayerDB.Instance()._currentShipDecorator._stats.GetAngularSpeed());
            Stats.SetSpeed(PlayerDB.Instance()._currentShipDecorator._stats.GetSpeed());
            Stats.SetBackSpeed(PlayerDB.Instance()._currentShipDecorator._stats.GetReversSpeed());
        }

        _movecontroller = gameObject.GetComponent<MoveController>();
        _movecontroller.SetParameters(this, GetRigidbody(), Stats.GetAngularSpeed());
        _photonView = gameObject.GetComponent<PhotonView>();
        if (_onlineType != ShipOnlineType.Bot)
            _shipData = new PlayerShipData(_shipId, new Dictionary<int, string>());

        if (photonView.isMine && _onlineType != ShipOnlineType.Bot)
        {
            if(!PhotonNetwork.offlineMode)
                 this.photonView.RPC("CreateFromServer", PhotonTargets.OthersBuffered, PhotonNetwork.player.NickName, PhotonNetwork.player.CustomProperties);


            MissionManager.Instance().SetPlayer(PhotonNetwork.player.ID, PhotonNetwork.player.NickName);
        }
        else
        {
            if(_onlineType != ShipOnlineType.Bot)
            {
                _onlineType = ShipOnlineType.Opponent;             
            }
                              
        }
        _shipManager = GameObject.FindGameObjectWithTag("Context").GetComponent<ShipManager>();

        if(_onlineType == ShipOnlineType.Bot)
        {
            MissionManager.Instance().AddShip(this);
            ShipManager.AddShip(this);
        }
        else
        {
            ShipManager.AddShip(this);
        }

    } 


    [PunRPC]
    public void CreateFromServer(string player, ExitGames.Client.Photon.Hashtable shipdata)
    {

        playerName = player;

        foreach(DictionaryEntry dict in shipdata)
        {
            if (string.IsNullOrEmpty((string)dict.Value)) continue;

            string value = dict.Key.ToString();
            Debug.Log("Value : " + value);
            if(value.Contains("slot"))
            {
                int slotid = 0;
                Int32.TryParse(value.Split('_')[1], out slotid);
                Debug.Log("Slot id: " + slotid);

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

        shipReady = true;
    }
    public void CreateFromServer(ExitGames.Client.Photon.Hashtable shipdata)
    {

        foreach (DictionaryEntry dict in shipdata)
        {
            if (string.IsNullOrEmpty((string)dict.Value)) continue;

            string value = dict.Key.ToString();

            if (value.Contains("slot"))
            {
                int slotid = 0;
                Int32.TryParse(value.Split('_')[1], out slotid);
                foreach (Slot slot in _slots)
                {
                    if (slot.SlotId == slotid)
                    {
                        GameObject weapon = Instantiate(Resources.Load((string)dict.Value) as GameObject);
                        weapon.transform.SetParent(slot.transform);
                        weapon.transform.localPosition = Vector3.zero;
                        weapon.transform.rotation = slot.transform.rotation;
                    }
                }
            }
        }

        shipReady = true;
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
    public Transform GetTarget()
    {
        return _target;
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

    public List<Slot> GetSlots()
    {
        return _slots;
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

        if (Stats.GetArmorValue() > 0)
        {
            if (Stats.GetArmorValue() - value < 0)
            {
                Stats.SetHullValue(Stats.GetHullValue() - (value - Stats.GetArmorValue()));
                Stats.SetArmorValue(0);
            }
            else
            {
                Stats.SetArmorValue(Stats.GetArmorValue()-value);
            }
        }
        else
        {
            Stats.SetHullValue(Stats.GetHullValue() - value);
        }
    

        if (Stats.GetHullValue() <= 0)
        {
            if (!PhotonNetwork.offlineMode)
                _photonView.RPC("Dead", PhotonTargets.All);
            else
                Dead();
        }
    }
    [PunRPC]
    private void Dead()
    {

        List<GameObject> points = NetworkManager.Instance().GetPoints();

        transform.position = points[UnityEngine.Random.Range(0, points.Count)].transform.position;

        Stats.RestoreStat();

        if (photonView.isMine)
        {
            if (Lastdamageship.GetOnlineType() == ShipOnlineType.Player)
            {
                MissionManager.Instance().AddGoldReward(goldToHead);
            }
        }

        if (_onlineType == ShipOnlineType.Bot)
        {
            MissionManager.Instance().RemoveShip(this);
            Destroy(gameObject);
        }
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


