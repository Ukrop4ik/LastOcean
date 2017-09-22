using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MoveController : Photon.MonoBehaviour {

    private ShipMain _ship;
    private Rigidbody _rgBody;

    [Range(0.0f, 10f)]
    [SerializeField]
    private float _speed;
    private float _angularSpeed;
    private float _angularSpeedBuffer;
    [Range(0.001f, 1.0f)]
    public float _enginePower;
    [Range(0.001f, 1.0f)]
    private float _enginePowerBuffer;
    private float _rotateBuffer;
    Vector3 newDirection;
    Vector3 oldDirection;
    Vector3 forceDirection;
    bool _isRevers = false;
    float contr;
    public void SetParameters(ShipMain ship, Rigidbody rBody, float drag)
    {
        _ship = ship;
        _rgBody = rBody;
        _rgBody.angularDrag = drag / 2f;
        oldDirection = transform.forward;
        forceDirection = transform.forward;
        _angularSpeed = _ship.GetStats().GetAngularSpeed();
    }

    private void FixedUpdate()
    {
        //if (!_ship) return;
        //if (!photonView.isMine) return;
        //if (_ship.GetOnlineType() != ShipOnlineType.Player) return;

        _enginePowerBuffer = Mathf.Lerp(_enginePowerBuffer, _enginePower, Time.deltaTime);

        if (_enginePower > 0)
        {
            MoveForward();
        }

        if(_isRevers)
        {
            _rgBody.AddForce(-transform.forward * _ship.GetStats().GetReversSpeed());
        }

        if(_ship.GetOnlineType() == ShipOnlineType.Player)
           RotationKeyboard(0);

        forceDirection = Vector3.Lerp(forceDirection, newDirection, Time.deltaTime * 0.5f);
    }

    private void MoveForward()
    {
        _speed = Mathf.Clamp(_speed, 0.0f, _ship.GetStats().GetSpeed() * _enginePowerBuffer);

        _speed = Mathf.Lerp(_speed, _ship.GetStats().GetSpeed() * _enginePowerBuffer, Time.deltaTime * _ship.GetStats().GetAcceleration());   

        _rgBody.AddForce(forceDirection * _speed);
    }

    public void Revers(bool value)
    {
        _isRevers = value;

    }

    public void RotationKeyboard(float rotation)
    {
        if (_ship.GetOnlineType() == ShipOnlineType.Player)
        {
            float android_h = CrossPlatformInputManager.GetAxis("Horizontal") * _angularSpeed;
            float axis_h = Input.GetAxis("Horizontal") * _angularSpeed;
            _angularSpeedBuffer = Mathf.Clamp(_angularSpeedBuffer, 0, _angularSpeed);
            _angularSpeedBuffer = Mathf.Lerp(_angularSpeedBuffer, _angularSpeed, Time.deltaTime);


#if UNITY_ANDROID
            if(android_h != 0)
                 contr = Mathf.Lerp(contr, CrossPlatformInputManager.GetAxis("Horizontal"), Time.deltaTime);

#endif
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
               
                contr = Mathf.Lerp(contr, Input.GetAxis("Horizontal"), Time.deltaTime);

            }

            float _h = android_h != 0 ? android_h : axis_h;

            _h = contr * _angularSpeedBuffer * Time.deltaTime;

            if(axis_h != 0)
                Debug.Log("Buffer axis: " + _angularSpeedBuffer + "  h: " + axis_h);
            if(android_h != 0)
                Debug.Log("Buffer android: " + _angularSpeedBuffer + "  h: " + android_h);

            transform.Rotate(0, _h, 0);
            newDirection = transform.forward;

            if (android_h == 0 && axis_h == 0)
            {
                _angularSpeedBuffer = Mathf.Lerp(_angularSpeedBuffer, 0, Time.deltaTime * 2f);
                contr = Mathf.Lerp(contr, 0, Time.deltaTime);
            }
        }


        if (_ship.GetOnlineType() == ShipOnlineType.Bot)
        {
            float h = rotation * _ship.GetStats().GetAngularSpeed();

            if (rotation != 0)
            {
                contr = Mathf.Lerp(contr, rotation, Time.deltaTime);
                _angularSpeedBuffer = Mathf.Lerp(_angularSpeedBuffer, _angularSpeed, Time.deltaTime);
                _angularSpeedBuffer = Mathf.Clamp(_angularSpeedBuffer, 0, _angularSpeed);
            }
            else
            {
                _angularSpeedBuffer = Mathf.Lerp(_angularSpeedBuffer, 0, Time.deltaTime * 2f);
            }

            h = contr * _angularSpeedBuffer * Time.deltaTime;

            transform.Rotate(0, h, 0);
            newDirection = transform.forward;
        }
    }

    public void SetEnginPower(float power)
    {
        _enginePower = power;
    }
}
