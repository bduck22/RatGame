using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class PotionsCreate : MonoBehaviour
{
    [Header("약물 제작법")]
    public PotionData[] PotionDatas;


    public DropInventory[] HerbSlot = new DropInventory[2];   // 약초 슬롯
    public int[] HerbSlotAnount = new int[2];       // 허브 비율
    public Slider HerbAmountSlider;
    public CreateItems CreateItem; 
    public ItemSlot ResultSlot; // 물약 슬롯

    public string ttxt; // 물약 데이터 확인 임시 텍스트

    PotionData ListPotions; // 조건들을 확인한 후 가장 높은 등급의 아이템만 생성
    int Listlevel;


    private void Start()
    {
        HerbAmountSlider = GetComponentInChildren<Slider>();
    }

    public void CreatePotion() // 물약 생성 함수
    {
        if (HerbSlot[0].inventorySlot.item == null || HerbSlot[1].inventorySlot.item == null) return; // 아이템이 두곳중 한곳이라도 비어 있으면 

        HerbSlotAnount[0] = (int)((HerbAmountSlider.value + 0.05f) * 10f);            // 1번 슬롯 비율
        HerbSlotAnount[1] = 10 - (int)((HerbAmountSlider.value + 0.05f) * 10f);       // 2번 슬롯 비율

       

        for (int i = 0; i < PotionDatas.Length; i++)
        {
            var herbA = HerbSlot[0].inventorySlot.item;
            var herbB = HerbSlot[1].inventorySlot.item;
            var data = PotionDatas[i];



            // 모든 재료가 맞을 때
            if ((data.Herb1 == herbA && data.Herb2 == herbB) || (data.Herb1 == herbB && data.Herb2 == herbA))
            {
                if (herbA == herbB)
                {
                    if (Mathf.Abs(data.HerbAmount1 - HerbSlotAnount[0]) <= 2 || Mathf.Abs(data.HerbAmount1 - HerbSlotAnount[1]) <= 2) // 오차범위 2이하 일때
                    {
                        PotionLevel(3, data);

                    }
                    else
                    {

                        PotionLevel(2, data);
                    }
                }
                else
                {
                    if (data.Herb1 == herbA)
                    {
                        if (Mathf.Abs(data.HerbAmount1 - HerbSlotAnount[0]) <= 2)
                        {

                            PotionLevel(3, data);

                        }
                        else
                        {

                            PotionLevel(2, data);
                        }


                    }
                    else
                    {
                        if (Mathf.Abs(data.HerbAmount1 - HerbSlotAnount[1]) <= 2)
                        {

                            PotionLevel(3, data);

                        }
                        else
                        {

                            PotionLevel(2, data);
                        }
                    }
                }




            }
            // 한재료만 맞을때
            else if ((data.Herb1 == herbA || data.Herb2 == herbB) || (data.Herb1 == herbB || data.Herb2 == herbA))
            {

                PotionLevel(1, data);
            }
            // 둘 다 틀릴 때
            else
            {
                PotionLevel(0, data);
            }



        }

        GameObject item = CreateItem.CreateItem(ListPotions, ResultSlot.transform);

        HerbSlot[0].draggedItem.gameObject.SetActive(false);
        HerbSlot[0].ReSet();
        HerbSlot[1].draggedItem.gameObject.SetActive(false);
        HerbSlot[1].ReSet();

        Listlevel = 0;
        ListPotions = null;

    }

    public void PotionLevel(int level, PotionData potion)
    {


        // 나중에 도감에 등록
        switch (level)
        {

            case 1:
                Debug.Log("일반 등급");
                break;

            case 2:
                Debug.Log("희귀 등급");
                break;

            case 3:
                Debug.Log("전설 등급");
                break;

            default:
                Debug.Log("이건 약물을 만들 수 없어!!!");
                break;

        }

        if (Listlevel < level)      // 약물 등급이 더 높은 아이템만 취급
        {
            Listlevel = level;
            ListPotions = potion;
        }




    }
}
