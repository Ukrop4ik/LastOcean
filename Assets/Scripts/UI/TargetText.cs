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
    [SerializeField]
    private GameObject body;
    private bool isVisible;

    public void CreateText(ShipMain ship)
    {
        _ship = ship;
        text.text = ship.playerName;
        Target = ship.transform;
        _active = true;
        StartCoroutine(CheckVisible());
    }

    void LateUpdate()
    {
        if (!_active) return;
        if (!isVisible) return;
        if (!Target) return;

        else
        {
            this.transform.position = Camera.main.WorldToScreenPoint(Target.position);
            HP.fillAmount = _ship.GetStats().GetHullValue() / _ship.GetStats().GetHullValue(true);
            ARMOR.fillAmount = _ship.GetStats().GetArmorValue() / _ship.GetStats().GetArmorValue(true);
        }
            
    }

    private IEnumerator CheckVisible()
    {
        yield return new WaitForSeconds(0.1f);

        if(!Target)
        {
            Destroy(this.gameObject);
            yield return null;
        }

        if (ShipManager.CheckDistToPlayership(Target.position) < 50f)
        {
            isVisible = true;
            body.SetActive(true);
        }
        else
        {
            isVisible = false;
            body.SetActive(false);
        }

        StartCoroutine(CheckVisible());
    }

}
