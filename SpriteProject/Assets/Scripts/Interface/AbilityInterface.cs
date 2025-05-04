using UnityEngine;
public interface IWeaponAbility
{
    void Shoot(Vector3 desPos); // 무기에서 무조건 구현해야하는 함수 
}

public interface IBulletAbility
{
    float Damage { get; }
    float Speed { get; }
    float Range { get; }

    void SetBulletAbility(WeaponAbiliyData weaponAbiliyData);
    void SetBulletDirection(Vector3 _originPos, Vector3 _desPos);
}
public interface IPlayerAbility
{
    float Health { get; }
    float MoveWalkSpeed { get; }
    float MoveDashSpeed { get; }

    void TakeDamage(float damage);
    void Death();
    void Heal(float amount);
    void SetAbilities();
}