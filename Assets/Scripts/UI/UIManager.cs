using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private Text _netStatusText;
    [SerializeField]
    private List<GameObject> HUDs = new List<GameObject>();
    [SerializeField]
    private List<TimeLineVisual> timlinevisuallist = new List<TimeLineVisual>();

    private static UIManager instance;
    public static UIManager Instance() { return instance; }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public List<TimeLineVisual> GetTimlineVisual()
    {
        return timlinevisuallist;
    }
    public void AddVisualTask(PlayerDB.Tasks task)
    {
        foreach(TimeLineVisual vis in timlinevisuallist)
        {
            if(vis.Task == null)
            {
                vis.Task = task;
                return;
            }
        }
    }

    private void Start()
    {
        _netStatusText = GameObject.Find("NetworkStatusText").GetComponent<Text>();
        SceneController.Instance().LoadedScene = "MainMenu";
        SceneManager.LoadScene("MainMenu");
        HUDs[0].SetActive(true);
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

        foreach(TimeLineVisual vis in timlinevisuallist)
        {
            vis.Check();
        }
        StartCoroutine(UpdateUI());
    }
    [System.Serializable]
    public class TimeLineVisual
    {
        public int Time;
        public GameObject Obj;
        public Image Image;
        public Text Text;
        public int TaskListId;
        public PlayerDB.Tasks Task = null;

        public TimeLineVisual(int time, GameObject obj, Image image, Text text, int listid)
        {
            Time = time;
            Obj = obj;
            Image = image;
            Text = text;
            TaskListId = listid;
        }

        public void Check()
        {
            if (Task == null) return;

            if(Task.TimeEnd > (int)NetworkData.Instance().GetServerTimeInt())
            {
               // Obj.SetActive(true);
                Text.text = (Task.TimeEnd - ((int)NetworkData.Instance().GetServerTimeInt())).ToString();
            }
            else
            {
                Clear();
               // Obj.SetActive(false);
            }

        }

        public void Clear()
        {
            Time = 0;
            Task = null;
        }
    }


}
