using UnityEngine;

public class Store : MonoBehaviour
{
    public int[] Prices;
    ItemDatas ItemDatas;

    public StoreUI uis;

    public ItemClass selectPotion;
    public int selectpotionnumber;
    private void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;
    }

    private void OnEnable()
    {
        uis.Reload();
    }

    public void BuyHerb(int number)
    {
        ItemBase data = ItemDatas.items[number];

        if (GameManager.Instance.IsBuyed((int)data.Price))
        {
            GameManager.Instance.inventoryManager.deliverycounts[number]++;

            //ItemClass itemdummy = new ItemClass();
            //itemdummy.itemNumber = number;
            //itemdummy.ItemCount = 1;
            //itemdummy.shap = false;
            //itemdummy.ProcessWay = -1;
            //itemdummy.itemType = ItemType.Herb;
            //GameManager.Instance.AddItem(itemdummy);
            uis.Reload();
        }
    }

    public void Upgrade(int number)
    {
        if (GameManager.Instance.IsBuyed(Prices[number]))
        {
            if(number == 3)
            {
                GameManager.Instance.MouseCount++;
            }
            else
            {
                GameManager.Instance.ProcessController.ProcessLevel[number]++;
            }
            uis.Reload();
        }
    }

    public void Setpotion(int number)
    {
        selectpotionnumber = number;
        selectPotion = GameManager.Instance.inventoryManager.inventory[selectpotionnumber];
    }

    public void Sell()
    {
        
        GameManager.Instance.Money += (selectPotion.Completeness * 0.01f) * GameManager.Instance.itemDatas.items[selectPotion.itemNumber].Price;
        GameManager.Instance.inventoryManager.inventory.RemoveAt(selectpotionnumber);
    }
}
