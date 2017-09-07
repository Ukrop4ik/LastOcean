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
    private bool _active = false;

    public void CreateText(ShipMain ship)
    {

        text.text = ship.playerName;
        Target = ship.transform;
        _active = true;

        if (ship.GetOnlineType() == ShipOnlineType.Bot)
            text.text = "BOT";
    }

    void LateUpdate()
    {
        if (!_active) return;

        if (!Target)
            Destroy(this.gameObject);
        else
            this.transform.position = Camera.main.WorldToScreenPoint(Target.position);
            
    }
}
