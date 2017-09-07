using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DockUI : MonoBehaviour {


    [SerializeField]
    private Text _goldText;
    [SerializeField]
    private Transform _shipsinUI;

    private void OnEnable()
    {
        StartCoroutine(UpdateUI());
        StartCoroutine(LoadShipsFromDB());
    }

    private IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(0.1f);

        _goldText.text = "Gold: " + PlayerDB.Instance().GetPlayerGold();

        StartCoroutine(UpdateUI());
    }

    public IEnumerator LoadShipsFromDB()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("ShipDataLoad");
        foreach(PlayerDB.ShipData data in PlayerDB.Instance().GetShips())
        {
            GameObject panel = Instantiate(Resources.Load("ShipEquip/" + data.ID) as GameObject, _shipsinUI);
            panel.transform.localScale = Vector3.one;

            ShipDecorator decorator = panel.GetComponent<ShipDecorator>();

            foreach(ShipDecorator.ShipSlot slot in data.Slots)
            {
                decorator.AddItemToSlot(slot.SlotId, slot.IteminslotId);
                decorator.CreateItemInSlot(decorator.GetSlotFromId(slot.SlotId), slot.IteminslotId);
            }
            
        }
    }
}
