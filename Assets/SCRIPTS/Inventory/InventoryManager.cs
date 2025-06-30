using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<ItemSlot> slots = new List<ItemSlot>();

    public GameObject itemPrefab;

    Controls controls;

    private bool debugAddItemPressed = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        controls = new Controls();

        controls.Gameplay.AddItem.performed += ctx => debugAddItemPressed = true;
        controls.Gameplay.AddItem.canceled += ctx => debugAddItemPressed = false;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    public void AddItem(Item newItem)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                newItem.transform.SetParent(slot.transform, false);
                newItem.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
                slot.item = newItem;
                return;
            }

            print("1:" + slot.item + " " + "2:"+ slot.IsEmpty);
        }
        Debug.Log("Inventory full!");
    }

    public void RemoveItem(Item item)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.item = null;
                return;
            }
        }
    }

    void FixedUpdate()
    {
        if (debugAddItemPressed)
        {
            AddItem(Instantiate(itemPrefab).GetComponent<Item>());
        }

        debugAddItemPressed = false;
    }
}
