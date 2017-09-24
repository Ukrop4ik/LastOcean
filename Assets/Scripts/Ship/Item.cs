using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int Mass = 0;
    [SerializeField]
    private string _itemId;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private int _count;
    private Image _image;
    [SerializeField]
    private Transform _startparent;
    public SlotType Type;
    public SpecialType SpecialType;

    public int GetCount()
    {
        return _count;
    }
    public void SetCount(int value)
    {
        _count = value;
    }
    public string GetId()
    {
        return _itemId;
    }
    public Sprite GetSprite()
    {
        return _sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        _startparent = transform.parent;

        if(_startparent.name == "Slot")
        {
            transform.SetParent(transform.root);
            RemoveFromSlot(Mass, _startparent.GetComponent<SlotDecorator>());
        }
        if(_startparent.name == "Inventory")
        {
            transform.SetParent(transform.root);
        }


        TakeFromStack(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;


        if (eventData.pointerEnter == null)
        {
            transform.SetParent(_startparent);

            if (_startparent.name == "Slot")
                 SetToSlot( _itemId, Mass, _startparent.GetComponent<SlotDecorator>());

            if (_startparent.name == "Inventory")
                AddToStack(this, _startparent.GetComponent<Inventory>());
            return;
        }
        if(eventData.pointerEnter.tag != "DropField" && eventData.pointerEnter.name != "Inventory")
        {
            transform.SetParent(_startparent);

            if (_startparent.name == "Slot")
                SetToSlot( _itemId, Mass, _startparent.GetComponent<SlotDecorator>());

            if (_startparent.name == "Inventory")
                AddToStack(this, _startparent.GetComponent<Inventory>());
            return;
        }

        if (eventData.pointerEnter.tag == "DropField")
        {
            if(eventData.pointerEnter.name == "Inventory")
            {
                AddToStack(this, eventData.pointerEnter.GetComponent<Inventory>());
                transform.SetParent(eventData.pointerEnter.transform);
            }
            if (eventData.pointerEnter.name == "Slot")
            {
                if (eventData.pointerEnter.GetComponent<SlotDecorator>().Type == Type && eventData.pointerEnter.GetComponent<SlotDecorator>().SpecialType.Contains(SpecialType))
                {
                    SetToSlot(_itemId, Mass, eventData.pointerEnter.GetComponent<SlotDecorator>());
                    transform.SetParent(eventData.pointerEnter.transform);
                }
                else
                {
                    transform.SetParent(_startparent);

                    if (_startparent.name == "Slot")
                        SetToSlot( _itemId, Mass, _startparent.GetComponent<SlotDecorator>());

                    if (_startparent.name == "Inventory")
                        AddToStack(this, _startparent.GetComponent<Inventory>());
                    return;
                }
            }
            
        }
        else
        {
            if(_startparent.gameObject.name == "Inventory")
            {
                AddToStack(this, _startparent.GetComponent<Inventory>());
            }
            else
                transform.SetParent(_startparent);
        }
    }

    public void SetToSlot(string id, int mass, SlotDecorator decorator = null, ShipDecorator shipDecorator = null)
    {
        if(decorator)
            decorator.SetToSlot(id, mass);

        switch(SpecialType)
        {
            case SpecialType.Shild:
                if(!shipDecorator)
                     decorator.GetShipDecorator()._stats.SetArmorValue(decorator.GetShipDecorator()._stats.GetArmorValue(true) + 500, true);
                else
                     shipDecorator._stats.SetArmorValue(shipDecorator._stats.GetArmorValue(true) + 500, true);
                break;
            default:
                break;
        }
    }
    public void RemoveFromSlot(int mass, SlotDecorator decorator = null, ShipDecorator shipDecorator = null)
    {
        if (decorator)
            decorator.RemoveFromSlot(mass);
        switch (SpecialType)
        {
            case SpecialType.Shild:
                if (!shipDecorator)
                    decorator.GetShipDecorator()._stats.SetArmorValue(decorator.GetShipDecorator()._stats.GetArmorValue(true) - 500, true);
                else
                    shipDecorator._stats.SetArmorValue(shipDecorator._stats.GetArmorValue(true) - 500, true);
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = _sprite;
    }

    public void TakeFromStack(Item item)
    {
        if (_count == 1) return;
        GameObject g = Instantiate(Resources.Load("Items/" + item._itemId) as GameObject);
        g.name = item._itemId;
        Item i = g.GetComponent<Item>();
        i.SetCount(_count - 1);
        _count = 1;
        g.transform.SetParent(_startparent);
        g.transform.localScale = Vector3.one;

        if(_startparent.name == "Inventory")
        {
            Inventory inv = _startparent.GetComponent<Inventory>();
        }

    }
    public void AddToStack(Item item, Inventory inv)
    {
        if (_count < 1) return;
        Debug.Log(item._itemId + "  " + inv.name);
        if(inv.IsInventoryContainsItem(_itemId))
        {
            inv.SetItemToStack(_itemId, _count, gameObject);
        }
    }
}
