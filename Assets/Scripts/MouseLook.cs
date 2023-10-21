using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f; // Fare hassasiyeti
    public float smoothing = 2.0f; // Fare düzeltme

    private Vector2 mouseLook;
    private Vector2 smoothV;

    private GameObject player;

    void Start()
    {
        player = this.transform.parent.gameObject;
    }

    void Update()
    {
        // Fare girişini al
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Fare girişini yumuşatma
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
        mouseLook += smoothV;

        // Minimum ve maksimum açı sınırlamaları eklemek isterseniz burada yapabilirsiniz

        // Yatay (sol-sağ) dönme
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

        // Dikey (yukarı-aşağı) dönme
        player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }
}
