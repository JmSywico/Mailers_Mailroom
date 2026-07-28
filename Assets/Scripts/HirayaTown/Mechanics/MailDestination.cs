using UnityEngine;
using UnityEngine.EventSystems;

public sealed class MailDestination :
    MonoBehaviour,
    IDropHandler
{
    [SerializeField]
    private string destinationId;

    [SerializeField]
    private RectTransform placementArea;

    public string DestinationId =>
        destinationId;

    public RectTransform PlacementArea
    {
        get
        {
            if (placementArea != null)
                return placementArea;

            return transform as RectTransform;
        }
    }

    public void OnDrop(
        PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DraggableMailItem mailItem =
            eventData.pointerDrag
                .GetComponent<DraggableMailItem>();

        if (mailItem == null)
            return;

        bool wasAccepted =
            mailItem.TryPlaceAtDestination(
                this
            );

        if (wasAccepted)
        {
            Debug.Log(
                $"{mailItem.MailItemId} sorted into " +
                $"{destinationId}.",
                this
            );
        }
        else
        {
            MailDeliveryAttemptTracker attemptTracker =
                eventData.pointerDrag
                    .GetComponent<MailDeliveryAttemptTracker>();

            if (attemptTracker != null)
                attemptTracker.RecordWrongAttempt();

            Debug.LogWarning(
                $"{mailItem.MailItemId} does not belong " +
                $"in {destinationId}.",
                this
            );
        }
    }
}
