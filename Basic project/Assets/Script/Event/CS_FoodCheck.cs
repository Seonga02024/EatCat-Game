using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CS_FoodCheck : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform myCatPos;
    [SerializeField] private Transform otherCatPas;
    private GameObject currentCollFood;
    private int currentFoodIndex = -1;
    public int CurrentFoodIndex { get { return currentFoodIndex; } }
    private bool iscurrentGet = false;
    public bool IscurrentGet { get { return iscurrentGet; } }

    public void MoveFoodToCat(bool isMyEat){
        //Debug.Log("MoveFoodToCat");
        if(currentCollFood != null){
            //Debug.Log("change MoveFoodToCat");
            CS_moveFood moveFoodCS = currentCollFood.GetComponent<CS_moveFood>();
            moveFoodCS.ChangeIsEating(true);
            if(isMyEat) moveFoodCS.TargetPosition = new Vector2(myCatPos.position.x, myCatPos.position.y);
            else moveFoodCS.TargetPosition = new Vector2(otherCatPas.position.x, otherCatPas.position.y);
            iscurrentGet = true;
        }
    }

    // 트리거 진입 시 호출되는 메서드
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger Enter: " + other.gameObject.name);
        currentCollFood = other.gameObject;
        currentFoodIndex = currentCollFood.GetComponent<CS_moveFood>().Index;
        iscurrentGet = false;
    }

    // 트리거 내부에 있는 동안 호출되는 메서드
    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Trigger Stay: " + other.gameObject.name);
        if(currentCollFood == null) {
            currentCollFood = other.gameObject;
            currentFoodIndex = currentCollFood.GetComponent<CS_moveFood>().Index;
        }
    }

    // 트리거에서 나갈 때 호출되는 메서드
    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Trigger Exit: " + other.gameObject.name);
        currentCollFood = null;
        currentFoodIndex = -1;
    }
}
