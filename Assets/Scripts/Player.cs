using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private static Player instance;
    public static Player Instance() { return instance; }


    [SerializeField]
    private GameObject _ship;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public GameObject GetShip()
    {
        return _ship;
    }
    public void SetShip(GameObject ship)
    {
        _ship = ship;
    }



}
