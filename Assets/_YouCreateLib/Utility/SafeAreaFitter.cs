using UnityEngine;

[ExecuteAlways]
public class SafeAreaFitter : MonoBehaviour
{
    [SerializeField] private RectTransform safeAreaContent;

    private Rect lastSafeArea;

    private void Update()
    {
        if (safeAreaContent == null)
        {
            return;
        }

        var safeArea = Screen.safeArea;

#if UNITY_EDITOR
        if (Screen.width == 1125 && Screen.height == 2436)
        {
            safeArea.y = 102;
            safeArea.height = 2202;
        }

        if (Screen.width == 2436 && Screen.height == 1125)
        {
            safeArea.x = 132;
            safeArea.y = 63;
            safeArea.height = 1062;
            safeArea.width = 2172;
        }
#endif

        if (safeArea == lastSafeArea)
        {
            return;
        }

        ApplySafeArea(safeArea);

        lastSafeArea = safeArea;
    }

    private void ApplySafeArea(Rect safeArea)
    {
        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        safeAreaContent.anchorMin = anchorMin;
        safeAreaContent.anchorMax = anchorMax;
    }
}
