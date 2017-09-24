using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : Photon.MonoBehaviour {

    [SerializeField]
    private List<WeaponHUDIndicator> LSireWeaponIndicator = new List<WeaponHUDIndicator>();
    [SerializeField]
    private List<WeaponHUDIndicator> RSireWeaponIndicator = new List<WeaponHUDIndicator>();
    [SerializeField]
    private Transform _compassCenter;
    [SerializeField]
    private GameObject _navigationArrow;
    private int _navigationArrowCount = 0;
    [SerializeField]
    private GameObject _targetText;
    [SerializeField]
    private GameObject _pointer;
    private Button lFireButton;
    [SerializeField]
    private Button RFireButton;
    [SerializeField]
    private List<Sprite> _reverssticksprite = new List<Sprite>();
    [SerializeField]
    private List<Sprite> _reversLedSprits = new List<Sprite>();
    [SerializeField]
    private Image _reversstickimage;
    [SerializeField]
    private Image _reversLed;

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
    [Range(0.01f, 1f)]
    private float _updateUiRate;
    [SerializeField]
    private Image _hpslider;
    [SerializeField]
    private Image _armorslider;
    [SerializeField]
    private GameObject _enemyShipStatusPanel;
    [SerializeField]
    private Image _hpsliderEnemy;
    [SerializeField]
    private Image _armorslider_enemy;
    [SerializeField]
    private Text _missionTimer;
    [SerializeField]
    private Text consoletext;
    private ShipManager _shipManager;
    [SerializeField]
    private ShipMain _playerShip;
    [SerializeField]
    private Scrollbar _engineScrollbar;
    private MoveController _moveController;
    private float _shipCountBuffer;

    private ShipMain _selectedShip;

    private bool _isRevers = false;

    private ShipStat _shipStat;

    public WeaponsOnSids weaponsOnSides = new WeaponsOnSids();

    [SerializeField]
    private GameObject _hpValue;


    public LayerMask layerMask;

    public bool Lfire = false;
    public bool Rfire = false;

    private void ClearAll()
    {
        weaponsOnSides.back_weapons.Clear();
        weaponsOnSides.front_weapons.Clear();
        weaponsOnSides.left_weapons.Clear();
        weaponsOnSides.rith_weapons.Clear();
        _selectedShip = null;
        _shipStat = null;
        consolestring = "";

    }

    private void Start()
    {

        foreach (WeaponHUDIndicator indWeapon in LSireWeaponIndicator)
        {
            indWeapon.obj.SetActive(false);
        }

        foreach (WeaponHUDIndicator indWeapon in RSireWeaponIndicator)
        {
            indWeapon.obj.SetActive(false);
        }
    }

    private void OnEnable ()
    {
        StartCoroutine(StartSetup());
       _engineScrollbar.onValueChanged.AddListener(delegate { EngineValueChange(); });

    }

    private void OnDisable()
    {
        _engineScrollbar.onValueChanged.RemoveListener(delegate { EngineValueChange(); });
        ClearAll();
    }

    private void EngineValueChange()
    {
        _playerShip.GetMoveController().SetEnginPower(_engineScrollbar.value);

        if(_isRevers)
        {
            Revers();
        }
    }

    public void SetMissionTimer(int value)
    {
        int _timer = value;
        string minuts = "0";
        string seconds = "0";

        if (_timer <= 0) return;

        if (_timer / 60 > 0)
            minuts = (_timer / 60).ToString("00");

        seconds = (_timer % 60).ToString("00");

        _missionTimer.text = minuts + " : " + seconds;
    }

    public void CreateTargetText(ShipMain _ship, Transform target)
    {
        GameObject tt = Instantiate(_targetText, Vector3.zero, Quaternion.identity) as GameObject;
        _ship.SetTargetbar(tt);
        tt.transform.SetParent(this.transform);
        tt.transform.SetSiblingIndex(0);
        tt.transform.localScale = Vector3.one;
        tt.GetComponent<TargetText>().CreateText(_ship);

    }


    public void CreateNavigationArrow(Transform target)
    {
        StartCoroutine(Arrow(target));
    }

    private IEnumerator Arrow(Transform target)
    {
        yield return new WaitForSeconds(0.1f);

        if (_playerShip)
        {
            NavigationArrow scr;
            GameObject arrow = Instantiate(_navigationArrow, _compassCenter.position, Quaternion.identity);
            scr = arrow.GetComponent<NavigationArrow>();
            scr.player = ShipManager.GetPlayerShip().gameObject.transform;
            scr.target = target;
            arrow.transform.SetParent(this.transform);
            arrow.transform.localScale = Vector3.one;
        }
        else
        {
            StartCoroutine(Arrow(target));
        }

    }

    public void Fire()
    {

    }
    public void FireL()
    {

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
                _selectedShip = hit.collider.gameObject.GetComponent<Select>().GetSelectedShip();
            }
        }

        if(_selectedShip)
        {
            //_pointer.SetActive(true);
            //_pointer.transform.position = Camera.main.WorldToScreenPoint(_selectedShip.transform.position);
            //_pointer.GetComponent<RectTransform>().Rotate(0, 0, _pointer.transform.rotation.z + Time.deltaTime);
        }
        else
        {
            //_pointer.SetActive(false);
        }

        _engineScrollbar.value += Input.GetAxis("Vertical") * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.R))
        {
            Revers();
        }

        if (Input.GetKey(KeyCode.Q) || Lfire)
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

        if (Input.GetKey(KeyCode.E) || Rfire)
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

    public void Revers()
    {
        _isRevers = !_isRevers;
        _playerShip.GetMoveController().SetEnginPower(0);
        _engineScrollbar.value = 0;
        _playerShip.GetMoveController().Revers(_isRevers);
        _reversstickimage.sprite = _isRevers ? _reverssticksprite[1] : _reverssticksprite[0];
        _reversLed.sprite = _isRevers ? _reversLedSprits[1] : _reversLedSprits[0];
    }

    private IEnumerator UpdateShipStatus()
    {
        yield return new WaitForSeconds(_updateUiRate);

        if(_playerShip && _shipStat)
        {
            _hpslider.fillAmount = _shipStat.GetHullValue() / _shipStat.GetHullValue(true);
            _armorslider.fillAmount = _shipStat.GetArmorValue() / _shipStat.GetArmorValue(true);

            if (LSireWeaponIndicator.Count > 0)
                foreach (WeaponHUDIndicator weponInd in LSireWeaponIndicator)
                {
                    if (!weponInd.weapon) continue;
                    if(weponInd.weapon._ammocount != weponInd.bulletisactive)
                        weponInd.EnableBullet(weponInd.weapon._ammocount);

                    weponInd.Feel();
                }


            if (RSireWeaponIndicator.Count > 0)
                foreach (WeaponHUDIndicator weponInd in RSireWeaponIndicator)
                {
                    if (!weponInd.weapon) continue;
                    if (weponInd.weapon._ammocount != weponInd.bulletisactive)
                        weponInd.EnableBullet(weponInd.weapon._ammocount);

                    weponInd.Feel();
                }
        }

        if(_playerShip.GetTarget() && _selectedShip)
        {
            _enemyShipStatusPanel.SetActive(true);
            _hpsliderEnemy.fillAmount = _selectedShip.GetStats().GetHullValue() / _selectedShip.GetStats().GetHullValue(true);
            _armorslider_enemy.fillAmount = _selectedShip.GetStats().GetArmorValue() / _selectedShip.GetStats().GetArmorValue(true);
        }
        else
        {
            _enemyShipStatusPanel.SetActive(false);
        }

        StartCoroutine(UpdateShipStatus());

    }
    public void ShowDamage(float value, Transform target)
    {
        GameObject o = Instantiate(_hpValue, transform);
        o.GetComponent<DamageValueText>().SetValue(value, target);
    }
    private IEnumerator StartSetup()
    {
        yield return new WaitForSeconds(0.1f);

        if (_playerShip == null)
        {
            consolestring = "Wait..... \n";
            if (!_shipManager)
            {
                _shipManager = GameObject.FindGameObjectWithTag("Context").GetComponent<ShipManager>();
            }
            else
            {
                _playerShip = ShipManager.GetPlayerShip();
            }
            StartCoroutine(StartSetup());
        }
        else
        {
            _moveController = _playerShip.GetMoveController();
            consolestring += "Player Ship Found \n";

            _shipStat = _playerShip.gameObject.GetComponent<ShipStat>();

            consolestring += "Controller Found \n";

            GameObject.FindGameObjectWithTag("Camera").GetComponent<BattleCamera>().Settings(_playerShip.transform);

            consolestring += "Camera Ready \n";

            int weaponcount = 0;
            if (_playerShip.shipReady)
            {
                if (LSireWeaponIndicator.Count > 0)
                    foreach (WeaponHUDIndicator weponInd in LSireWeaponIndicator)
                    {
                        weponInd.AddBulletToList();
                    }
            

                if (RSireWeaponIndicator.Count > 0)
                    foreach (WeaponHUDIndicator weponInd in RSireWeaponIndicator)
                    {
                        weponInd.AddBulletToList();
                    }

                int weaponnum = 0;

                foreach (Weapon w in _playerShip.GetWeaponOnSide(ShipSide.Left))
                {
                    if (w)
                    {
                        LSireWeaponIndicator[weaponnum].obj.SetActive(true);
                        LSireWeaponIndicator[weaponnum].weapon = w;
                        LSireWeaponIndicator[weaponnum].reloadtime = w._reloadspeed;
                        LSireWeaponIndicator[weaponnum].EnableBullet(w._ammocountMax);
                        weaponnum++;

                        weaponsOnSides.left_weapons.Add(w);
                        weaponcount++;
                       
                    }

                }
                weaponnum = 0;
                foreach (Weapon w in _playerShip.GetWeaponOnSide(ShipSide.Rith))
                {
                    if (w)
                    {
                        RSireWeaponIndicator[weaponnum].obj.SetActive(true);
                        RSireWeaponIndicator[weaponnum].weapon = w;
                        RSireWeaponIndicator[weaponnum].reloadtime = w._reloadspeed;
                        RSireWeaponIndicator[weaponnum].EnableBullet(w._ammocountMax);
                        weaponnum++;

                        weaponsOnSides.rith_weapons.Add(w);
                        weaponcount++;
                    }

                }
                weaponnum = 0;
                foreach (Weapon w in _playerShip.GetWeaponOnSide(ShipSide.Forward))
                {
                    if (w)
                    {
                        weaponsOnSides.front_weapons.Add(w);
                        weaponcount++;
                    }

                }
                weaponnum = 0;
                foreach (Weapon w in _playerShip.GetWeaponOnSide(ShipSide.Back))
                {
                    if (w)
                    {
                        weaponsOnSides.back_weapons.Add(w);
                        weaponcount++;
                    }

                }
                consolestring += weaponcount + " " + "Weapons Ready";

                StartCoroutine(UpdateShipStatus());
                StopCoroutine(StartSetup());
            }
        }

    }

    [System.Serializable]
    private class WeaponHUDIndicator
    {
        public GameObject obj;
        public Image weaponimage;
        public GameObject bullet_field;
        public List<GameObject> bulletlist = new List<GameObject>();
        public Weapon weapon;
        public int bulletisactive;
        public Image _reloadProggressBar;
        public bool isReload = false;
        public float reloadtime;
        private float reloadbuffer;

        public WeaponHUDIndicator(GameObject o, Image im, GameObject bul)
        {
            obj = o;
            weaponimage = im;
            bullet_field = bul;
            

        }

        public void AddBulletToList()
        {
            for (int i = 0; i < bullet_field.transform.childCount; i++)
            {
                bulletlist.Add(bullet_field.transform.GetChild(i).gameObject);
            }
                
        }
        public void EnableBullet(int count)
        {

            foreach (GameObject bullet in bulletlist)
            {
                bullet.SetActive(false);
                bulletisactive = 0;
            }

            if (count <= bulletlist.Count)
                for (int i = 0; i < count; i++)
                {
                    bulletisactive++;
                    bullet_field.transform.GetChild(i).gameObject.SetActive(true);
                }
        

        }

        public void Feel()
        {

            if (weapon._isReloading)
            {
                Debug.Log("Reloading");
                if(weapon._reloadspeedCurr > 0)
                    _reloadProggressBar.fillAmount = weapon._reloadspeedCurr / weapon._reloadspeed;
            }
            else
            {
                _reloadProggressBar.fillAmount = 0;
            }

        }
    }

}
