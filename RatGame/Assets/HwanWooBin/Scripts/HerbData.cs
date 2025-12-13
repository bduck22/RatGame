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

    public override ItemBase InitData(string[] datas)
    {

        base.InitData(datas);

        for(int i = 0; i < 3; i++)
        {
            this.itemProcessedWay[i] = int.Parse(datas[i+4]);
        }
        return this;
    }
}
