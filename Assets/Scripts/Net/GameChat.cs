using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameChat : MonoBehaviour, IChatClientListener {

    private ChatClient m_ChatClient;

    //UI
    [SerializeField]
    private Text _chatText;
    [SerializeField]
    private InputField _chatInput;


    private void Awake()
    {
        m_ChatClient = new ChatClient(this);
    }

    // Use this for initialization
    void Start()
    {
        Connect();
        Debug.Log("Connecting to chat");
    }

    private void OnEnable()
    {

    }

    public void SendMessage()
    {
        m_ChatClient.PublishMessage("Global", _chatInput.text);
        _chatInput.text = "";
    }
    private void Update()
    {
        if (m_ChatClient != null)
        {
            m_ChatClient.Service();
            return;
        }


    }

    public void Connect()
    {
        m_ChatClient.Connect(PhotonNetwork.PhotonServerSettings.ChatAppID, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(PlayerDB.Instance().NickName));
    }

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnDisconnected()
    {


    }

    public void OnConnected()
    {
        m_ChatClient.Subscribe(new string[] { "Global" });
        m_ChatClient.SetOnlineStatus(ChatUserStatus.Online);
        Debug.Log("Connect to chat");
    }

    public void OnChatStateChange(ChatState state)
    {

    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            msgs += "<color=#00ffffff>[" + senders[i] + "]</color>" + " " + messages[i] + "\n";
            Debug.Log(channelName + " " + msgs);
        }
        _chatText.text += msgs;

    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log(channels[0] + " " + results[0]);
    }

    public void OnUnsubscribed(string[] channels)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }
}
