using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private Text _netStatusText;
    [SerializeField]
    private List<GameObject> HUDs = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _netStatusText = GameObject.Find("NetworkStatusText").GetComponent<Text>();
        StartCoroutine(UpdateUI());
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

    public void SetShipPropToPlayer()
    {
        ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();

        foreach (KeyValuePair<int, string> KVP in Player.Instance().GetShipDecorator().GetWeaponData())
        {
            prop.Add(("slot_" + KVP.Key), KVP.Value);
        }
        PhotonNetwork.player.SetCustomProperties(prop);
        Player.Instance().SetPlayerShipprop(prop);


    }

    private IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(0.1f);

        _netStatusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString() + " Ping: " + PhotonNetwork.GetPing();

        StartCoroutine(UpdateUI());
    }


}
