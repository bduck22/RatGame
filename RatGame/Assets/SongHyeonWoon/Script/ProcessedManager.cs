using System;
using UnityEngine;


[Serializable]
public class ItemProcessed
{
    public DropInventory processedSlot;   // ���� ����
    public DropInventory resulSlot;         // ��� ����
    public int possibility;


}
public class ProcessedManager : MonoBehaviour
{
    public ItemData[] itemDatas; // ������ ������ ������
    public ItemData failedItem; // ���� ������ ������
    public CreateItems CreateItems; // ������ ���� �޼���
    public ItemProcessed[] itemProcessed;
 



    public void ProcessedItem(int proceddedNum) // ������ ���� �ż���
    {
        ItemData item;
        ItemData chickItem;

        item = itemProcessed[proceddedNum - 1].processedSlot.inventorySlot.item;
        if (itemProcessed[proceddedNum - 1].processedSlot.inventorySlot.item != null && !itemProcessed[proceddedNum - 1].processedSlot.inventorySlot.item.isProcessed && itemProcessed[proceddedNum - 1].resulSlot.inventorySlot.item == null) // �������� �����Ѵٸ� ���� �������� �������� �ʾҴٸ�, ���� ��� â�� ��� ������
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
            {

                if (itemProcessed[proceddedNum - 1].possibility == item.itemProcessedWay[i]) // �ڽ��� ��������̶� ���� ���
                {
                    chickItem = item;
                    
                    Debug.Log("������ ����");
                    ProcessedListChick(chickItem ,item.itemProcessedWay[i], itemProcessed[proceddedNum - 1], false);
                    break;
                }
                count++;
            }


            if (count >= 3) // �׷��� ã�� ��������
            {
                
                ProcessedListChick(failedItem, 0, itemProcessed[proceddedNum - 1], true);
                Debug.Log("������");
              
            }


          

        }
        else
        {
            Debug.Log("�������� ������ �� �����ϴ�");
        }
    }

    public void ProcessedListChick(ItemData item,int processedNum, ItemProcessed process, bool isfailed) // ���� ����Ʈ Ȯ��
    {

        process.processedSlot.draggedItem.gameObject.SetActive(false); // ���� ���� ����
        process.processedSlot.ReSet();

        if (!isfailed) // ���ۿ� ���� ���� ��
        {
            for (int i = 0; i < itemDatas.Length; i++)
            {
                if (item.itemName == itemDatas[i].itemName && itemDatas[i].itemProcessedWay[0] == processedNum) // ���� �̸��� ������ ���� ������ ������ ���ƾ� �Ѵ�
                {
                    process.resulSlot.inventorySlot.item = itemDatas[i];
                    process.resulSlot.inventorySlot.count += 1;
                    CreateItems.CreateItem(itemDatas[i], process.resulSlot.transform);

                    break;
                }
            }
        }
        else // ���� ���� ��
        {
            process.resulSlot.inventorySlot.item = failedItem;
            process.resulSlot.inventorySlot.count += 1;
            CreateItems.CreateItem(failedItem, process.resulSlot.transform); // ���� ���� ������ ����
        }

      
    }



}
