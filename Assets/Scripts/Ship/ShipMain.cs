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

    [SerializeField]
    private PlayerShipData _shipData;
    [SerializeField]
    private List<string> gun = new List<string>();

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


    public void CreateShip(string id, Dictionary<int, string> weapons)
    {
        _shipId = id;

       _shipData = new PlayerShipData(_shipId, weapons);

        if (_shipData.WeaponsInSlots.Count > 0)
        {
            foreach (KeyValuePair<int, string> KVP in weapons)
            {
                if (KVP.Value == "") continue;

                GameObject weapon = Instantiate(Resources.Load(KVP.Value) as GameObject);

                foreach (Slot slot in _slots)
                {
                    if (KVP.Key == slot.SlotId)
                    {
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

        if (stream.isWriting)
        {
            stream.SendNext(_shipId);
            stream.SendNext(_shipData.WeaponsInSlots);

        }
    
        else
        {
            if (!shipReady && !photonView.isMine)
            {
                Debug.Log("CreateShip" + "gam: " + _onlineType);
                Dictionary<int, string> dicct = new Dictionary<int, string>();
                foreach (KeyValuePair<int, string> KVP in (Dictionary<int, string>)stream.ReceiveNext())
                {
                    dicct.Add(KVP.Key, KVP.Value);
                }
                CreateShip((string)stream.ReceiveNext(), dicct);
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
}
