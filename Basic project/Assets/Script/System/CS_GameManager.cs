using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GameManager : MonoBehaviour
{
    [Header("food Prefab")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject mtGamePanel;
    [SerializeField] private GameObject soloGamePanel;

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
