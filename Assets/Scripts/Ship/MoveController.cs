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
    [Range(0.001f, 1.0f)]
    public float _enginePower;
    [Range(0.001f, 1.0f)]
    private float _enginePowerBuffer;
    private float _rotateBuffer;
  

    public void SetParameters(ShipMain ship, Rigidbody rBody, float drag)
    {
        _ship = ship;
        _rgBody = rBody;
        _rgBody.angularDrag = drag / 2f;
    }

    private void FixedUpdate()
    {
        if (!photonView.isMine) return;
        if (_ship.GetOnlineType() != ShipOnlineType.Player) return;

        _enginePowerBuffer = Mathf.Lerp(_enginePowerBuffer, _enginePower, Time.deltaTime);

        if (_enginePower > 0)
        {
            MoveForward();
        }

        RotationKeyboard();

        if (_ship.transform.rotation.x != 0 || _ship.transform.rotation.z != 0)
        {
            //_ship.transform.Rotate(0, _ship.transform.rotation.y, 0);
        }
    }

    private void MoveForward()
    {
        _speed = Mathf.Clamp(_speed, 0.0f, _ship.GetStats().GetSpeed() * _enginePowerBuffer);

        _speed = Mathf.Lerp(_speed, _ship.GetStats().GetSpeed() * _enginePowerBuffer, Time.deltaTime * _ship.GetStats().GetAcceleration());   

        _rgBody.AddForce(transform.forward * _speed);
    }

    private void RotationKeyboard()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal") * _ship.GetStats().GetAngularSpeed() * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            h = Input.GetAxis("Horizontal") * _ship.GetStats().GetAngularSpeed() * Time.deltaTime;
        }

        transform.Rotate(0,h,0);
    }

    public void SetEnginPower(float power)
    {
        _enginePower = power;
    }
}
