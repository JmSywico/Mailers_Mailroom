using UnityEngine;
using UnityEngine.EventSystems;

public sealed class MailDestination : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private string destinationId;

    [SerializeField]
    private RectTransform snapPoint;

    public string DestinationId => destinationId;

    public RectTransform SnapPoint
    {
        get
        {
            if (snapPoint != null)
                return snapPoint;

            return transform as RectTransform;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DraggableMailItem mailItem =
            eventData.pointerDrag.GetComponent<DraggableMailItem>();

        if (mailItem == null)
            return;

        bool wasAccepted =
            mailItem.TryPlaceAtDestination(this);

        if (wasAccepted)
        {
            Debug.Log(
                $"{mailItem.MailItemId} sorted into {destinationId}.",
                this
            );
        }
        else
        {
            Debug.LogWarning(
                $"{mailItem.MailItemId} does not belong in {destinationId}.",
                this
            );
        }
    }
}