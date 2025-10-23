using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotUI
{
    public Image ItemImage;
    public Text countText;
    public Sprite DefaultImage;
    private InventorySlot slot;

    public void SetSlot(InventorySlot newSlot)
    {
        slot = newSlot;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (slot == null || slot.IsEmpty)
        {
            ItemImage.sprite = DefaultImage;  // 참조 유지
            countText.text = "";
        }
        else
        {

            ItemImage.sprite = slot.item.itemImage;
            countText.text = slot.count > 1 ? slot.count.ToString() : "";
        }
    }
}


public class InventorySlotUI : MonoBehaviour
{
    public List<SlotUI> slotUIs;
    public List<Button> inventoryButtons;
    public InventoryManager invMan;
    public GameObject InvoentoryIcon;
    void Start()
    {
        invMan = InventoryManager.instance;
        for (int i=0; i < invMan.inventoryCount; i++)
        {
            GameObject buttonicon = Instantiate(InvoentoryIcon);
            buttonicon.transform.SetParent(transform, false);

            Button btn = buttonicon.GetComponent<Button>();
            inventoryButtons.Add(btn);

            slotUIs.Add(new SlotUI());
            slotUIs[i].ItemImage = buttonicon.GetComponent<Image>();
            slotUIs[i].DefaultImage = slotUIs[i].ItemImage.sprite;
            slotUIs[i].countText = buttonicon.transform.GetChild(0).GetComponent<Text>();

            btn.onClick.RemoveAllListeners();
           // 람다 사용
            int index = i; // 반드시 지역 변수
            btn.onClick.AddListener(() =>OnSetButton(index));
        }


        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].SetSlot(invMan.inventorySlot[i]);
        }
    }

    public void OnSetButton(int index)
    {
        invMan.RemoveItem(index);
    }

}
