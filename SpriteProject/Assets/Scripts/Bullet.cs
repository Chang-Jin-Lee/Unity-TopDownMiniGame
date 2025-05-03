using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour, IBulletAbility
{
    public Vector3 originalPos;
    public Vector3 destinationPos;
    public Vector3 direction;
    public float damage = 5.0f;
    public float speed = 10.0f;
    public float range = 300.0f;
    private bool bMovable = false;

    public float Damage => damage;
    public float Speed => speed;
    public float Range => range;

    public void SetBulletAbility(WeaponAbiliyData weaponAbiliyData)
    {
        damage = weaponAbiliyData.damage;
        speed = weaponAbiliyData.speed;
        range = weaponAbiliyData.range;
    }

    public void SetBulletDirection(Vector3 _originPos, Vector3 _desPos)
    {
        originalPos = _originPos;
        destinationPos = _desPos;
        direction = (destinationPos - originalPos).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        bMovable = true;
    }

    //private void Awake()
    //{
    //    direction = transform.forward;
    //}

    void FixedUpdate()
    {
        Debug.DrawLine(originalPos, originalPos + direction * 5f, Color.blue);
        if (bMovable)
        {
            if ((transform.position - originalPos).magnitude < range)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
    private void OnCollisionEnter(Collision other)
    {
        IPlayerAbility playerAbility = ComponentHelper.FindInterface<IPlayerAbility>(other.gameObject);
        if (playerAbility != null)
        {
            playerAbility.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.transform.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
