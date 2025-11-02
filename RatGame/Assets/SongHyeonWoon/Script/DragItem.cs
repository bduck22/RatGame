using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform OriginalParent { get; private set; }
    public RectTransform retPos { get; private set; }
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Image image;

    public ItemBase itemData;
  


    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        image = GetComponent<Image>();
        retPos = GetComponent<RectTransform>();

        image.sprite = itemData.itemImage;
    }

    public void SetPos(Transform pos)
    {
        if (pos == null) return;
        if(retPos == null) retPos = GetComponent<RectTransform>();

        OriginalParent = pos;
        transform.SetParent(pos);
        retPos.localPosition = Vector3.zero;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalParent = transform.parent;
        transform.SetParent(canvas.transform);

        transform.SetAsLastSibling(); // 맨 앞 레이어로 보이기

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        retPos.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    
        // 슬롯 밖에 놓은 경우 원래 자리로 돌아가게 처리
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(OriginalParent);
            retPos.localPosition = Vector3.zero;

        }


    }
}
