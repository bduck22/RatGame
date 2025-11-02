using UnityEngine;
using static UnityEditor.Progress;

public class AddItemButton : MonoBehaviour
{
    public InventoryManager invMan;
    public ItemBase[] itemDatas;
    public CreateItems CreateItem;

    private void Start()
    {
        invMan = InventoryManager.instance;
    }

    public void Additem(int itemIndex)
    {

        if (itemDatas[itemIndex] != null)
        {

            




            invMan.AddItem(itemDatas[itemIndex]);


            Transform saveNullSlot = null;
            for (int i = 0; i < invMan.inventoryCount; i++)
            {
                //dragItem.SetPos(invMan.inventorySlot[itemIndex].transform.GetChild(0));
                if (invMan.inventorySlot[i].item == itemDatas[itemIndex])
                {
                    GameObject item = CreateItem.CreateItem(itemDatas[itemIndex], invMan.inventorySlot[i].transform.GetChild(0));
                   
                    return;
                }
                else if (invMan.inventorySlot[i].item == null && saveNullSlot == null)
                {
                    saveNullSlot = invMan.inventorySlot[i].transform.GetChild(0);
                }


            }

            if (saveNullSlot == null) { Debug.Log("인벤토리의 빈공간이 없습니다"); return; }
 

        }


       
        
        
    }

}
