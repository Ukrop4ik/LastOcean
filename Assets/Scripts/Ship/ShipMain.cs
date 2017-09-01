using LitJson;
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

    public bool OpponentReady = false;

    private void Start()
    {
        Stats = gameObject.GetComponent<ShipStat>();
        _photonView = gameObject.GetComponent<PhotonView>();
        _shipData = new PlayerShipData(_shipId, new Dictionary<int, string>());

        //for(int i =0; i < gun.Count; i++)
        //{
        //    _shipData.WeaponsInSlots.Add(i + 1, gun[i]);
        //}

        if (photonView.isMine)
        {


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

    private void Update()
    {
        if(!shipReady && data_str != "")
        {
            CreateShip(data_str);

            if(_onlineType == ShipOnlineType.Opponent)
            {
                OpponentReady = true;
            }
        }
    }

    public void CreateShip(string data)
    {

        jsonData = JsonMapper.ToObject(data);

        if (jsonData.Count > 0)
        {

            for (int i = 0; i < jsonData.Count; i++)
            {
                foreach (Slot slot in _slots)
                {
                    if (slot.SlotId == (int)jsonData[i]["slot"])
                    {
                        if ((string)jsonData[i]["id"] != "")
                        {
                            GameObject weapon = Instantiate(Resources.Load((string)jsonData[i]["id"]) as GameObject);
                            weapon.transform.SetParent(slot.transform);
                            weapon.transform.localPosition = Vector3.zero;
                            weapon.transform.rotation = slot.transform.rotation;
                        }
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



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (!OpponentReady || !shipReady)
        {
            if (stream.isWriting)
            {
                stream.SendNext(jsonData.ToJson());
                Debug.Log("Send");
            }

            else
            {
                data_str = (string)stream.ReceiveNext();
                Debug.Log("Get");
            }
        }
        else
        {
            if (stream.isWriting)
            {
                stream.SendNext("");
                Debug.Log("Send empty");
            }

            else
            {
                Debug.Log((string)stream.ReceiveNext());
            }
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


