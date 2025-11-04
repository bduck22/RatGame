using System;
using UnityEngine;

[Serializable]
public class ItemClass
{
    public ItemType itemType;
    public int itemNumber=-1;           // 아이템 번호
    public int ItemCount=0;            // 아이템 개수
    public int ProcessWay=-1;           // 가공 유형
    public float Completeness;       // 완성도
    public bool shap;                // 형태

    public int amount1 = -1;
    public int amount2 = -1;
    public HerbData herb1;
    public int process1 = -1;
    public HerbData herb2;
    public int process2 = -1;
}
