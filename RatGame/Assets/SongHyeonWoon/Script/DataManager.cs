using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public ItemDatas itemDatas;

    public List<ItemClass> inventory = new List<ItemClass>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

   
}
