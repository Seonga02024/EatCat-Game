using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CS_moveFood : MonoBehaviour, IPunObservable
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
    [SerializeField] private Vector2 targetPosition; 
    public Vector2 TargetPosition { set { targetPosition = value; } }
    private Vector3 networkedPosition;
    private PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        //this.transform.parent = GameObject.Find("FoodStartPoint").transform;
        transform.SetParent(GameObject.Find("FoodStartPoint").transform, true); // SetParent 사용
        endPoint = GameObject.Find("FoodEndPoint").transform;
        isMove = true;
        if(CS_MTGameManager.Instance.IsMTGameStart) ChangeMoveSpeed(CS_MTGameManager.Instance.CurrentFoodMoveSpeed);
        else if(CS_SoloGameManager.Instance.IsSoloGameStart) ChangeMoveSpeed(CS_SoloGameManager.Instance.CurrentFoodMoveSpeed);
    }

    void Update()
    {
        if(CS_MTGameManager.Instance.IsMTGameStart || CS_SoloGameManager.Instance.IsSoloGameStart){
            // isMove가 true일 때 y축 -방향으로 이동
            if (isMove && isEat == false)
            {
                if(CS_MTGameManager.Instance.IsMTGameStart){
                    if (CS_MTGameManager.Instance.IsHost)
                    {   
                        transform.Translate(Vector3.down * speed * Time.deltaTime);
                    }else{
                        transform.localPosition = Vector3.Lerp(transform.localPosition, networkedPosition, Time.deltaTime * speed);
                    }
                }
                else if(CS_SoloGameManager.Instance.IsSoloGameStart){
                    transform.Translate(Vector3.down * speed * Time.deltaTime);
                }
            }

            if(isEat){
                // 현재 위치에서 목표 위치로의 방향을 계산합니다.
                Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                    
                // 방향과 속도를 사용하여 새로운 위치를 계산합니다.
                Vector2 newPosition = (Vector2)transform.position + direction * speed * Time.deltaTime;
                    
                // 오브젝트를 새로운 위치로 이동시킵니다.
                transform.position = newPosition;

                // 만약 오브젝트가 목표 위치에 도달했다면 이동을 멈춥니다.
                if (Vector2.Distance(transform.position, targetPosition) < 1.5f)
                {
                    RemoveObject();
                }
            }

            // endPoint보다 밑에 있을 때 객체 제거
            if (endPoint != null && transform.position.y < endPoint.position.y)
            {
                RemoveObject();
            }
        }
    }

    public void ChangeIsEating(bool isActive){
        isEat = isActive;
        if(isEat){
            if(CS_MTGameManager.Instance.IsMTGameStart){
                Invoke("RemoveObject", 1.3f);
            }

            if(CS_SoloGameManager.Instance.IsSoloGameStart){
                Invoke("RemoveObject", 2f);
            }
        }
    }

    public void RemoveObject(){
        if (CS_MTGameManager.Instance.IsHost){
            PhotonNetwork.Destroy(gameObject);
        }
        
        if(CS_SoloGameManager.Instance.IsSoloGameStart){
            Destroy(gameObject);
        }
    }

    public void ChangeMoveSpeed(int changeSpeed){
        speed = changeSpeed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (CS_MTGameManager.Instance.IsHost){
                //Debug.Log("OnPhotonSerializeView");
                // 로컬 플레이어의 위치와 회전을 네트워크로 보냅니다.
                stream.SendNext(transform.localPosition);
                //Debug.Log($"Sending Position: {transform.position}");
            }
        }
        else
        {
            // 원격 플레이어의 위치와 회전을 네트워크로부터 받습니다.
            networkedPosition = (Vector3)stream.ReceiveNext();
            // Debug.Log("OnPhotonSerializeView");
            // Debug.Log($"Received Position: {networkedPosition}");
        }
    }

    private void OnDestroy()
    {
        if(CS_MTGameManager.Instance.IsMTGameStart){
            CS_MTGameManager.Instance.RemoveFood(gameObject);
        }

        if(CS_SoloGameManager.Instance.IsSoloGameStart){
            CS_SoloGameManager.Instance.RemoveFood(gameObject);
        }
    }
}
