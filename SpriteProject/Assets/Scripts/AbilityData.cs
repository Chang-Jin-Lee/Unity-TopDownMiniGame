using UnityEngine;

[CreateAssetMenu(menuName = "SpriteProject/AbilityData")]
public class AbilityData : ScriptableObject
{
    [SerializeField] public float health = 100f;
    [SerializeField] public float moveWalkSpeed = 5f;
    [SerializeField] public float moveDashSpeed = 5f;
}