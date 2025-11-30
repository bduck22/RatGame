using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionLocalizationData", menuName = "AssetsData/Herb")]

public class PotionLocalizationData : GoogleLocalizationData
{
    public override void SetData(int i, string[] itemdata)
    {
        base.SetData(i, itemdata);
        HerbData herb = items[i] as HerbData;
        herb.itemProcessedWay = new List<int>();

        for (int j = 4; j < itemdata.Length; j++)
        {
            if (!string.IsNullOrEmpty(itemdata[j]) && itemdata[j].Trim() != "null")
            {
                Debug.Log(itemdata[j]);
                herb.itemProcessedWay.Add(int.Parse(itemdata[j].Trim()));

            }
        }
    }
}
