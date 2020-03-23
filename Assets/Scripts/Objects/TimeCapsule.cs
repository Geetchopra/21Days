using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Time Capsule. Adds or subtracts time when collected. 
/// can be collected by walking over it.
/// </summary>
public class TimeCapsule : MonoBehaviour
{
    [SerializeField] private int costAmount;
    [SerializeField] private TimeManager.Times costType;
    [SerializeField] private TimeManager.Operations operation;

    /// <summary>
    /// Add or subtract time if collided with the player.
    /// </summary>
    /// <param name="collision">The collision object that this item collided with.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            TimeManager.ChangeTime(costAmount, costType, operation);
            Destroy(gameObject);
        }
    }
}
