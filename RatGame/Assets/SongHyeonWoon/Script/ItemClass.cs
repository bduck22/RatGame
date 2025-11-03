using System;
using UnityEngine;

[Serializable]
public class ItemClass
{
    public ItemType itemType;
    public int itemNumder=-1;           // 아이템 번호
    public int ItemCount=0;            // 아이템 개수
    public int ProcessWay=-1;           // 가공 유형
    public float Completeness;       // 완성도
    public bool shap;                // 형태
}
