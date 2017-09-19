using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetText : MonoBehaviour {

    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image HP;
    [SerializeField]
    private Image ARMOR;
    [SerializeField]
    private bool _active = false;
    ShipMain _ship;

    public void CreateText(ShipMain ship)
    {
        _ship = ship;
        text.text = ship.playerName;
        Target = ship.transform;
        _active = true;
    }

    void LateUpdate()
    {
        if (!_active) return;

        if (!Target)
            Destroy(this.gameObject);
        else
        {
            this.transform.position = Camera.main.WorldToScreenPoint(Target.position);
            HP.fillAmount = _ship.GetStats().GetHullValue() / _ship.GetStats().GetHullValue(true);
            ARMOR.fillAmount = _ship.GetStats().GetArmorValue() / _ship.GetStats().GetArmorValue(true);
        }
            
    }
}
