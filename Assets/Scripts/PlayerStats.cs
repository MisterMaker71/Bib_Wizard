using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    [Min(0f)]
    public float ManaRegen = 15;
    [Min(0f)]
    public float HealthRegen = 2.5f;
    public float[] XpMilestones = { 5, 10 };
    //public AnimationCurve XpMilestones;

    public void resetToDefault()
    {
        MaxMana = 50;
        MaxHealth = 100;
        MovementSpeed = 5;
        CastingSpeed = 1;
        Damage = 2;
        ManaRegen = 15;
        HealthRegen = 2.5f;
    }
    public void TryLevelUp()
    {
        int i = Wizard.wizard.level;
        if (Wizard.wizard.level >= XpMilestones.Length)
        {
            i = XpMilestones.Length - 1;
        }

        if (Wizard.wizard.xp >= XpMilestones[i])
        {
            Wizard.wizard.xp -= XpMilestones[i];
            Wizard.wizard.level++;
        }
    }
    public void PowerUpAll(float level = 1)
    {
        MaxHealth += level * 5;
        MaxMana += level * 10;
        MovementSpeed += level * 1.5f;
        CastingSpeed += level * 0.5f;
        Damage += level * 1;
    }
}

