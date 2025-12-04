using UnityEngine;

public class DarkStore : MonoBehaviour
{
    [Header("À§Çèµµ")]
    public int RiskLevel;

    [Header("¿ÀÇÂ È®·ü")]
    public int OpenProbability = 40;


    ItemDatas ItemDatas;
    //public StoreUI uis;

    public ItemClass selectPotion;
    public int selectpotionnumber;
    private void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;
    }

    private void OnEnable()
    {
        //uis.Reload();
    }

    public void BuyHerb(int number)
    {
        ItemBase data = ItemDatas.items[number];

        if (GameManager.Instance.IsBuyed((int)data.Price))
        {

            GameManager.Instance.inventoryManager.deliverycounts[number]++;
            GameManager.Instance.report.UseCoinInStore(data, true);

           // uis.Reload();
        }
    }

    public void Upgrade(int number)
    {
      
    }

    public void Setpotion(int number)
    {
        selectpotionnumber = number;
        selectPotion = GameManager.Instance.inventoryManager.inventory[selectpotionnumber];
    }

    public void Sell()
    {

        GameManager.Instance.Money += (selectPotion.Completeness * 0.01f) * GameManager.Instance.itemDatas.items[selectPotion.itemNumber].Price;
        GameManager.Instance.report.SellPotion.Add(GameManager.Instance.inventoryManager.inventory[selectpotionnumber]);
        GameManager.Instance.inventoryManager.inventory.RemoveAt(selectpotionnumber);


        //uis.Reload();
    }


}
