using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    public bool isLocalPlayer;

    public TextMeshProUGUI healthText;

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;

        healthText.text = health.ToString();

        if (health <= 0)
        {
            if (isLocalPlayer)
            {
            RoomManager.instance.SpawnPlayer();
            }

            Destroy(gameObject);
            Debug.Log("dead");
        }
    }
}
