using UnityEngine;

public class AddItemButton : MonoBehaviour
{
    public InventoryManager invMan;
    public GameObject itemPrefab;
    public ItemData[] itemDatas;

    private void Start()
    {
        invMan = InventoryManager.instance;
    }

    public void Additem(int itemIndex)
    {

        if (itemDatas[itemIndex] != null)
        {
            GameObject item = Instantiate(itemPrefab);

            DragItem dragItem = item.GetComponent<DragItem>();

            dragItem.itemData = itemDatas[itemIndex];


            invMan.AddItem(itemDatas[itemIndex]);


            Transform saveNullSlot = null;
            for (int i = 0; i < invMan.inventoryCount; i++)
            {
                //dragItem.SetPos(invMan.inventorySlot[itemIndex].transform.GetChild(0));
                if (invMan.inventorySlot[i].item == itemDatas[itemIndex])
                {
                    dragItem.SetPos(invMan.inventorySlot[i].transform.GetChild(0));
                    return;
                }
                else if (invMan.inventorySlot[i].item == null && saveNullSlot == null)
                {
                    saveNullSlot = invMan.inventorySlot[i].transform.GetChild(0);
                }


            }

            if (saveNullSlot == null) { Debug.Log("인벤토리의 빈공간이 없습니다"); return; }
            dragItem.SetPos(saveNullSlot);
            Debug.Log("아이템 넣음");

        }


       
        
        
    }

}
