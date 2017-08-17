using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    private ShipManager _shipManager;
    [SerializeField]
    private ShipMain _playerShip;
    [SerializeField]
    private Scrollbar _engineScrollbar;

	private void Start ()
    {

        _shipManager = GameObject.FindGameObjectWithTag("Context").GetComponent<ShipManager>();
        StartCoroutine(StartSetup());
       _engineScrollbar.onValueChanged.AddListener(delegate { EngineValueChange(); });
    }

    private void EngineValueChange()
    {
        _playerShip.GetMoveController().SetEnginPower(_engineScrollbar.value);
    }

    private void Update()
    {
        _engineScrollbar.value += Input.GetAxis("Vertical") * Time.deltaTime;
    }

    private IEnumerator StartSetup()
    {
        yield return new WaitForSeconds(0.1f);

        if (_playerShip == null)
        {
            Debug.Log("Wait.....");
            _playerShip = _shipManager.GetPlayerShip();
            StartCoroutine(StartSetup());
        }
        else
        {
            StopCoroutine(StartSetup());
        }

    }

    private void OnDestroy()
    {
        _engineScrollbar.onValueChanged.RemoveListener(delegate { EngineValueChange(); });
    }

}
