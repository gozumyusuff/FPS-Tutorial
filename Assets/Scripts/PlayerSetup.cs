using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public NewMovement movement;

    public GameObject camera;

    public string nickname;

    public TextMeshPro nicknameText;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        camera.SetActive(true);
    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        nickname = _name;

        nicknameText.text = nickname;
    }


}
