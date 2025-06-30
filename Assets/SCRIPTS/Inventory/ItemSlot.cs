using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ItemSlot : MonoBehaviour, IDropHandler
{
    public Item item;
    public bool IsEmpty => item == null;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped != null)
        {
            dropped.transform.SetParent(transform);
            dropped.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
