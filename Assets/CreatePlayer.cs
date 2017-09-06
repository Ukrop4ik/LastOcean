﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreatePlayer : MonoBehaviour {

    PlayerDB.PlayerData Data;
    [SerializeField]
    List<PlayerDB.ItemData> ItemData = new List<PlayerDB.ItemData>();
    [SerializeField]
    InputField inputName;
    [SerializeField]
    Button inputButton;

    private void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/" + "Profile" + ".prf") && !string.IsNullOrEmpty(PlayerPrefs.GetString("NickName")))
        {
            SceneManager.LoadScene(1);
        }
    }

    private void Update()
    {
        if(inputName.text == "" || inputName.text.Length < 3)
        {
            inputButton.interactable = false;
        }
        else
            inputButton.interactable = true;
    }


    public void CreateNewPlayer()
    {
        SaveData(ItemData, inputName.text);
    }

    public void SaveData(List<PlayerDB.ItemData> items, string NickName)
    {
        Data = new PlayerDB.PlayerData(NickName, items);
        File.WriteAllText(Application.persistentDataPath + "/" + Data.PlayerName + ".prf", JsonMapper.ToJson(Data));
        File.WriteAllText(Application.persistentDataPath + "/" + "Profile" + ".prf", "NewPlayerProfileData: " + Data.PlayerName);
        PlayerPrefs.SetString("NickName", NickName);
        SceneManager.LoadScene(1);
    }

    [ContextMenu("OpenProfileFolder")]
    public void OpenProfileFolder()
    {
        Process.Start(Application.persistentDataPath);
    }
}
