﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {


    [SerializeField]
    private List<SceneDate> scenes = new List<SceneDate>();

    [SerializeField]
    private UIManager UI;


    public void LoadScene(string name)
    {
        foreach(SceneDate date in scenes)
        {
            if (date.SceneName == name)
            {
                SceneManager.LoadScene(name);
                UI.SelectActiveHUD(date.Type);
                return;
            }
        }
    }
    public void LoadScene(int id)
    {
        foreach (SceneDate date in scenes)
        {
            if (date.BuildId == id)
            {
                SceneManager.LoadScene(id);
                UI.SelectActiveHUD(date.Type);
                return;
            }
        }
    }

    [System.Serializable]
    private class SceneDate
    {
        public string SceneName;
        public int BuildId;
        public SceneType Type;
        public SceneDate(string name, int id, SceneType type)
        {
            SceneName = name;
            BuildId = id;
            Type = type;
        }
    }

}
