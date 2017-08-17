using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMain : MonoBehaviour {

    [SerializeField]
    [Range(0f,10f)]
    private float _maximumSpeed;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _angularspeed;
    private ShipManager _shipManager;
    [SerializeField]
    private ShipOnlineType _onlineType;
    private MoveController _movecontroller;
    [SerializeField]
    private List<Slot> _slots = new List<Slot>();

    private void Start()
    {
        _shipManager = GameObject.FindGameObjectWithTag("Context").GetComponent<ShipManager>();
        _shipManager.AddShip(this);
        _movecontroller = gameObject.GetComponent<MoveController>();
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

    public Rigidbody GetRigidbody()
    {
        return gameObject.GetComponent<Rigidbody>();
    }
    public float GetSpeed()
    {
        return _maximumSpeed;
    }
    public float GetAcceleration()
    {
        return _acceleration;
    }
    public float GetAngularSpeed()
    {
        return _angularspeed;
    }
}
