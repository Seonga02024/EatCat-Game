using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class CS_Player : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private PhotonView PV;
    [SerializeField] private TextMeshProUGUI nickNameTxt;

    // [Header("Game Object")]
    private GameObject RespawnPos;
    private GameObject RespawnPos1;
    private GameObject RespawnPos2;

    public void Awake(){
        // 자신이면 PhotonNetwork.NickName
        // 다른 사람이면 PV.Owner.NickName 넣음
        RespawnPos = GameObject.Find("RespawnPos");
        RespawnPos1 = GameObject.Find("RespawnPos1");
        RespawnPos2 = GameObject.Find("RespawnPos2");
        nickNameTxt.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        this.transform.parent = RespawnPos.transform;
        this.transform.position = PV.IsMine ? RespawnPos1.transform.position : RespawnPos2.transform.position;
    }

    // 변수 동기화 진행
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
