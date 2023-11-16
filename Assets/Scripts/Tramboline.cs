using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tramboline : MonoBehaviour
{
    public float FlyForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 FlyDirection = transform.up * FlyForce;
                rb.AddForce(FlyDirection, ForceMode.Impulse);
            }

        }

        
    }
}
