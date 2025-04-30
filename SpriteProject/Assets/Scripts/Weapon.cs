using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeaponAbility
{
    private Rigidbody rb;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private WeaponAbiliyData weaponAbiliyData;

    public float fireRate = 1.0f;

    public Vector3 GetSpawnPoint => bulletSpawnPoint.transform.position;

    void Start()
    {
        fireRate = weaponAbiliyData.fireRate;
        bulletPrefab.SetBulletAbility(weaponAbiliyData);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player pl = other.GetComponent<Player>();
        if (pl != null)
        {
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(DelayedEquip(pl));
        }
    }

    private IEnumerator DelayedEquip(Player pl)
    {
        yield return null; // 1프레임 기다리기
        pl.EquipWeapon(gameObject);
        Destroy(gameObject);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position - transform.up * 300, Color.green);
    }

    public void Shoot(Vector3 desPos)
    {
        Debug.DrawLine(bulletSpawnPoint.transform.position, desPos, Color.green);

        Vector3 dir = (desPos - bulletSpawnPoint.transform.position).normalized;
        Quaternion correction = Quaternion.FromToRotation(-transform.up, dir);
        transform.rotation = correction * transform.rotation;

        Bullet spawnedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        spawnedBullet.SetBulletDirection(bulletSpawnPoint.transform.position, desPos);
    }

}
