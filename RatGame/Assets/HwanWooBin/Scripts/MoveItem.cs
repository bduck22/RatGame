
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveItem : MonoBehaviour
{
    [SerializeField] bool moving;
    Image image;
    RaycastResult selectUI;
    Transform OrigionPos;
    RectTransform retPos;
    public int itemIndex;
    InventoryManger inventoryManager;

    void Start()
    {
        image = GetComponent<Image>();
        OrigionPos = transform.parent;
        retPos = GetComponent<RectTransform>();
        inventoryManager = GameManager.Instance.inventoryManager;
    }


    void Update()
    {
        if (moving)
        {
#if UNITY_EDITOR

            transform.position = Input.mousePosition; // 마우스 위치로 오브젝트 이동
#endif
            if (Input.touchCount > 0)
            {
                transform.position = Input.GetTouch(0).position;
            }
        }


    }

    Transform LastParent;
    public void MoveOn()
    {
        // 최상위 캔버스로 이동
        // 맨 앞 레이어로 보이기
        moving = true;
        image.raycastTarget = false;


        itemIndex = int.Parse(transform.parent.name);
<<<<<<< HEAD
<<<<<<< HEAD
        LastParent = transform.parent;      // 현재 부모 저장 -> 이미지 위치 복귀용
=======
=======
>>>>>>> df3f6d5584789f7cc3980050d125a99ada8bb00c
        LastParent = transform.parent;
>>>>>>> df3f6d5584789f7cc3980050d125a99ada8bb00c
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

    }
    public void MoveOff()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current); // 포인터 데이터, 클릭, 터치 등이 발생할 때
        List<RaycastResult> results = new List<RaycastResult>(); // 레이캐스트의 결과 즉 UI 요소들을 담을 리스트
#if UNITY_EDITOR
        pointerData.position = Input.mousePosition;
#endif
        if (Input.touchCount > 0)
        {
            pointerData.position = Input.GetTouch(0).position;
        }

        EventSystem.current.RaycastAll(pointerData, results); // pointerData 즉 현재 위치에 있는 모든 UI요소들 담기

        if (results.Count > 0) // UI 요소가 하나라도 있다면
        {
            selectUI = results[0]; // 가장 위에 있는 UI 요소 선택
        }

        transform.SetParent(LastParent);
        transform.SetSiblingIndex(0);   //  형제 오브젝트 사이 가장 위쪽으로 이동
        transform.localPosition = Vector3.zero;
        Checking();
    }

    public void Checking()
    {
        moving = false;
        image.raycastTarget = true;
<<<<<<< HEAD
<<<<<<< HEAD
            if (selectUI.gameObject.GetComponent<DropSlot>()) // DropSlot타입인지 확인
        {
                DropSlot dropSlot = selectUI.gameObject.GetComponent<DropSlot>();
                if (dropSlot.Item.itemNumber == -1&&!dropSlot.Lock) // dropSlot이 비어있고 잠겨있지 않다면
            {
                    inventoryManager.inventory[itemIndex].ItemCount--; // 인벤토리에서 아이템 개수 감소
                    dropSlot.Item = inventoryManager.inventory[itemIndex]; // dropSlot에 아이템 할당
                if (inventoryManager.inventory[itemIndex].ItemCount <= 0) // 인벤토리에 남은 아이템이 없다면
=======
=======
>>>>>>> df3f6d5584789f7cc3980050d125a99ada8bb00c
        if (selectUI.gameObject.GetComponent<DropSlot>()||(selectUI.gameObject.transform.parent&&selectUI.gameObject.transform.parent.GetComponent<DropSlot>()))
        {
            DropSlot dropSlot;
            if (selectUI.gameObject.GetComponent<DropSlot>())
            {
                dropSlot = selectUI.gameObject.GetComponent<DropSlot>();
            }
            else
            {
                dropSlot = selectUI.gameObject.transform.parent.GetComponent<DropSlot>();
            }

            if (dropSlot.Item.itemNumber == -1)
            {
                if (!dropSlot.Lock)
                {
                    inventoryManager.inventory[itemIndex].ItemCount--;
                    dropSlot.Item = inventoryManager.inventory[itemIndex];
                    if (inventoryManager.inventory[itemIndex].ItemCount <= 0)
>>>>>>> df3f6d5584789f7cc3980050d125a99ada8bb00c
                    {
                        inventoryManager.inventory.RemoveAt(itemIndex); // 인벤토리에서 아이템 제거(itemIndex칸에 있는 인벤토리를)
                }

                    inventoryManager.UpdateInventory(); // 인벤토리 UI 업데이트
                    dropSlot.Load(); // dropSlot UI 업데이트?
            }
<<<<<<< HEAD
<<<<<<< HEAD
        }
        transform.SetParent(OrigionPos);     // 원래 부모로 복귀-> 즉 이미지를 선택된 슬롯의 자식 이미지로 이동하지 않고
                                             // 선택된 슬롯에 있는 moveImage와 Data와 UI만 변경하는 방식이구나!!!!!

=======
=======
>>>>>>> df3f6d5584789f7cc3980050d125a99ada8bb00c
            else if (!dropSlot.Lock)
            {
                dropSlot.BackItem();

                inventoryManager.inventory[itemIndex].ItemCount--;
                dropSlot.Item = inventoryManager.inventory[itemIndex];
                if (inventoryManager.inventory[itemIndex].ItemCount <= 0)
                {
                    inventoryManager.inventory.RemoveAt(itemIndex);
                }

                inventoryManager.UpdateInventory();
                dropSlot.Load();
            }
        }
        transform.SetParent(OrigionPos);
>>>>>>> df3f6d5584789f7cc3980050d125a99ada8bb00c
        retPos.localPosition = Vector3.zero;
    }
}