using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Photon.MonoBehaviour {

    public enum WinCondition
    {
        BotKill
    }

    public WinCondition winCondition;

    [SerializeField]
    private int goldReward;

    [SerializeField]
    private int _missionTime;
    

    [SerializeField]
    private List<ShipMain> _bots = new List<ShipMain>();

    [SerializeField]
    private List<PlayerStats> _players = new List<PlayerStats>();

    private static MissionManager instance;
    private HUD _HUD;

    public static MissionManager Instance() { return instance; }

    private void Awake()
    {
        instance = this;
       _HUD = GameObject.Find("HUD").GetComponent<HUD>();
        
    }

    private void Start()
    {
        StartCoroutine(Check());
        StartCoroutine(Timer());
    }


    public void SetPlayer(int id, string name)
    {
        photonView.RPC("Set", PhotonTargets.AllBufferedViaServer, id, name);
    }
    [PunRPC]
    public void Set(int id, string name)
    {
        PlayerStats stats = new PlayerStats(0, name, id);
        _players.Add(stats);
    }

    public void AddShip(ShipMain ship)
    {
        if (!_bots.Contains(ship))
            _bots.Add(ship);
    }

    public void RemoveShip(ShipMain ship)
    {
        if (_bots.Contains(ship))
            _bots.Remove(ship);
    }

    public List<ShipMain> GetBots()
    {
        return _bots;
    }

    private IEnumerator Check()
    {
        yield return new WaitForSeconds(1f);

        if(winCondition == WinCondition.BotKill)
        {
            if (_bots.Count == 0)
            {
                photonView.RPC("Win", PhotonTargets.AllBuffered);
                StopCoroutine(Check());
            }
        }


        StartCoroutine(Check());
    }

    [PunRPC]
    private void Win()
    {
        SceneController.Instance().LoadScene("MainMenu");
        Player.Instance().SetPlayerGold(Player.Instance().GetPlayerGold() + goldReward);
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);

        _missionTime--;
        _HUD.SetMissionTimer(_missionTime);
        if(_missionTime > 0)
           StartCoroutine(Timer());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.Serialize(ref _missionTime);
    }

    [System.Serializable]
    private class PlayerStats
    {
        public int Gold;
        public string NickName;
        public int Id;

        public PlayerStats(int gold, string nickname, int id)
        {
            Gold = gold;
            NickName = nickname;
            Id = id;
        }
    }

}
