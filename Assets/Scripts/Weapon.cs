using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public Camera camera;
    public float fireRate;

    public GameObject hitVFX;

    private float nextfire;
    
    void Update()
    {
        if (nextfire > 0)
        {
            nextfire -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && nextfire <= 0)
        {
            nextfire = 1 / fireRate;

            Fire();
        }
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
