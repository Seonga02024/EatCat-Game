using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CS_ButtonEffect : MonoBehaviour
{
    [Header("Default")]
    [SerializeField] private bool onClickChangeScale = true;
    [SerializeField] private bool onClickChangeColor = true;
    [SerializeField] private Image[] btnImages;
    [SerializeField] private Button button;


    [Header("Custom Color")]
    [SerializeField] private bool isUseCustomColor = false;
    [SerializeField] private Color customColor = new Color(200 / 255f, 200 / 255f, 200 / 255f, 255 / 255f);
    

    /* Const Values */
    private readonly Color NORMAL_COLOR = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    private readonly Color PRESSED_COLOR = new Color(229 / 255f, 51 / 255f, 137 / 255f, 255 / 255f);
    private readonly Vector3 NORMAL_SCALE = Vector3.one;
    private readonly Vector3 PRESSED_SCALE = new Vector3(1.15f, 1.15f, 1.15f);

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonDown();
    }

    private void OnButtonDown()
    {
        if (onClickChangeColor)
        {
            if (!isUseCustomColor)
            {
                foreach (var btnImage in btnImages)
                {
                    btnImage.color = PRESSED_COLOR;
                }
            }
            else
            {
                foreach (var btnImage in btnImages)
                {
                    btnImage.color = customColor;
                }
            }
        }

        if (onClickChangeScale)
        {
            foreach (var btnImage in btnImages)
            {
                btnImage.rectTransform.localScale = PRESSED_SCALE;
            }
        }
    }

    private void OnButtonUp()
    {
        if (onClickChangeColor)
        {
            foreach (var btnImage in btnImages)
            {
                btnImage.color = NORMAL_COLOR;
            }
        }

        if (onClickChangeScale)
        {
            foreach (var btnImage in btnImages)
            {
                btnImage.rectTransform.localScale = NORMAL_SCALE;
            }
        }
    }
}
