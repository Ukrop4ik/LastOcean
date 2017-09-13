using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;
using System;

public class Chat : Photon.MonoBehaviour, IChatClientListener {

    private ChatClient m_ChatClient;

    private void Awake()
    {
        m_ChatClient = new ChatClient(this);
    }

	// Use this for initialization
	void Start () {
		
	}

    private void Update()
    {
        if(m_ChatClient != null)
        {
            return;
        }

        m_ChatClient.Service();
    }

    public void Connect()
    {
        m_ChatClient.Connect(PhotonNetwork.PhotonServerSettings.ChatAppID, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(PlayerDB.Instance().NickName));
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new NotImplementedException();
    }

    public void OnConnected()
    {
        m_ChatClient.Subscribe(new string[] { "Global" });
        m_ChatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new NotImplementedException();
    }
}
