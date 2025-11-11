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
}
