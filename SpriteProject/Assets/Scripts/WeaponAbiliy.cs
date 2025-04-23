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

[CreateAssetMenu(menuName = "SpriteProject/WeaponAbilityData")]
public class WeaponAbiliyData : ScriptableObject
{
    public float damage = 5.0f;
    public float speed = 10.0f;
    public float range = 300.0f;
}
