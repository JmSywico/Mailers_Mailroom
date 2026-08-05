using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public sealed class MobileResponsiveCanvasScaler : MonoBehaviour
{
    [Tooltip("The mailroom is authored as a portrait layout. Landscape keeps this reference and fits the full layout on screen.")]
    [SerializeField]
    private Vector2 portraitReferenceResolution =
        new Vector2(1080.0f, 1920.0f);

    private CanvasScaler canvasScaler;
    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        CacheCanvasScaler();
        ApplyCurrentOrientation();
    }

    private void OnEnable()
    {
        CacheCanvasScaler();
        ApplyCurrentOrientation();
    }

    private void Update()
    {
        if (Screen.width == lastScreenWidth &&
            Screen.height == lastScreenHeight)
        {
            return;
        }

        ApplyCurrentOrientation();
    }

    private void ApplyCurrentOrientation()
    {
        CacheCanvasScaler();

        if (canvasScaler == null ||
            Screen.width <= 0 ||
            Screen.height <= 0)
        {
            return;
        }

        canvasScaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        canvasScaler.referenceResolution =
            portraitReferenceResolution;

        canvasScaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.Expand;

        lastScreenWidth =
            Screen.width;

        lastScreenHeight =
            Screen.height;
    }

    private void CacheCanvasScaler()
    {
        if (canvasScaler == null)
        {
            canvasScaler =
                GetComponent<CanvasScaler>();
        }
    }
}
