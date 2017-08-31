using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private string _itemId;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private int _count;
    private Image _image;
    [SerializeField]
    private Transform _startparent;

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
        transform.SetParent(transform.root);
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
            return;
        }

        if (eventData.pointerEnter.tag == "DropField")
        {
            if(eventData.pointerEnter.name == "Inventory")
            {
                AddToStack(this, eventData.pointerEnter.GetComponent<Inventory>());
            }
            else
                transform.SetParent(eventData.pointerEnter.transform);
        }
        else
        {
            if(_startparent.gameObject.name == "Inventory")
            {
                AddToStack(this, eventData.pointerEnter.GetComponent<Inventory>());
            }
            else
                transform.SetParent(_startparent);
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
        if (_count <= 1) return;

        Debug.Log(item._itemId + " ");
        GameObject g = Instantiate(Resources.Load("Items/" + item._itemId) as GameObject);
        g.name = item._itemId;
        Item i = g.GetComponent<Item>();
        i.SetCount(_count - 1);
        _count = 1;
        g.transform.SetParent(_startparent);
        g.transform.localScale = Vector3.one;

    }
    public void AddToStack(Item item, Inventory inv)
    {
        if (_count < 1) return;

        if(inv.IsInventoryContainsItem(_itemId))
        {
            inv.SetItemToStack(_itemId, _count);
        }

        Destroy(this.gameObject);
    }
}
