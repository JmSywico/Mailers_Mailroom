using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MobileOrientationPreference
{
    Auto,
    Portrait,
    Landscape
}

public sealed class MobileOrientationController : MonoBehaviour
{
    [SerializeField]
    private MobileOrientationPreference startingPreference =
        MobileOrientationPreference.Auto;

    [SerializeField]
    private Button autoButton;

    [SerializeField]
    private Button portraitButton;

    [SerializeField]
    private Button landscapeButton;

    [SerializeField]
    private TMP_Text currentPreferenceText;

    private MobileOrientationPreference currentPreference;

    private void Awake()
    {
        ApplyPreference(
            startingPreference
        );
    }

    private void OnEnable()
    {
        if (autoButton != null)
            autoButton.onClick.AddListener(UseAuto);

        if (portraitButton != null)
            portraitButton.onClick.AddListener(UsePortrait);

        if (landscapeButton != null)
            landscapeButton.onClick.AddListener(UseLandscape);

        RefreshControls();
    }

    private void OnDisable()
    {
        if (autoButton != null)
            autoButton.onClick.RemoveListener(UseAuto);

        if (portraitButton != null)
            portraitButton.onClick.RemoveListener(UsePortrait);

        if (landscapeButton != null)
            landscapeButton.onClick.RemoveListener(UseLandscape);
    }

    public void UseAuto()
    {
        ApplyPreference(
            MobileOrientationPreference.Auto
        );
    }

    public void UsePortrait()
    {
        ApplyPreference(
            MobileOrientationPreference.Portrait
        );
    }

    public void UseLandscape()
    {
        ApplyPreference(
            MobileOrientationPreference.Landscape
        );
    }

    private void ApplyPreference(
        MobileOrientationPreference preference)
    {
        currentPreference =
            preference;

        switch (currentPreference)
        {
            case MobileOrientationPreference.Portrait:
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
                Screen.orientation = ScreenOrientation.Portrait;
                break;

            case MobileOrientationPreference.Landscape:
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;

            default:
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.orientation = ScreenOrientation.AutoRotation;
                break;
        }

        RefreshControls();
    }

    private void RefreshControls()
    {
        if (autoButton != null)
        {
            autoButton.interactable =
                currentPreference != MobileOrientationPreference.Auto;
        }

        if (portraitButton != null)
        {
            portraitButton.interactable =
                currentPreference != MobileOrientationPreference.Portrait;
        }

        if (landscapeButton != null)
        {
            landscapeButton.interactable =
                currentPreference != MobileOrientationPreference.Landscape;
        }

        if (currentPreferenceText != null)
        {
            currentPreferenceText.text =
                currentPreference.ToString().ToUpperInvariant();
        }
    }
}
