
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

    // Update is called once per frame 
    void Update()
    {
        if (moving)
        {
#if UNITY_EDITOR

            transform.position = Input.mousePosition; // 마우스 위치로 오브젝트 이동
            

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            // RaycastAll 사용
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            
            if (results.Count > 0)
            {
                selectUI = results[0];
            }


#endif
            if (Input.touchCount > 0)
            {
                transform.position = Input.GetTouch(0).position;

                PointerEventData pointerData2 = new PointerEventData(EventSystem.current);
                pointerData.position = Input.GetTouch(0).position;

                // RaycastAll 사용
                List<RaycastResult> results2 = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData2, results2);

                if (results2.Count > 0)
                {
                    selectUI = results2[0];
                }
            }
        }


    }


    public void MoveOn()
    {
         // 최상위 캔버스로 이동
         // 맨 앞 레이어로 보이기
        moving = true;
        image.raycastTarget = false;


        for (int i=0;i<transform.parent.parent.childCount;i++)
        {
            if (transform.parent.parent.GetChild(i)==transform.parent)
            {
                itemIndex = i;
            }
        }
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

    }
    public void MoveOff()
    {
        moving = false;
        image.raycastTarget = true;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI 위에서 뗌!" + selectUI.gameObject.name);

            selectUI.gameObject.GetComponentInParent<ISetTheItems>().SetTheItem(selectUI.gameObject, gameObject);

        }
            transform.SetParent(OrigionPos);
            retPos.localPosition = Vector3.zero;
        
           

    }
}
