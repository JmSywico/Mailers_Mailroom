using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public sealed class MobileSafeAreaFitter : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea;
    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        CacheRectTransform();
        ApplySafeArea();
    }

    private void OnEnable()
    {
        CacheRectTransform();
        ApplySafeArea();
    }

    private void Update()
    {
        if (Screen.width == lastScreenWidth &&
            Screen.height == lastScreenHeight &&
            Screen.safeArea == lastSafeArea)
        {
            return;
        }

        ApplySafeArea();
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (!isActiveAndEnabled)
            return;

        CacheRectTransform();

        if (rectTransform == null ||
            Screen.width <= 0 ||
            Screen.height <= 0)
        {
            return;
        }

        Rect safeArea =
            Screen.safeArea;

        Vector2 anchorMin =
            safeArea.position;

        Vector2 anchorMax =
            safeArea.position + safeArea.size;

        anchorMin.x /=
            Screen.width;

        anchorMin.y /=
            Screen.height;

        anchorMax.x /=
            Screen.width;

        anchorMax.y /=
            Screen.height;

        rectTransform.anchorMin =
            anchorMin;

        rectTransform.anchorMax =
            anchorMax;

        rectTransform.offsetMin =
            Vector2.zero;

        rectTransform.offsetMax =
            Vector2.zero;

        lastSafeArea =
            safeArea;

        lastScreenWidth =
            Screen.width;

        lastScreenHeight =
            Screen.height;
    }

    private void CacheRectTransform()
    {
        if (rectTransform == null)
        {
            rectTransform =
                GetComponent<RectTransform>();
        }
    }
}
