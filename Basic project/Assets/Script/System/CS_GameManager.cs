using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CS_GameManager : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] private Transform foodStartPoint;
    [SerializeField] private Transform foodEndPoint;

    private Queue<GameObject> foodQueue = new Queue<GameObject>();
    private int initialQueueSize = 10;
    private float interval = 4.0f;
    private int currentCorrectFoodIndex = 0;
    public int GetCurrentCorrectFoodIndex() { return currentCorrectFoodIndex; }

    [Header("food Prefab")]
    [SerializeField] private GameObject foodPrefab1;
    [SerializeField] private GameObject foodPrefab2;
    private bool isHost = false;
    private bool isGameStart = false;
    public bool IsGameStart { get { return isGameStart; } }


    private void StartGame()
    {
        // 게임 시작 로직을 여기에 추가하세요.
        // 예를 들어, 게임 씬으로 전환하거나 게임 오브젝트를 활성화합니다.
        Debug.Log("Game started!");
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
                // 여기서 꺼낸 객체를 사용하거나, 삭제하거나 원하는 작업 수행
                // Destroy(foodToRelease);
                GameObject foodToRelease = foodQueue.Dequeue();
                CS_moveFood foodToReleaseCS = foodToRelease.GetComponent<CS_moveFood>();
                CS_NetworkManager.Instance.testText.text = "index";
                foodToRelease.SetActive(true);
                foodToReleaseCS.IsMove = true;
                foodToReleaseCS.EndPoint = foodEndPoint;
                
            }

            // 새 객체 생성하여 큐에 추가
            GameObject newFood = Instantiate(foodPrefab2, foodStartPoint);
            newFood.SetActive(false);
            foodQueue.Enqueue(newFood);
        }
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// singleton

    private static CS_GameManager _instance;

    public static CS_GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CS_GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<CS_GameManager>();
                    singletonObject.name = typeof(CS_GameManager).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as CS_GameManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// singleton
}
