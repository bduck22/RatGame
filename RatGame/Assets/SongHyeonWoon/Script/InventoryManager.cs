using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;

    public bool IsEmpty => item == null;


    public void AddItem(ItemData newitem, int amount = 1) // 아이템 추가하기
    {

        if (item == null) // 슬롯이 비어있으면
        {
            item = newitem;
            count = amount;
        }
        else if (item == newitem) // 아이템이 이미 존재하면
        {
            count += amount;
        }

    }

    public void RemoveItem(int amount = 1)
    {
        Debug.Log("삭제됨");
        count -= amount;
        if (count <= 0) // 아이템이 1개만 존재하면
        {
            item = null;
            count = 0;
        }
    }
}
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventorySlot[] inventorySlot;
    public int inventoryCount = 10;
    public InventorySlotUI inventorySlotUI;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < inventoryCount; i++)
        {
            inventorySlot[i] = new InventorySlot();
        }

        inventorySlotUI = GetComponent<InventorySlotUI>();

    }

    public void AddItem(ItemData AdditemData)
    {
        InventorySlot emptySlot = null;
        for(int i=0; i<inventoryCount; i++)
        {
            if (inventorySlot[i].item == AdditemData)
            {
                inventorySlot[i].AddItem(AdditemData);
                inventorySlotUI.UpdateInventoryUI();
                return;
            }
            if (emptySlot == null && inventorySlot[i].item == null)
            {
                emptySlot = inventorySlot[i];
            }
        }

        if(emptySlot != null)
        {
            emptySlot.AddItem(AdditemData);
            inventorySlotUI.UpdateInventoryUI();
        }
        else
        {
            Debug.Log("인벤토리 공간이 부족합니다");
        }
       
    }

    public void RemoveItem(int inventoryIndex)
    {
        inventorySlot[inventoryIndex].RemoveItem();
        inventorySlotUI.UpdateInventoryUI();
        Debug.Log("삭제시도 " + inventoryIndex);
    }


}
