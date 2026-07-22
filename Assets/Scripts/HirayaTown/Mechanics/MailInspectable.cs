using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DraggableMailItem))]
public sealed class MailInspectable :
    MonoBehaviour,
    IPointerClickHandler
{
    [Header("Inspection")]
    [SerializeField]
    private MailInspectionController inspectionController;

    [Header("Stamping")]
    [SerializeField]
    private StampSelectionController stampSelectionController;

    [Header("Inspection Information")]
    [SerializeField]
    private string mailType = "LETTER";

    [SerializeField]
    private string recipient;

    [TextArea(2, 4)]
    [SerializeField]
    private string address;

    [TextArea(2, 4)]
    [SerializeField]
    private string details;

    private DraggableMailItem draggableMailItem;
    private StampableMailItem stampableMailItem;

    private void Awake()
    {
        draggableMailItem =
            GetComponent<DraggableMailItem>();

        stampableMailItem =
            GetComponent<StampableMailItem>();
    }

    public void OnPointerClick(
        PointerEventData eventData)
    {
        if (eventData.button !=
            PointerEventData.InputButton.Left)
        {
            return;
        }

        if (draggableMailItem != null &&
            draggableMailItem.IsSorted)
        {
            return;
        }

        if (stampSelectionController != null &&
            stampSelectionController
                .HasSelectedStamp &&
            stampableMailItem != null)
        {
            bool receivedStamp =
                stampSelectionController
                    .TryConsumeSelectedStamp(
                        out StampTheme stampTheme
                    );

            if (receivedStamp)
            {
                stampableMailItem.ApplyStamp(
                    stampTheme
                );
            }

            return;
        }

        if (inspectionController == null)
        {
            Debug.LogError(
                $"{name} has no Inspection Controller assigned.",
                this
            );

            return;
        }

        inspectionController.ShowMail(
            mailType,
            recipient,
            address,
            details
        );
    }
}