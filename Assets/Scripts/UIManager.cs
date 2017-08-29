using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private List<GameObject> HUDs = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SelectActiveHUD(SceneType type)
    {
        switch(type)
        {
            case SceneType.Main:
                HUDs[0].SetActive(true);
                HUDs[1].SetActive(false);
                break;
            case SceneType.Battle:
                HUDs[1].SetActive(true);
                HUDs[0].SetActive(false);
                break;
            default:
                break;               
        }
    }

    public void SetShip(GameObject ship)
    {
        Player.Instance().SetShip(ship);
    }
}
