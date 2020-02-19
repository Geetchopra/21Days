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
    [SerializeField] private char costType;
    [SerializeField] private string operation;

    /// <summary>
    /// Check if cost type and operation are initialized in the editor.
    /// </summary>
    void Start()
    {
        if (costType != 'd' && costType != 'h' && costType != 'm' && costType != 's')
            throw new Exception("Error: Time type is not initialized correctly");

        if (operation != "add" && operation != "subtract")
            throw new Exception("Add_or_subtract not initialized correctly");
    }

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
