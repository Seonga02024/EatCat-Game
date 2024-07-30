using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CS_FoodCheck : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView PV;
    private GameObject currentCollFood;
    private int currentFoodIndex = -1;
    public int CurrentFoodIndex { get { return currentFoodIndex; } }

    public void MoveFoodToCat(bool isMyEat){
        if(currentCollFood != null){
            CS_moveFood moveFoodCS = currentCollFood.GetComponent<CS_moveFood>();
            moveFoodCS.IsEat = true;
            moveFoodCS.IsMyEat = isMyEat;
            // if(currentFoodIndex == CS_GameManager.Instance.GetCurrentCorrectFoodIndex()){
            //     PV.RPC("GetScore", RpcTarget.All, 2);
            // }
        }
    }

    // 트리거 진입 시 호출되는 메서드
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger Enter: " + other.gameObject.name);
        currentCollFood = other.gameObject;
        currentFoodIndex = currentCollFood.GetComponent<CS_moveFood>().Index;
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
