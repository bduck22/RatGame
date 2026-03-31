using System.Linq;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Potion")]
public class PotionData : ItemBase
{
    public HerbData Herb1;
    public int process1 = -1;
    public HerbData Herb2;
    public int process2 = -1;

    public int HerbAmount1;
    public int HerbAmount2;

    public int itemLevel = -1; // 0 ±âº»°ª, 1 ÀÏ¹İ, 2 Èñ±Í, 3 Àü¼³

    public bool NonWater;
    public Sprite NonShapeImage;
    public string NonShapeName;

    public int[] Persents = new int[4];

    public override void SetValue(ItemBase item)
    {
        base.SetValue(item);

        if (item is PotionData potion)
        {
            Herb1 = potion.Herb1;
            process1 = potion.process1;
            Herb2 = potion.Herb2;
            process2 = potion.process2;
            HerbAmount1 = potion.HerbAmount1;
            HerbAmount2 = potion.HerbAmount2;
            itemLevel = potion.itemLevel;
            NonWater = potion.NonWater;
            NonShapeName = potion.NonShapeName;
            Persents = (int[])potion.Persents.Clone();
        }
    }

    public override ItemBase InitData(string[] datas)
    {
        base.InitData(datas);

        this.itemLevel = (int)(Level)Enum.Parse(typeof(Level), datas[4]);

        this.Herb1 = ItemDatas.instance.items[(int)(Herbs)Enum.Parse(typeof(Herbs), datas[5])] as HerbData;
        this.Herb2 = ItemDatas.instance.items[(int)(Herbs)Enum.Parse(typeof(Herbs), datas[7])] as HerbData;

        this.process1 = int.Parse(datas[6]);
        this.process2 = int.Parse(datas[8]);

        this.HerbAmount1 = int.Parse(datas[9]);
        this.HerbAmount2 = int.Parse(datas[10]);

        if (datas[11] == "¾×Ã¼")
        {
            this.NonWater = false;
        }
        else
        {
            this.NonWater = true;
        }

        this.Persents = (int[])datas[12].Split('/').Select(s => int.Parse(s)).ToArray();

        return this;
    }
}
