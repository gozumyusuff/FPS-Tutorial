using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun.UtilityScripts;

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

    [Range(0, 1)]
    public float recoverPercent = 0.7f;

    public float recoilUp = 1f;

    public float recoilBack = 0f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;


    private bool recoiling;
    private bool recovering;



    private void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
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

        if (recoiling)
        {
            Recoil();
        }

        if (recovering)
        {
            Recovering();
        }
    }

    void Reload()
    {

        if (mag > 0 && ammo < 30)
        {
            animation.Play(reload.name);
            mag--;

            ammo = magAmmo;
        }
            
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Fire()
    {
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        
        RaycastHit hit;

        PhotonNetwork.LocalPlayer.AddScore(1);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f ))
        {
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                PhotonNetwork.LocalPlayer.AddScore(damage);

                if (damage > hit.transform.gameObject.GetComponent<Health>().health)
                {
                    PhotonNetwork.LocalPlayer.AddScore(100);
                }

                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
        
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }

    void Recovering()
    {
        Vector3 finalPosition = originalPosition;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
}
