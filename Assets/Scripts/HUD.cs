using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [System.Serializable]
    public struct WeaponsOnSids
    {
        public List<Weapon> left_weapons;
        public List<Weapon> rith_weapons;
        public List<Weapon> front_weapons;
        public List<Weapon> back_weapons;

        public WeaponsOnSids(List<Weapon> weapons_on_left, List<Weapon> weapons_on_rith, List<Weapon> weapons_on_front, List<Weapon> weapons_on_back)
        {
            left_weapons = weapons_on_left;
            rith_weapons = weapons_on_rith;
            front_weapons = weapons_on_front;
            back_weapons = weapons_on_back;
        }
    }
    private string consolestring = "";

    [SerializeField]
    private Text consoletext;
    private ShipManager _shipManager;
    [SerializeField]
    private ShipMain _playerShip;
    [SerializeField]
    private Scrollbar _engineScrollbar;
    private MoveController _moveController;

    public WeaponsOnSids weaponsOnSides = new WeaponsOnSids();


    public LayerMask layerMask;


    private void Start()
    {

    }

    private void OnEnable ()
    {
        StartCoroutine(StartSetup());
       _engineScrollbar.onValueChanged.AddListener(delegate { EngineValueChange(); });
    }

    private void OnDisable()
    {
        _engineScrollbar.onValueChanged.RemoveListener(delegate { EngineValueChange(); });
    }

    private void EngineValueChange()
    {
        _playerShip.GetMoveController().SetEnginPower(_engineScrollbar.value);
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                _playerShip.SetTarget(hit.collider.gameObject.GetComponent<Select>().GetSelectedShip().transform);
            }
        }

        _engineScrollbar.value += Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
        {
            if (weaponsOnSides.left_weapons.Count > 0)
            foreach(Weapon we in weaponsOnSides.left_weapons)
            {
              we.GetMySlot().SetUse(true);
            }
        }
        else if (_playerShip)
        {
            if (weaponsOnSides.left_weapons.Count > 0)
            foreach (Weapon we in weaponsOnSides.left_weapons)
            {
                we.GetMySlot().SetUse(false);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (weaponsOnSides.rith_weapons.Count > 0)
                foreach (Weapon we in weaponsOnSides.rith_weapons)
                {
                    we.GetMySlot().SetUse(true);
                }
        }
        else if (_playerShip)
        {
            if (weaponsOnSides.rith_weapons.Count > 0)
            foreach (Weapon we in weaponsOnSides.rith_weapons)
            {
                we.GetMySlot().SetUse(false);
            }
        }

        consoletext.text = consolestring;
    }

    private IEnumerator StartSetup()
    {
        yield return new WaitForSeconds(0.1f);

        if (_playerShip == null)
        {
            consolestring += "Wait..... \n";
            if (!_shipManager)
            {
                _shipManager = GameObject.FindGameObjectWithTag("Context").GetComponent<ShipManager>();
            }
            else
            {
                _playerShip = _shipManager.GetPlayerShip();
            }
            StartCoroutine(StartSetup());
        }
        else
        {
            _moveController = _playerShip.GetMoveController();
            consolestring += "Player Ship Found \n";
            consolestring += "Controller Found \n";

            GameObject.FindGameObjectWithTag("Camera").GetComponent<BattleCamera>().Settings(_playerShip.transform);

            consolestring += "Camera Ready \n";

            int weaponcount = 0;

            foreach (Weapon w in _playerShip.GetWeaponOnSide(ShipSide.Left))
            {
                if (w)
                {
                    weaponsOnSides.left_weapons.Add(w);
                    weaponcount++;
                }
                    
            }
            foreach (Weapon w in _playerShip.GetWeaponOnSide(ShipSide.Rith))
            {
                if (w)
                {
                    weaponsOnSides.rith_weapons.Add(w);
                    weaponcount++;
                }
                    
            }
            foreach (Weapon w in _playerShip.GetWeaponOnSide(ShipSide.Forward))
            {
                if (w)
                {
                    weaponsOnSides.front_weapons.Add(w);
                    weaponcount++;
                }
                    
            }
            foreach (Weapon w in _playerShip.GetWeaponOnSide(ShipSide.Back))
            {
                if (w)
                {
                    weaponsOnSides.back_weapons.Add(w);
                    weaponcount++;
                }

            }
            consolestring += weaponcount + " " + "Weapons Ready";

            StopCoroutine(StartSetup());
        }

    }

  

}
