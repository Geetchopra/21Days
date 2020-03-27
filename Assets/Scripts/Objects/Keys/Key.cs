using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Key", menuName = "Key", order = 52)]
public class Key : ScriptableObject
{
    [Tooltip("The unique ID of this key. Master keys have an ID of 0.")]
    public int ID;

    [Tooltip("Cost to pick up the key.")]
    public int costAmount = 50;

    [Tooltip("Cost amount type.")]
    public TimeManager.Times costType = TimeManager.Times.minutes;

    [Tooltip("UI Image to update in the inventory.")]
    public Image image;
}
