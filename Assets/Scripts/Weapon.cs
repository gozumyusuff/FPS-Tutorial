using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public int damage;
    public Camera camera;
    public float fireRate;

    public GameObject hitVFX;

    private float nextfire;

    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;

    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    public Animation animation;
    public AnimationClip reload;

    private void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Update()
    {
        if (nextfire > 0)
        {
            nextfire -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && nextfire <= 0 && ammo > 0 && animation.isPlaying == false)
        {
            nextfire = 1 / fireRate;

            ammo--;

            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;

            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            Debug.Log("reloaded");
        }
    }

    void Reload()
    {
        animation.Play(reload.name);

        if (mag > 0 && ammo < 30)
        {
            mag--;

            ammo = magAmmo;
        }
            
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Fire()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f ))
        {
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
        Debug.Log("fire");
    }
}
