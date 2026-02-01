using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardMovement: MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    [SerializeField] private HandManager handManager;

    [SerializeField] private float hoverLift = 20f;
    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private GameObject cardPlay;
    [SerializeField] private GameObject border; // the UI would be past this point so do not allow play there

    [SerializeField] private GameObject movePlayPosition;
    [SerializeField] private GameObject maskPlayPosition;
    [SerializeField] private GameObject glowEffect; //  same with this maybe??
    //[SerializeField] private GameObject playArrow; // probably dont need

    [SerializeField] private float lerpFactor = .2f;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalRotation = rectTransform.localRotation;
        originalPosition = rectTransform.localPosition;
        glowEffect.SetActive(false);
        //playArrow.SetActive(false);
    }
    private void Start()
    {
        handManager = HandManager.FindFirstObjectByType<HandManager>();
        movePlayPosition = GameObject.Find("MovePlayZone");
        maskPlayPosition = GameObject.Find("MaskPlayZone");
        cardPlay = GameObject.Find("Midpoint");
        border = GameObject.Find("Border");
    }

    void Update()
    {
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDragState();
                if (!Mouse.current.leftButton.isPressed)
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandleMaskPlayState();
                if (!Mouse.current.leftButton.isPressed)
                {
                    TransitionToState0();
                }
                break;
            case 4:
                HandleMovePlayState();
                if (!Mouse.current.leftButton.isPressed)
                {
                    TransitionToState0();
                }
                break;
        }
    }

    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = originalScale; // reset scale
        rectTransform.localRotation = originalRotation; // reset rotation
        rectTransform.localPosition = originalPosition; // reset position
        glowEffect.SetActive(false);
        // playArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;
            currentState = 1; // hover state
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 0; // back to normal state
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2; // drag state
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), 
                                                                    eventData.position, 
                                                                    eventData.pressEventCamera,
                                                                    out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("DRAG Mouse Y Position: " + Mouse.current.position.ReadValue().y);
        Debug.Log("DRAG Mouse X Position: " + Mouse.current.position.ReadValue().x);
    
        if (currentState == 2)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), 
                                                                        eventData.position, 
                                                                        eventData.pressEventCamera, 
                                                                        out localPointerPosition))
            {

                rectTransform.position = Vector3.Lerp(rectTransform.position, Mouse.current.position.ReadValue(), lerpFactor);

                if (Mouse.current.position.ReadValue().x > border.transform.position.x) // past the boarder so do not allow play
                {
                    //currentState = 1; // stay in drag state
                    //playArrow.SetActive(false);
                    return;
                }

                if (Mouse.current.position.ReadValue().y > cardPlay.transform.position.y) // above threshold to play the card
                    if  (Mouse.current.position.ReadValue().x < cardPlay.transform.position.x) // left side of screen
                    {
                        currentState = 3; // mask play state
                        //playArrow.SetActive(true);
                        //rectTransform.localPosition = Vector3.Lerp(rectTransform.position, maskPlayPosition, lerpFactor);
                    }
                    else if (Mouse.current.position.ReadValue().x > cardPlay.transform.position.x) // right side of screen and before the boarder
                    {
                        currentState = 4; // move play state
                        //playArrow.SetActive(true);
                        //rectTransform.localPosition = Vector3.Lerp(rectTransform.position, movePlayPosition, lerpFactor);
                    }
                    //currentState = 3;
                    //playArrow.SetActive(true);
                    //rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
                
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandleMaskPlayState() // left of screen
    {
        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, maskPlayPosition.transform.localPosition, lerpFactor);
        rectTransform.localRotation = Quaternion.identity;  // reset to 0

        //Debug.Log("MASK Mouse X Position: " + Mouse.current.position.ReadValue().x);

        if (Mouse.current.position.ReadValue().y < cardPlay.transform.position.y)
        {
            currentState = 2; // back to drag state
            //playArrow.SetActive(false);
        }

        if (Mouse.current.position.ReadValue().x > cardPlay.transform.position.x) //  greater than 0 meaning mouse is on right of screen
        {
            currentState = 3; // move to movement state
        }
    }

    private void HandleMovePlayState()
    {
        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, movePlayPosition.transform.localPosition, lerpFactor);
        rectTransform.localRotation = Quaternion.identity;  // reset to 0

        //Debug.Log("MOVE Mouse X Position: " + Mouse.current.position.ReadValue().x);

        if (Mouse.current.position.ReadValue().y < cardPlay.transform.position.y)
        {
            currentState = 2; // back to drag state
            //playArrow.SetActive(false);
        }
    }

    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;  // reset to 0
        //Debug.Log("DRAGGING");
    }
}
