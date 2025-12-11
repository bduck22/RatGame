using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Herb")]
public class HerbData : ItemBase
{
    public int[] itemProcessedWay = new int[3];

    public override void SetValue(ItemBase item)
    {
        base.SetValue(item);

        if (item is HerbData herb)
        {
            itemProcessedWay = (int[])herb.itemProcessedWay.Clone();
        }
    }
}
