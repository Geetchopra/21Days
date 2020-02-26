using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the inventory of the items the player has.
/// </summary>
public class PlayerItems : MonoBehaviour
{
    private static List<string> keys = new List<string>();
    private static Dictionary<string, int> throwables = new Dictionary<string, int>();

    /// <summary>
    /// Get the number of keys in the current player inventory.
    /// </summary>
    /// <returns> The number of keys. </returns>
    public static int GetKeyCount()
    {
        if (keys.Any())
            return keys.Count;
        else
            return 0;
    }

    /// <summary>
    /// Get the number of throwables of a particular type in the current player inventory.
    /// </summary>
    /// <param name="type"> The type of throwable. </param>
    /// <returns> The number of throwables available. </returns>
    public static int GetThrowableCount(string type)
    {
        if (throwables.ContainsKey(type))
            return throwables[type];
        else
            return 0;
    }

    /// <summary>
    /// Gets the next available throwable in the inventory.
    /// </summary>
    /// <returns> String value of the throwable type, or null if no available throwables. </returns>
    public static string GetNextThrowable()
    {
        if (throwables.Any())
            return throwables.Keys.First();
        else
            return null;
    }

    /// <summary>
    /// Add the respective item to the inventory.
    /// </summary>
    /// <param name="type"> The type of item - keys or throwables. </param>
    /// <param name="value"> The item to be added. </param>
    public static void Equip(string type, string value)
    {
        if (type == "key")
        {
            keys.Add(value);
        }
        else if (type == "throwable")
        {
            if (throwables.ContainsKey(value))
                throwables[value] += 1;
            else
                throwables.Add(value, 1);
        }
    }

    /// <summary>
    /// Removes the respective item from the inventory.
    /// </summary>
    /// <param name="type"> The type of item - keys or throwables. </param>
    /// <param name="value"> The item to be removed. </param>
    public static void Unequip(string type, string value)
    {
        if (type == "key")
        {
            keys.Remove(value);
        }
        else if (type == "throwable")
        {
            if (throwables[value] == 1)
                throwables.Remove(value);
            else
                throwables[value] -= 1;
        }
    }

    /// <summary>
    /// Check if a particular item is present in the inventory.
    /// </summary>
    /// <param name="type"> The type of item - keys or throwables. </param>
    /// <param name="value"> The item to be searched for. </param>
    /// <returns> True, if value was found, else False. </returns>
    public static bool Find(string type, string value) 
    {
        if (type == "key")
            return keys.Exists(x => x == value);
        else if (type == "throwable")
            return throwables.ContainsKey(value);
        else
            return false;
    }
}
