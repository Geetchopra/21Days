using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A template for each throwable item with a throwable type, 
/// count and UI button associated with it.
/// </summary>
[CreateAssetMenu(fileName = "New Throwable", menuName = "Throwable", order = 51)]
public class Throwable : ScriptableObject
{
    public new string name;
    [HideInInspector]
    public enum HitEffects { stun = 0, slow = 1, damage = 2, attract = 3 };
    public int damageAmount = 0;
    public HitEffects hitEffect;
    public float throwingForce;
    public int costAmount;
    public TimeManager.Times costType;
    public Button button;
    public Sprite image;
}