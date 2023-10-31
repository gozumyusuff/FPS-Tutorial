using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public static RoomManager instance;

    public GameObject player;
    public Transform[] spawnPoints;
    public GameObject roomCam;

    public GameObject nameUI;
    public GameObject connectingUI;

    private string nickname = "unnamed";

    private void Awake()
    {
        instance = this;
    }

    public void ChangeNickname(string _name)
    {
        nickname = _name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
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

        roomCam.SetActive(false);

        SpawnPlayer();

        Cursor.lockState = CursorLockMode.Locked;

    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        if (_player.GetComponent<PhotonView>().IsMine)
        {
            _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        }

        _player.GetComponent<Health>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;
        
    }

}

