using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IWeaponAbility
{
    void Shoot(Vector3 desPos); // 도착 위치를 전달해야함
}

public interface IBulletAbility
{
    float Damage { get; }
    float Speed { get; }
    float Range { get; }

    void SetBulletAbility(WeaponAbiliyData weaponAbiliyData);
    void SetBulletDirection(Vector3 _originPos, Vector3 _desPos);
}
public interface IAbility
{
    float Health { get; }
    float MoveWalkSpeed { get; }
    float MoveDashSpeed { get; }

    void TakeDamage(float damage);
    void Death();
    void Heal(float amount);
    void SetAbilities();
}