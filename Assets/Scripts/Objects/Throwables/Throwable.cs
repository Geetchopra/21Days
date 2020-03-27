using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Throwable", menuName = "Throwable", order = 51)]
public class Throwable : ScriptableObject
{
    [Tooltip("The name or type of the throwable.")]
    public new string name;

    [HideInInspector]
    public enum HitEffects { stun = 0, slow = 1, damage = 2 };

    [Tooltip("Type of effect to apply on hit with an AI character.")]
    public HitEffects hitEffect;

    [Tooltip("Amount of damage to apply, if applicable based on the hit effect.")]
    public int damageAmount = 0;

    [Tooltip("Force with which to throw the object.")]
    public float throwingForce = 50;

    [Tooltip("If the object is fragile. If true, any throwable of this type will destroy on impact.")]
    public bool fragile = false;

    [Tooltip("Cost to pick up the throwable.")]
    public int costAmount;

    [Tooltip("Cost amount type.")]
    public TimeManager.Times costType;

    [Tooltip("UI Button to update in the inventory.")]
    public Button button;

    [Tooltip("UI sprite to update in the inventory.")]
    public Sprite sprite;
}