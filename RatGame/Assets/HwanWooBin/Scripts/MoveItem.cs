
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

    void Start()
    {
      image = GetComponent<Image>();
      OrigionPos = transform.parent;
        retPos = GetComponent<RectTransform>();
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
        LastParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

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
        if (EventSystem.current.IsPointerOverGameObject())
        {

            if (selectUI.gameObject.GetComponent<DropSlot>())
            {
                DropSlot dropSlot = selectUI.gameObject.GetComponent<DropSlot>();

                GameManager.Instance.inventory[itemIndex].ItemCount--;
                dropSlot.Item = GameManager.Instance.inventory[itemIndex];

                GameManager.Instance.UpdateInventory();
                dropSlot.Load();
            }
            transform.localPosition = Vector3.zero;
        }
        transform.SetParent(OrigionPos);
        retPos.localPosition = Vector3.zero;
    }
}