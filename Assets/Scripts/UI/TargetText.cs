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

    public void CreateText(string _text, Transform target)
    {

        text.text = _text;
        Target = target;
        _active = true;
    }

    void Update()
    {
        if (!_active) return;
        if (!Target)
            Destroy(this.gameObject);

        this.transform.position = Camera.main.WorldToScreenPoint(Target.position);

    }
}
