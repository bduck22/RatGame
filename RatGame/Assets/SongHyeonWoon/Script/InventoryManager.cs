using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;

    public bool IsEmpty => item == null;


    public void AddItem(ItemData newitem, int amount = 1) // ������ �߰��ϱ�
    {

        if (item == null) // ������ ���������
        {
            item = newitem;
            count = amount;
        }
        else if (item == newitem) // �������� �̹� �����ϸ�
        {
            count += amount;
        }

    }

    public void RemoveItem(int amount = 1)
    {
        Debug.Log("������");
        count -= amount;
        if (count <= 0) // �������� 1���� �����ϸ�
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
            Debug.Log("�κ��丮 ������ �����մϴ�");
        }
       
    }

    public void RemoveItem(int inventoryIndex)
    {
        inventorySlot[inventoryIndex].RemoveItem();
        inventorySlotUI.UpdateInventoryUI();
        Debug.Log("�����õ� " + inventoryIndex);
    }


}
