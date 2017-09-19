using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DockUI : MonoBehaviour {


    [SerializeField]
    private Text _goldText;
    [SerializeField]
    private Text _metalText;
    [SerializeField]
    private Text _fuelText;
    [SerializeField]
    private Text _gemsText;
    [SerializeField]
    private Transform _shipsinUI;

    [SerializeField]
    private List<GameObject> _panels = new List<GameObject>();

    private GameObject _lastPanel;

    public void ActivatePanel(GameObject panel)
    {


        foreach (GameObject obj in _panels)
        {
            if(panel != obj)
                obj.SetActive(false);
        }
        
        if(_lastPanel == panel && panel.activeInHierarchy)
            CameraMover(0);
        else
        {
            if (panel.name == "Ship")
                CameraMover(1);
            if (panel.name == "MissionPanel")
                CameraMover(4);
 
        }
        
    
        panel.SetActive(!panel.activeInHierarchy);
        _lastPanel = panel;
    }

    public void CameraMover(int indx)
    {
        TransformPoints point = Camera.main.gameObject.GetComponent<TransformPoints>();
        point.MoveToPoint(indx);
    }

    private void Start()
    {
        SceneController.Instance().LoadedScene = "InitScene";
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateUI());
        StartCoroutine(LoadShipsFromDB());

    }

    private IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(0.05f);

        _goldText.text =  PlayerDB.Instance().GetPlayerGold().ToString();
        _metalText.text =  PlayerDB.Instance().GetPlayerMetal().ToString();
        _fuelText.text =  PlayerDB.Instance().GetPlayerFuel().ToString();
        _gemsText.text =  PlayerDB.Instance().GetPlayerGems().ToString();

        if (PlayerDB.Instance().GetTasks().Count > 0)
        {
            for (int i = 0; i < PlayerDB.Instance().GetTasks().Count; i++)
            {
                UIManager.Instance().GetTimlineVisual()[i].Text.text = StaticMetods.ConvertTimeToString((PlayerDB.Instance().GetTasks()[i].TimeEnd - NetworkData.Instance().GetServerTimeInt()));

                if (PlayerDB.Instance().GetTasks()[i].IsMomental == 0 && IsTaskEnd(PlayerDB.Instance().GetTasks()[i]))
                {
                    UIManager.Instance().GetTimlineVisual()[i].Image.color = Color.green;
                    UIManager.Instance().GetTimlineVisual()[i].TaskListId = i;
                }
                else
                {
                    UIManager.Instance().GetTimlineVisual()[i].Image.color = Color.red;
                }
            }
            for (int i = PlayerDB.Instance().GetTasks().Count; i < UIManager.Instance().GetTimlineVisual().Count; i++)
            {
                UIManager.Instance().GetTimlineVisual()[i].Text.text = "";
                UIManager.Instance().GetTimlineVisual()[i].Image.color = Color.yellow;
            }
        }

        StartCoroutine(UpdateUI());
    }

    private bool IsTaskEnd(PlayerDB.Tasks tasks)
    {
        return tasks.TimeEnd <= NetworkData.Instance().GetServerTimeInt();
    }

    public IEnumerator LoadShipsFromDB()
    {
        yield return new WaitForSeconds(1f);

        List<ShipDecorator> _decorators = new List<ShipDecorator>();

        Debug.Log("ShipDataLoad");
        foreach(PlayerDB.ShipData data in PlayerDB.Instance().GetShips())
        {
            GameObject panel = Instantiate(Resources.Load("ShipEquip/" + data.ID) as GameObject, _shipsinUI);
            panel.transform.localScale = Vector3.one;

            ShipDecorator decorator = panel.GetComponent<ShipDecorator>();

            foreach(ShipDecorator.ShipSlot slot in data.Slots)
            {
                if (slot.IteminslotId != "")
                {
                    Item i = decorator.CreateItemInSlot(decorator.GetSlotFromId(slot.SlotId), slot.IteminslotId);
                    decorator.AddItemToSlot(slot.SlotId, slot.IteminslotId, i.Mass);
                }
            }

            _decorators.Add(decorator);
            
        }

        PlayerDB.Instance()._currentShipDecorator = _decorators[0];
    }
}
