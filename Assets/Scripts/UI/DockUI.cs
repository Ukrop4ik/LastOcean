using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DockUI : MonoBehaviour {


    [SerializeField]
    private Text _goldText;

    private void OnEnable()
    {
        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(0.1f);

        _goldText.text = "Gold: " + Player.Instance().GetPlayerGold();

        StartCoroutine(UpdateUI());
    }
}
