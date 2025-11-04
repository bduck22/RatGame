using UnityEngine;

[CreateAssetMenu(menuName = "Potion")]
public class PotionData : ItemBase
{
    public HerbData Herb1;
    public HerbData Herb2;

    public int HerbAmount1;
    public int HerbAmount2;

    public int itemLevel = 0; // 0 ±âº»°ª, 1 ÀÏ¹Ý, 2 Èñ±Í, 3 Àü¼³


}
