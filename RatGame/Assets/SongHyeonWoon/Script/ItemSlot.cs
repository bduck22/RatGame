using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotUI
{

    public Text countText;
    public ItemSlot slot;

    public void SetSlot(ItemSlot newSlot)
    {
        slot = newSlot;



        UpdateUI();
    }

    public void UpdateUI()
    {
        if (slot == null || slot.IsEmpty)
        {
            if (countText != null)
                countText.text = "";
        }
        else
        {
            if (countText != null)
                countText.text = slot.count > 1 ? slot.count.ToString() : "";
        }
    }
}

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    public int count;
    public SlotUI slotUI = new SlotUI();
    public int MaxCount;

    public bool IsEmpty => item == null;

    private void Start()
    {
        slotUI.countText = GetComponentInChildren<Text>();

        slotUI.SetSlot(this);
    }

    public void AddItem(ItemData newitem, int amount = 1)
    {
        if (count >= MaxCount) { Debug.Log("추가 불가능"); return; }

        if (item == null)
        {
            item = newitem;
            count = amount;
        }
        else if (item == newitem)
        {

            count += amount;
        }

        slotUI.UpdateUI();
    }

    public void RemoveItem(int amount = 1)
    {
        count -= amount;
        if (count <= 0)
        {
            item = null;
            count = 0;
        }

        slotUI.UpdateUI();
    }
}
