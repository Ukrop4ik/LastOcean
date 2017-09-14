using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour {

    public string SceneId;
    [SerializeField]
    private Button _missionButton;
    private UIManager UI;
    private SceneController _controller;

    private void Update()
    {
        if(PlayerDB.Instance()._currentShipDecorator.isReady)
        {
            _missionButton.interactable = true;
        }
        else
        {
            _missionButton.interactable = false;
        }
    }

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
