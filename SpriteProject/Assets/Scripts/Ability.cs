using UnityEngine;

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

[CreateAssetMenu(menuName = "SpriteProject/AbilityData")]
public class AbilityData : ScriptableObject
{
    [SerializeField] public float health = 100f;
    [SerializeField] public float moveWalkSpeed = 5f;
    [SerializeField] public float moveDashSpeed = 5f;
}