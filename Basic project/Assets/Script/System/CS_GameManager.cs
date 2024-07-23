using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GameManager : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] private Transform foodStartPoint;
    [SerializeField] private Transform foodEndPoint;

    private Queue<GameObject> foodQueue = new Queue<GameObject>();
    private int initialQueueSize = 10;
    private float interval = 4.0f;

    [Header("food Prefab")]
    [SerializeField] private GameObject foodPrefab1;
    [SerializeField] private GameObject foodPrefab2;

    void Start()
    {
        // 초기 큐 채우기
        for (int i = 0; i < initialQueueSize; i++)
        {
            GameObject food = Instantiate(foodPrefab1, foodStartPoint);
            food.SetActive(false);
            foodQueue.Enqueue(food);
        }

        // 4초마다 반복 실행
        StartCoroutine(ReleaseAndAddFood());
    }

    IEnumerator ReleaseAndAddFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // 큐에서 하나 꺼내서 삭제
            if (foodQueue.Count > 0)
            {
                GameObject foodToRelease = foodQueue.Dequeue();
                foodToRelease.SetActive(true);
                foodToRelease.GetComponent<CS_moveFood>().isMove = true;
                foodToRelease.GetComponent<CS_moveFood>().endPoint = foodEndPoint;
                // 여기서 꺼낸 객체를 사용하거나, 삭제하거나 원하는 작업 수행
                // Destroy(foodToRelease);
            }

            // 새 객체 생성하여 큐에 추가
            GameObject newFood = Instantiate(foodPrefab2, foodStartPoint);
            newFood.SetActive(false);
            foodQueue.Enqueue(newFood);
        }
    }
}
