using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour {

    public string SceneId;
    private UIManager UI;
    private SceneController _controller;

    private void Start()
    {
        UI = GameObject.Find("UI").GetComponent<UIManager>();
        _controller = UI.gameObject.GetComponent<SceneController>();
    }

    public void LoadMission()
    {
        UI.SetShipPropToPlayer();
        _controller.LoadScene(SceneId);
    }
}
