using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TouchPanel : MonoBehaviour
{
    private bool isEating = false;
    public bool IsEating { set { isEating = value; }}

    private void Update()
    {
        if(CS_MTGameManager.Instance.IsMTGameStart || CS_SoloGameManager.Instance.IsSoloGameStart){
            if(isEating == false){
                CheckTouch();
            }
        }
    }

    private void CheckTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = touch.position;
                if (touchPosition.x < Screen.width / 2)
                {
                    Debug.Log("Left Touch");
                    SendTouchData(true);
                }
                else
                {
                    Debug.Log("Right Touch");
                    SendTouchData(false);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            if (mousePosition.x < Screen.width / 2)
            {
                Debug.Log("Left Touch");
                SendTouchData(true);
            }
            else
            {
                Debug.Log("Right Touch");
                SendTouchData(false);
            }
        }
    }

    private void SendTouchData(bool isLeft){
        if(CS_MTGameManager.Instance.IsMTGameStart){
            CS_MTGameManager.Instance.CheckEatCorrrectFood();
        }

        if(CS_SoloGameManager.Instance.IsSoloGameStart){
            CS_SoloGameManager.Instance.CheckEatCorrrectFood(isLeft);
        }

        isEating = true;
        Invoke("WaitEating", 2);
    }

    private void WaitEating(){
        isEating = false;
    }
}
