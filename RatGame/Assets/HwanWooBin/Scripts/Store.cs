using UnityEngine;

public class Store : MonoBehaviour
{
    public int[] Prices;

    public void BuyHerb(int number)
    {
        if (GameManager.Instance.IsBuyed(Prices[number]))
        {
            GameManager.Instance.AddItem(number, -1, -1, false);
        }
    }
}
