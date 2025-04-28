using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IAbility
{
    [SerializeField] public AbilityData abilityTemplate;
    
    // About Ability
    public float health = 100.0f;
    public float moveSpeed = 5.0f;
    public float moveWalkSpeed = 5.0f;
    public float moveDashSpeed = 10.0f;
    
    public float Health => health;
    public float MoveWalkSpeed => moveWalkSpeed;
    public float MoveDashSpeed => moveDashSpeed;

    public void Death()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        print(health);
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
    }
    
    public void SetAbilities()
    {
        moveWalkSpeed = abilityTemplate.moveWalkSpeed;
        moveDashSpeed = abilityTemplate.moveDashSpeed;
        health = abilityTemplate.health;
    }

    void Start()
    {
        SetAbilities();
    }

}
