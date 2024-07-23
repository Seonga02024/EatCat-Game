using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_moveFood : MonoBehaviour
{
    public bool isMove = false; // 이동 여부를 결정하는 변수
    private float speed = 60f; // 이동 속도
    private int score = 10;
    public Transform endPoint;

    void Update()
    {
        // isMove가 true일 때 y축 -방향으로 이동
        if (isMove)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        // endPoint보다 밑에 있을 때 객체 제거
        if (endPoint != null && transform.position.y < endPoint.position.y)
        {
            Destroy(gameObject);
        }
    }
}
