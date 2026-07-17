using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public sealed class DraggableMailItem :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField]
    private string mailItemId;

    [SerializeField]
    private string correctDestinationId;

    [SerializeField]
    private RectTransform dragLayer;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Transform startingParent;
    private int startingSiblingIndex;
    private Vector2 startingAnchoredPosition;

    private bool isDragging;
    private bool isSorted;

    public string MailItemId => mailItemId;
    public bool IsSorted => isSorted;

    public event Action<DraggableMailItem> Sorted;

    private void Awake()
    {
        rectTransform =
            GetComponent<RectTransform>();

        canvasGroup =
            GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(
        PointerEventData eventData)
    {
        if (isSorted)
            return;

        if (dragLayer == null)
        {
            Debug.LogError(
                $"{name} has no Drag Layer assigned.",
                this
            );

            return;
        }

        isDragging = true;

        startingParent =
            transform.parent;

        startingSiblingIndex =
            transform.GetSiblingIndex();

        startingAnchoredPosition =
            rectTransform.anchoredPosition;

        transform.SetParent(
            dragLayer,
            true
        );

        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.9f;
        canvasGroup.blocksRaycasts = false;

        MoveToPointer(eventData);
    }

    public void OnDrag(
        PointerEventData eventData)
    {
        if (!isDragging || isSorted)
            return;

        MoveToPointer(eventData);
    }

    public void OnEndDrag(
        PointerEventData eventData)
    {
        if (!isDragging)
            return;

        isDragging = false;

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        if (!isSorted)
            ReturnToStartingPosition();
    }

    public bool TryPlaceAtDestination(
        MailDestination destination)
    {
        if (isSorted || destination == null)
            return false;

        bool isCorrectDestination =
            string.Equals(
                correctDestinationId.Trim(),
                destination.DestinationId.Trim(),
                StringComparison.OrdinalIgnoreCase
            );

        if (!isCorrectDestination)
            return false;

        isSorted = true;
        isDragging = false;

        transform.SetParent(
            destination.SnapPoint,
            false
        );

        rectTransform.anchorMin =
            new Vector2(0.5f, 0.5f);

        rectTransform.anchorMax =
            new Vector2(0.5f, 0.5f);

        rectTransform.pivot =
            new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition =
            Vector2.zero;

        rectTransform.localRotation =
            Quaternion.identity;

        rectTransform.localScale =
            Vector3.one;

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = false;

        Sorted?.Invoke(this);

        return true;
    }

    private void MoveToPointer(
        PointerEventData eventData)
    {
        if (dragLayer == null)
            return;

        bool convertedPosition =
            RectTransformUtility
                .ScreenPointToLocalPointInRectangle(
                    dragLayer,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPointerPosition
                );

        if (!convertedPosition)
            return;

        rectTransform.anchoredPosition =
            localPointerPosition;
    }

    private void ReturnToStartingPosition()
    {
        if (startingParent == null)
            return;

        transform.SetParent(
            startingParent,
            false
        );

        transform.SetSiblingIndex(
            startingSiblingIndex
        );

        rectTransform.anchoredPosition =
            startingAnchoredPosition;

        rectTransform.localRotation =
            Quaternion.identity;

        rectTransform.localScale =
            Vector3.one;
    }
}