using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject slotsHolder;
    public GameObject tabsHolder;

    public GameObject slotPrefab;
    public Sprite tabSelected;
    public Sprite tabTex;

    public void setActiveTab(GameObject which)
    {
        foreach (Transform tab in tabsHolder.transform)
        {
            tab.GetComponent<Image>().sprite = tabTex;
        }

        which.GetComponent<Image>().sprite = tabSelected;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 28; i++)
        {
            GameObject sp = Instantiate(slotPrefab);
            sp.transform.SetParent(slotsHolder.transform, false);
            InventoryManager.Instance.slots.Add(sp.GetComponent<ItemSlot>());
        }

        setActiveTab(tabsHolder.transform.GetChild(0).gameObject);
        //showPage(pagesHolder.transform.GetChild(0).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
