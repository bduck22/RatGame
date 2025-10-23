using UnityEngine;

public class AddItemButton : MonoBehaviour
{
    public InventoryManager invMan;
    public ItemData[] itemDatas;


    public void Additem(int itemIndex)
    {


        invMan.AddItem(itemDatas[itemIndex]);
    }
}
