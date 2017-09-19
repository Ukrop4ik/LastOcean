using Combu;
using LitJson;
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
    List<PlayerDB.Tasks> Tasks = new List<PlayerDB.Tasks>();
    [SerializeField]
    private List<PlayerDB.Resources> _resources = new List<PlayerDB.Resources>();
    [SerializeField]
    private List<PlayerDB.ShipData> _ships = new List<PlayerDB.ShipData>();

    [SerializeField]
    InputField inputName;
    [SerializeField]
    InputField inputPass;
    [SerializeField]
    Button inputButton;

    [SerializeField]
    private GameObject createuserpanel;
    [SerializeField]
    private GameObject joineduserpanel;
    [SerializeField]
    private Text joineduserText;


    private bool isReady = false;

    private void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/" + "Profile" + ".prf") && !string.IsNullOrEmpty(PlayerPrefs.GetString("NickName")))
        {
            //SceneManager.LoadScene("InitScene");
        }


        StartCoroutine(Init());
    }

    [ContextMenu("Login")]
    public void Login(string name, string pass)
    {
        CombuManager.platform.Authenticate<UserData>(name, pass, (bool success, string error) => {
            if (success)
            {
                UnityEngine.Debug.Log("Login success: ID " + CombuManager.localUser.id);
                PlayerPrefs.SetString("UserId", CombuManager.localUser.id);
                PlayerPrefs.SetString("Name", name);
                PlayerPrefs.SetString("Pass", pass);
                PlayerPrefs.Save();
                SceneManager.LoadScene("InitScene");
            }
            else
                UnityEngine.Debug.Log("Login failed: " + error);
        });
    }

    [ContextMenu("Create")]
    public void CreateNetUser()
    {
           

        UserData newUser = new UserData();
        newUser.userName = inputName.text;
        newUser.password = inputPass.text;

        List<PlayerDB.ShipData> _shipsd = new List<PlayerDB.ShipData>();
        List<ShipDecorator.ShipSlot> _slots = new List<ShipDecorator.ShipSlot>();
        _slots.Add(new ShipDecorator.ShipSlot(_ships[0].Slots[0].SlotId, _ships[0].Slots[0].IteminslotId));
        _slots.Add(new ShipDecorator.ShipSlot(_ships[0].Slots[1].SlotId, _ships[0].Slots[1].IteminslotId));
        _shipsd.Add(new PlayerDB.ShipData(_ships[0].ID, _slots));
        newUser.Ships = JsonMapper.ToJson(_shipsd);


        newUser.Tasks = "[" + "]";

        List<PlayerDB.ItemData> _itemsd = new List<PlayerDB.ItemData>();
        _itemsd.Add(new PlayerDB.ItemData(ItemData[0].ItemId, ItemData[0].ItemCount));
        newUser.InventoryItems = JsonMapper.ToJson(_itemsd);

       

        newUser.Update((bool success, string error) =>
        {
            // NB: registration does not make the user logged
            if (success)
            {
                UnityEngine.Debug.Log("Save success: ID " + newUser.id);


                CombuManager.platform.Authenticate<UserData>(inputName.text, inputPass.text, (bool _success, string _error) => {
                    if (_success)
                    {
                        UnityEngine.Debug.Log("Login success: ID " + CombuManager.localUser.id);
                        PlayerPrefs.SetString("UserId", CombuManager.localUser.id);
                        PlayerPrefs.SetString("Name", inputName.text);
                        PlayerPrefs.SetString("Pass", inputPass.text);
                        PlayerPrefs.Save();

                        // Add a new item
                        Combu.Inventory gold = new Combu.Inventory();
                        gold.name = "gold";
                        gold.quantity = 1000;
                        gold.Update((bool __success, string __error) => {
                            if (success)
                                UnityEngine.Debug.Log("Success");
                            else
                                UnityEngine.Debug.Log("Failed: " + error);
                        });
                        // Add a new item
                        Combu.Inventory metal = new Combu.Inventory();
                        metal.name = "metal";
                        metal.quantity = 1000;
                        metal.Update((bool __success, string __error) => {
                            if (success)
                                UnityEngine.Debug.Log("Success");
                            else
                                UnityEngine.Debug.Log("Failed: " + error);
                        });
                        // Add a new item
                        Combu.Inventory fuel = new Combu.Inventory();
                        fuel.name = "fuel";
                        fuel.quantity = 100;
                        fuel.Update((bool __success, string __error) => {
                            if (success)
                                UnityEngine.Debug.Log("Success");
                            else
                                UnityEngine.Debug.Log("Failed: " + error);
                        });
                        // Add a new item
                        Combu.Inventory gems = new Combu.Inventory();
                        gems.name = "gems";
                        gems.quantity = 100;
                        gems.Update((bool __success, string __error) => {
                            if (success)
                                UnityEngine.Debug.Log("Success");
                            else
                                UnityEngine.Debug.Log("Failed: " + error);
                        });

                        // AddStartItems();

                        StartCoroutine(Init());

                    }
                    else
                        UnityEngine.Debug.Log("Login failed: " + _error);
                });

            }
            else
            {
                UnityEngine.Debug.Log("Save failed: " + error);
                return;
            }
        });


    }

    private void AddStartItems()
    {
        // Add a new item
        Combu.Inventory newItem1 = new Combu.Inventory();
        newItem1.name = "Gold";
        newItem1.quantity = 1000;

        newItem1.Update((bool _success, string _error) =>
        {
            if (_success)
                UnityEngine.Debug.Log("Success");
            else
                UnityEngine.Debug.Log("Failed: " + _error);
        });

        // Add a new item
        Combu.Inventory newItem2 = new Combu.Inventory();
        newItem2.name = "Iron";
        newItem2.quantity = 1000;

        newItem2.Update((bool _success, string _error) =>
        {
            if (_success)
                UnityEngine.Debug.Log("Success");
            else
                UnityEngine.Debug.Log("Failed: " + _error);
        });

        // Add a new item
        Combu.Inventory newItem3 = new Combu.Inventory();
        newItem3.name = "Fuel";
        newItem3.quantity = 100;

        newItem3.Update((bool _success, string _error) =>
        {
            if (_success)
                UnityEngine.Debug.Log("Success");
            else
                UnityEngine.Debug.Log("Failed: " + _error);
        });

        // Add a new item
        Combu.Inventory newItem4 = new Combu.Inventory();
        newItem4.name = "Gems";
        newItem4.quantity = 100;

        newItem4.Update((bool _success, string _error) =>
        {
            if (_success)
                UnityEngine.Debug.Log("Success");
            else
                UnityEngine.Debug.Log("Failed: " + _error);
        });
    }

    private void Update()
    {
        if(inputName.text == "" || inputName.text.Length < 3 || inputPass.text.Length < 3 || !CombuManager.isInitialized)
        {
            inputButton.interactable = false;
        }
        else
            inputButton.interactable = true;
    }

    public void ManualLogin()
    {
        Login(PlayerPrefs.GetString("Name"), PlayerPrefs.GetString("Pass"));
    }
    public void Logout()
    {
        createuserpanel.SetActive(true);
        joineduserpanel.SetActive(false);

        PlayerPrefs.SetString("Name", "");
        PlayerPrefs.SetString("Pass", "");
        PlayerPrefs.SetString("UserId", "");
        PlayerPrefs.Save();
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);



        if(CombuManager.isInitialized)
        {
            if (PlayerPrefs.GetString("Name") != "")
            {

                joineduserText.text = PlayerPrefs.GetString("Name");
                createuserpanel.SetActive(false);
                joineduserpanel.SetActive(true);

            }

            GameDB.Instance().Dounload();

            StopCoroutine(Init());
            isReady = true;
        }
        else
        {
            StartCoroutine(Init());
        }
    }

    public void CreateNewPlayer()
    {
        SaveData(ItemData, inputName.text, Tasks);
    }

    public void SaveData(List<PlayerDB.ItemData> items, string NickName, List<PlayerDB.Tasks> tasks)
    {
        Data = new PlayerDB.PlayerData(NickName, items, tasks, _resources, _ships);
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
