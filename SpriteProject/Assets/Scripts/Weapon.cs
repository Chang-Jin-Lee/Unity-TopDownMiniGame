using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player pl = other.GetComponent<Player>();
        if(pl != null)
        {
            pl.EquipWeapon(gameObject);
            Destroy(gameObject);
        }
    }
}
