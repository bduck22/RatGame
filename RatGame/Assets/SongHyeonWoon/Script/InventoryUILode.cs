using UnityEngine;
using UnityEngine.UI;

public class InventoryUILode : MonoBehaviour
{
    public Transform InventoryUiPos;

    public GameObject itemPre;

    public ItemDatas itemDatas;
    private void Start()
    {
        itemDatas = DataManager.instance.itemDatas;
        
    }

    public  void UpdateInventoryUI()
    {
        int i = 0;
        foreach (ItemClass item in DataManager.instance.inventory)
        {
            if(InventoryUiPos.childCount <= i)
            {
                GameObject Slot = Instantiate(itemPre, InventoryUiPos);

                Slot.transform.GetChild(0).GetComponent<Image>().sprite = itemDatas.items[item.itemNumder].itemImage;
                Slot.transform.GetChild(1).GetComponent<Text>().text = (item.ItemCount<=1?"":item.ItemCount.ToString());
            }
            else
            {
                GameObject Slot = InventoryUiPos.GetChild(i).gameObject;

                Slot.transform.GetChild(0).GetComponent<Image>().sprite = itemDatas.items[item.itemNumder].itemImage;
                Slot.transform.GetChild(1).GetComponent<Text>().text = (item.ItemCount <= 1 ? "" : item.ItemCount.ToString());
            }
            i++;
        }
    }

}
