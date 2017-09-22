using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour {

    [SerializeField]
    private Transform _ship;
    private Transform _self;
    [SerializeField]
    private Transform _camera;

    [SerializeField]
    private Vector2 offsets;
    [SerializeField]
    private Transform _cameraRotator;

    [SerializeField]
    private Vector2 xBorder;
    [SerializeField]
    private Vector2 yBorder;

    public float sensitivity = 1F;
    private Vector3 MousePos;
    private float MyAngle = 0F;
    [SerializeField]
    private bool Invers;

    public void CameraRotation(float rotation)
    {

    }


    public void Settings(Transform ship)
    {
        _ship = ship;
        _self = transform;
        _camera.position = new Vector3(_camera.position.x + offsets.x, _camera.position.y, _camera.position.z + offsets.y);
    }

    private void Update()
    {
        if (!_self || !_ship) return;

        if (Input.GetMouseButton(0) && Input.mousePosition.y > (Screen.height / yBorder.x) &&  Input.mousePosition.x > (Screen.width / xBorder.x) && Input.mousePosition.x < (Screen.width * xBorder.y))
        {
            MyAngle = 0;
            MyAngle = sensitivity * ((Input.mousePosition.x - (Screen.width / 2f)) / Screen.width);

            MyAngle = Invers ? -MyAngle : MyAngle;

            _cameraRotator.transform.Rotate(_cameraRotator.transform.up, MyAngle);
        }

        _self.position = new Vector3(_ship.position.x, _self.position.y, _ship.position.z);    

        _self.rotation = _ship.rotation;
    }

}
