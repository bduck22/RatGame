using UnityEngine;


[CreateAssetMenu(menuName = "Item/Herb")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public bool isProcessed;
    public int[] itemProcessedWay; // 1 ���̱�, 2 �±�, 3 ������


    public ItemData()
    {
        if (!isProcessed) itemProcessedWay = new int[3];
        else itemProcessedWay = new int[1];
    }
}
