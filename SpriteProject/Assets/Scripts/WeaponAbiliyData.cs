using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpriteProject/WeaponAbilityData")]
public class WeaponAbiliyData : ScriptableObject
{
    public float damage = 5.0f;
    public float fireRate = 1.0f;
    public float speed = 10.0f;
    public float range = 300.0f;
}
