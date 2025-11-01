using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public ItemSlot[] inventorySlot;
    public int inventoryCount = 10;

    public GameObject itemSlotObject; // ������ ���� ����

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
            GameObject createitemSlot = Instantiate(itemSlotObject);
            createitemSlot.transform.SetParent(transform);
            inventorySlot[i] = createitemSlot.GetComponent<ItemSlot>();

        }

    }

    public void AddItem(ItemData AdditemData)
    {
        ItemSlot emptySlot = null;
        for(int i=0; i<inventoryCount; i++)
        {
            if (inventorySlot[i].item == AdditemData ) 
            {
                inventorySlot[i].AddItem(AdditemData);
                
               
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

        }
        else
        {
            Debug.Log("�κ��丮 ������ �����մϴ�");
        }
       
    }

    public void RemoveItem(int inventoryIndex)
    {
        inventorySlot[inventoryIndex].RemoveItem();
        Debug.Log("�����õ� " + inventoryIndex);
    }



}
