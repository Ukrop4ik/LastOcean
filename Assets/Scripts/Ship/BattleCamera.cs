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

    public float sensitivity = 1F;
    private Vector3 MousePos;
    private float MyAngle = 0F;
    [SerializeField]
    private bool Invers;
    [SerializeField]
    UnityStandardAssets.Cameras.FreeLookCam Fcam;
    [SerializeField]
    HackAndSlashCamera HKScam;

    public void CameraRotation(float rotation)
    {

    }


    public void Settings(Transform ship)
    {
        _ship = ship;
        _self = transform;
        _camera.position = new Vector3(_camera.position.x + offsets.x, _camera.position.y, _camera.position.z + offsets.y);
         Fcam.SetTarget(ship);
      //  Fcam.transform.rotation = ship.rotation;
        //HKScam.CameraSetUp(ship);
    }

    private void Update()
    {
        if (!_self || !_ship) return;

        //_self.position = new Vector3(_ship.position.x, _self.position.y, _ship.position.z);    

        //_self.rotation = _ship.rotation;
    }

}
