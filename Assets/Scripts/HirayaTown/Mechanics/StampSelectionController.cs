using TMPro;
using UnityEngine;

public sealed class StampSelectionController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TMP_Text selectedStampText;

    private bool hasSelectedStamp;
    private StampTheme selectedStampTheme;

    public bool HasSelectedStamp =>
        hasSelectedStamp;

    public StampTheme SelectedStampTheme =>
        selectedStampTheme;

    private void Start()
    {
        ClearSelection();
    }

    public void SelectStamp(int stampThemeIndex)
    {
        if (!System.Enum.IsDefined(
                typeof(StampTheme),
                stampThemeIndex))
        {
            Debug.LogError(
                $"Invalid stamp theme index: {stampThemeIndex}",
                this
            );

            return;
        }

        selectedStampTheme =
            (StampTheme)stampThemeIndex;

        hasSelectedStamp = true;

        UpdateSelectedStampText();
    }

    public bool TryConsumeSelectedStamp(
        out StampTheme stampTheme)
    {
        stampTheme = selectedStampTheme;

        if (!hasSelectedStamp)
            return false;

        ClearSelection();

        return true;
    }

    public void ClearSelection()
    {
        hasSelectedStamp = false;

        if (selectedStampText != null)
        {
            selectedStampText.text =
                "Selected Stamp: None";
        }
    }

    private void UpdateSelectedStampText()
    {
        if (selectedStampText == null)
            return;

        selectedStampText.text =
            $"Selected Stamp: " +
            FormatStampName(selectedStampTheme);
    }

    private string FormatStampName(
        StampTheme stampTheme)
    {
        return stampTheme switch
        {
            StampTheme.PopCulture =>
                "Pop Culture",

            StampTheme.CozyHome =>
                "Cozy Home",

            _ =>
                stampTheme.ToString()
        };
    }
}