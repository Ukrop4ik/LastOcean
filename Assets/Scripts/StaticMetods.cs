using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMetods : MonoBehaviour {

	public static string ConvertTimeToString(int time)
    {
        int _timer = time;
        string minuts = "0";
        string seconds = "0";

        if (_timer <= 0) return "";

        if (_timer / 60 > 0)
            minuts = (_timer / 60).ToString("00");

        seconds = (_timer % 60).ToString("00");

        return minuts + " : " + seconds;
    }

    public void ClaerField()
    {

    }
}
