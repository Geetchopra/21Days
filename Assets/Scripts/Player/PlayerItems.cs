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
    private static bool sprayGun;
    private static List<string> keys = new List<string>();
    private static List<string> throwables = new List<string>();

    /// <summary>
    /// Start - Called before the first frame update.
    /// </summary>
    void Start()
    {
        sprayGun = false;
    }

    /// <summary>
    /// TODO: Spray something on the wall.
    /// </summary>
    public static void Spray()
    { }

    /// <summary>
    /// Check is the spray is equipped or not.
    /// </summary>
    /// <returns> The value of sprayGun. </returns>
    public static bool IsSprayEquipped()
    {
        return sprayGun;
    }

    /// <summary>
    /// Get the number of items of the particular inventory category.
    /// </summary>
    /// <param name="type"> The type of item - keys or throwables </param>
    /// <param name="item"> TODO: The item type - used for throwables </param>
    /// <returns></returns>
    public static int GetListCount(string type, string item = null)
    {
        if (type == "keys" && keys.Any())
        {
            return keys.Count;
        }
        else if (type == "throwables" && throwables.Any())
        {
            return throwables.Count;
        }
        else
        {
            return 0;
        }
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
            throwables.Add(value);
        }
        else if (type == "spray_gun")
        {
            sprayGun = true;
        }
    }

    /// <summary>
    /// Removes the respective item from the inventory.
    /// </summary>
    /// <param name="type"> The type of item - keys or throwables. </param>
    /// <param name="value"> The item to be removed. </param>
    public static void Unequip(string type, string value)
    {
        if (type == "keys")
        {
            keys.Remove(value);
        }
        else if (type == "throwables")
        {
            throwables.Remove(value);
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
        if (type == "keys")
        {
            return keys.Exists(x => x == value);
        }
        else if (type == "throwables")
        {
            return throwables.Exists(x => x == value);
        }
        else
        {
            return false;
        }
    }
}
