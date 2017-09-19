using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAmeGenerator : MonoBehaviour {

    public List<string> name_1 = new List<string>();
    public List<string> name_2 = new List<string>();


    public string Generate()
    {
        string nickname = name_1[Random.Range(0, name_1.Count - 1)] + " " + name_2[Random.Range(0, name_1.Count - 1)];
        Debug.Log(nickname);
        return nickname;
    }
}
