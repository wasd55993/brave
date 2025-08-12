using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pun2CallBack : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Hello World");
        //加入或创建房间
        PhotonNetwork.JoinOrCreateRoom("Room",new Photon.Realtime.RoomOptions() { MaxPlayers = 4},default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.Instantiate("Player",new Vector3(0,0,0),Quaternion.identity,0);
    }
}
