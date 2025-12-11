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

    public int itemLevel = 0; // 0 ±âº»°ª, 1 ÀÏ¹Ý, 2 Èñ±Í, 3 Àü¼³

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
}
