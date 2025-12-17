
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class ExampleBoad
{
    public Image Image;
    public TextMeshProUGUI[] text = new TextMeshProUGUI[2]; // 약초 이름, 설명
    public Material boadMaterial;

}
public class MoveItem : MonoBehaviour
{
    [SerializeField] ExampleBoad exampleBoad; // 설명 보드
    [SerializeField] bool moving;
    Image image;
    Coroutine cc;
    RaycastResult selectUI;
    Transform OrigionPos;
    Transform LastParent;
    RectTransform retPos;
    public int itemIndex;
    InventoryManger inventoryManager;

    void Start()
    {
       
        image = GetComponent<Image>();
        OrigionPos = transform.parent;
        retPos = GetComponent<RectTransform>();
        inventoryManager = GameManager.Instance.inventoryManager;
        exampleBoad.boadMaterial = exampleBoad.Image.material;

    }

    private void OnEnable()
    {
        if (int.TryParse(transform.parent.name, out int value))
        {
            itemIndex = int.Parse(transform.parent.name);
        }

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
    public void OnClick()
    {
        //Debug.Log("--클릭 시작--");
        if (cc != null) StopCoroutine(cc);
        cc = StartCoroutine(WaitActiveTime());
    }

    public void OnExit()
    {
        //Debug.Log("--클릭 종료--");
        if(LastParent != null)
        exampleBoad.Image.transform.SetParent(LastParent);
        if (cc != null) StopCoroutine(cc);
        exampleBoad.Image.gameObject.SetActive(false);
    }

    public void MoveOn()
    {
        // 최상위 캔버스로 이동
        // 맨 앞 레이어로 보이기
        moving = true;
        image.raycastTarget = false;
        itemIndex = int.Parse(transform.parent.name);
        LastParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        OnExit();   // 설명창 보이지 않게 하기

    }
    public void MoveOff()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
#if UNITY_EDITOR
        pointerData.position = Input.mousePosition;
#endif
        if (Input.touchCount > 0)
        {
            pointerData.position = Input.GetTouch(0).position;
        }

        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            selectUI = results[0];
        }

        transform.SetParent(LastParent);
        transform.SetSiblingIndex(0);
        transform.localPosition = Vector3.zero;
        Checking();
    }

    public void Checking()
    {
        moving = false;
        image.raycastTarget = true;
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
                    {
                        inventoryManager.inventory.RemoveAt(itemIndex);
                    }

                    inventoryManager.UpdateInventory();
                    dropSlot.Load();
                }
            }
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
        retPos.localPosition = Vector3.zero;
    }

    IEnumerator WaitActiveTime()
    {
        yield return new WaitForSeconds(0.8f);
        exampleBoad.Image.gameObject.SetActive(true); // 설명창 보이기
        exampleBoad.text[0].text = "<color=orange>" + (inventoryManager.inventory[itemIndex].itemName)+ "</color>";
        exampleBoad.text[1].text = inventoryManager.inventory[itemIndex].itemDescription;

        exampleBoad.boadMaterial.SetFloat("_OnAnima", inventoryManager.inventory[itemIndex].itemType == ItemType.Potion ? 1 : 0);

       LastParent = transform.parent;
        exampleBoad.Image.transform.SetParent(transform.root);
        exampleBoad.Image.transform.SetAsLastSibling(); // 맨앞으로
        //Debug.Log("손닿음");
    }
}