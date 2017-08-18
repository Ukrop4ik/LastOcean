using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MoveController : MonoBehaviour {

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
    private void Start()
    {
        _ship = gameObject.GetComponent<ShipMain>();
        _rgBody = _ship.GetRigidbody();
        _rgBody.angularDrag = _ship.GetAngularSpeed() / 2f;
    }

    private void FixedUpdate()
    {

        if (_ship.GetOnlineType() != ShipOnlineType.Player) return;

        _enginePowerBuffer = Mathf.Lerp(_enginePowerBuffer, _enginePower, Time.deltaTime);

        if (_enginePower > 0)
        {
            MoveForward();
        }

        RotationKeyboard();
    }

    private void MoveForward()
    {
        _speed = Mathf.Clamp(_speed, 0.0f, _ship.GetSpeed() * _enginePowerBuffer);

        _speed = Mathf.Lerp(_speed, _ship.GetSpeed() * _enginePowerBuffer, Time.deltaTime * _ship.GetAcceleration());   

        _rgBody.AddForce(transform.forward * _speed);
    }

    private void RotationKeyboard()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal") * _ship.GetAngularSpeed() * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            h = Input.GetAxis("Horizontal") * _ship.GetAngularSpeed() * Time.deltaTime;
        }

        transform.Rotate(transform.up, h);
    }
    public void RotationFromButtons(int id)
    {
       
        if (id == 0)
             _rotateBuffer += _ship.GetAngularSpeed() * Time.deltaTime;
        else
             _rotateBuffer -= _ship.GetAngularSpeed() * Time.deltaTime;

        transform.Rotate(transform.up, _rotateBuffer);
    }

    public void SetEnginPower(float power)
    {
        _enginePower = power;
    }
}
