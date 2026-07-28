using UnityEngine;

public sealed class MobileRuntimeSettings : MonoBehaviour
{
    [SerializeField]
    private int targetFrameRate = 60;

    [SerializeField]
    private bool preventDeviceSleep = true;

    [SerializeField]
    private bool useSingleTouchInput = true;

    private void Awake()
    {
        Apply();
    }

    private void OnEnable()
    {
        Apply();
    }

    private void OnDisable()
    {
        if (preventDeviceSleep)
        {
            Screen.sleepTimeout =
                SleepTimeout.SystemSetting;
        }

        if (useSingleTouchInput)
        {
            Input.multiTouchEnabled =
                true;
        }
    }

    private void Apply()
    {
        Application.targetFrameRate =
            Mathf.Max(30, targetFrameRate);

        if (preventDeviceSleep)
        {
            Screen.sleepTimeout =
                SleepTimeout.NeverSleep;
        }

        if (useSingleTouchInput)
        {
            Input.multiTouchEnabled =
                false;
        }
    }
}
