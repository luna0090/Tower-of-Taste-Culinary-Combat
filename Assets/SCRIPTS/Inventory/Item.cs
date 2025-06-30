using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ItemType { MeleeWeapon, RangedWeapon, Spell, Dish, Heal }
public enum ItemRarity { Common, Uncommon, Rare, Epic, Legendary, Exotic }

[System.Serializable]
public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemName;
    public ItemRarity rarity;
    public ItemType type;
    public Sprite icon;
    public int powerLevel;
    public int uniqueID;

    private List<RectTransform> potentialSlots;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Transform originalSlot;

    public static implicit operator Item(string v)
    {
        throw new NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag Start");
        canvasGroup.blocksRaycasts = false;
        originalSlot = transform.parent;
        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;

        potentialSlots.Clear();

        foreach (ItemSlot slot in InventoryManager.Instance.slots)
        {
            if (GetWorldRect(GetComponent<RectTransform>()).Overlaps(GetWorldRect(slot.GetComponent<RectTransform>())))
            {
                potentialSlots.Add(slot.GetComponent<RectTransform>());
            }
        }

        //Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag Stop");
        canvasGroup.blocksRaycasts = true;
        /*List<float> slotDistances = new List<float>();

        foreach (RectTransform slot in potentialSlots)
        {
            slotDistances.Add(Vector3.Distance(slot.position, transform.position));
        }

        foreach (RectTransform slot in potentialSlots)
        {
            if (Vector3.Distance(slot.position, transform.position) == slotDistances.Min())
            {
                transform.SetParent(slot, false);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }*/

        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalSlot);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    void Start()
    {
        potentialSlots = new List<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        
    }
    
    Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        return new Rect(bottomLeft, topRight - bottomLeft);
    }
}
