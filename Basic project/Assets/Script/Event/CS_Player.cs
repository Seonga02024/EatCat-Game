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
    [SerializeField] private Image image;
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
        if(PV.IsMine == false){
            Vector3 scale = image.rectTransform.localScale;
            scale.x = -1; // Flip the X axis
            image.rectTransform.localScale = scale;

            Vector3 textScale = nickNameTxt.rectTransform.localScale;
            textScale.x = -1; // Flip the X axis for the text to keep it normal
            nickNameTxt.rectTransform.localScale = textScale;
        }else{
            Vector3 scale = image.rectTransform.localScale;
            scale.x = 1; // Flip the X axis
            image.rectTransform.localScale = scale;

            Vector3 textScale = nickNameTxt.rectTransform.localScale;
            textScale.x *= 1; // Flip the X axis for the text to keep it normal
            nickNameTxt.rectTransform.localScale = textScale;

            CS_MTGameManager.Instance.MyPlayerCS = this;
        }
    }

    // 변수 동기화 진행
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        //throw new System.NotImplementedException();
    }

    public void playEatAnimation(){
        Debug.Log("playEatAnimation");
        animator.SetBool("isEat", true);
        Invoke("StopAnimation", 2);
    }

    private void StopAnimation(){
        animator.SetBool("isEat", false);
    }
}
