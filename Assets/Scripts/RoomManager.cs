using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public GameObject player;
    [Space]
    public Transform spawnPoint;

    void Start()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        Debug.Log("we are in the lobby");

        PhotonNetwork.JoinOrCreateRoom("test", null, null);
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("we are connected and in a room");

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        if (_player.GetComponent<PhotonView>().IsMine)
        {
            _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        }
        
    }

}

