using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PlayerStats", fileName = "Assets/playerStats")]
public class PlayerStats : ScriptableObject
{
    [Range(0f, 200)]
    public float MaxMana = 50;
    [Range(0f, 200)]
    public float MaxHealth = 100;
    [Range(1f, 10)]
    public float MovementSpeed = 5;
    [Range(.5f, 5)]
    public float CastingSpeed = 1;
    [Range(0, 500)]
    public float Damage = 2;
}
