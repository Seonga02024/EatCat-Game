using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_SafeArea : MonoBehaviour
{
    public GameObject safeAreaPanel;

    private void Start()
    {
        // SafeArea 설정
        Rect safeArea = Screen.safeArea;
        Vector2 newAnchorMin = safeArea.position;
        Vector2 newAnchorMax = safeArea.position + safeArea.size;
        newAnchorMin.x /= Screen.width;
        newAnchorMax.x /= Screen.width;
        newAnchorMin.y /= Screen.height;
        newAnchorMax.y /= Screen.height;

        RectTransform rect = safeAreaPanel.GetComponent<RectTransform>();
        rect.anchorMin = newAnchorMin;
        rect.anchorMax = newAnchorMax;
    }
}
