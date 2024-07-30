using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_moveFood : MonoBehaviour
{
    [SerializeField] private int index = 0;
    public int Index { get { return index; } }
    [SerializeField] private bool isMove = false; // 이동 여부를 결정하는 변수
    public bool IsMove { get { return isMove; } set { isMove = value; } }
    private float speed = 100f; // 이동 속도
    private int score = 10;
    [SerializeField] private Transform endPoint;
    public Transform EndPoint { get { return endPoint; } set { endPoint = value; } }
    [SerializeField] private bool isEat = false; 
    public bool IsEat { set { isEat = value; } }
    [SerializeField] private bool isMyEat = false; 
    public bool IsMyEat { set { isMyEat = value; } }

    void Update()
    {
        // isMove가 true일 때 y축 -방향으로 이동
        if (isMove && isEat == false)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        if(isEat){
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        // endPoint보다 밑에 있을 때 객체 제거
        if (endPoint != null && transform.position.y < endPoint.position.y)
        {
            Destroy(gameObject);
        }
    }
}
