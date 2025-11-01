using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CreateItems : MonoBehaviour
{
    public GameObject ItemPrefab;
    public List<GameObject> ItemList;

    private GameObject ListChick() // ����Ʈ�� üũ�Ͽ� �������� ���� �Ǵ� ��Ȱ��
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].activeSelf == false) // ���� �������� ��Ȱ��ȭ ���¶��
            {
                ItemList[i].SetActive(true);
                
                return ItemList[i];
            }
        }

        GameObject item = Instantiate(ItemPrefab);
        ItemList.Add(item);
        return item;
    }
    public GameObject CreateItem(ItemData itemData, Transform itemPos)
    {
        GameObject itemPrefab = ListChick();
        itemPrefab.GetComponent<Image>().sprite = itemData.itemImage;
        itemPrefab.name = itemData.itemName;


        DragItem dragItem = itemPrefab.GetComponent<DragItem>();

        dragItem.itemData = itemData; // ������ ������ �ʱ�ȭ
        
        dragItem.SetPos(itemPos);
        return dragItem.gameObject;
 

    }
}
