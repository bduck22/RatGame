using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CreateItems : MonoBehaviour
{
    public GameObject ItemPrefab;
    public List<GameObject> ItemList;

    private GameObject ListChick() // 리스트를 체크하여 아이템을 생성 또는 재활용
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].activeSelf == false) // 만약 아이템이 비활성화 상태라면
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

        dragItem.itemData = itemData; // 아이템 데이터 초기화
        
        dragItem.SetPos(itemPos);
        return dragItem.gameObject;
 

    }
}
