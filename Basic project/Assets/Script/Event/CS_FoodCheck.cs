using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_FoodCheck : MonoBehaviour
{
    // 트리거 진입 시 호출되는 메서드
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter: " + other.gameObject.name);
    }

    // 트리거 내부에 있는 동안 호출되는 메서드
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Trigger Stay: " + other.gameObject.name);
    }

    // 트리거에서 나갈 때 호출되는 메서드
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger Exit: " + other.gameObject.name);
    }
}
