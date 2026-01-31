using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Transform originalParent;
    Vector3 originalPosition;

    public float hoverLift = 40f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // ?? Hover lift
    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += Vector2.up * hoverLift;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.anchoredPosition -= Vector2.up * hoverLift;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        transform.SetParent(originalParent.root); // move above hand
        canvasGroup.blocksRaycasts = false;
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.blocksRaycasts = true;
    }
}
