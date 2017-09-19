using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageValueText : MonoBehaviour {

    [SerializeField]
    private Text text;
    [SerializeField]
    private float lifeTime;
    private float Ypos;
    private bool isRedy;
    [SerializeField]
    Transform _target;
    [SerializeField]
    Transform textTr;
    private void Start()
    {

        Ypos = transform.position.y;
    }
    public void SetValue(float value, Transform target, bool isArmor = false)
    {
        _target = target;
        text.text = "- " + value.ToString("0");
        isRedy = true;
    }


    private void Update()
    {
        if (!_target)
        {
            Destroy(gameObject);
            return;
        }


        if (!isRedy) return;

        lifeTime -= Time.deltaTime;
        transform.position = Camera.main.WorldToScreenPoint(_target.position);

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
