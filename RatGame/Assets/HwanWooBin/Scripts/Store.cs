using UnityEngine;

public class Store : MonoBehaviour
{
    public int[] Prices;

    public void BuyHerb(int number)
    {
        if (GameManager.Instance.IsBuyed(Prices[number]))
        {
            ItemClass itemdummy = new ItemClass();
            itemdummy.itemNumber = number;
            itemdummy.ItemCount = 1;
            itemdummy.shap = false;
            itemdummy.ProcessWay = -1;
            itemdummy.itemType = ItemType.Herb;
            GameManager.Instance.AddItem(itemdummy);
        }
    }
}
