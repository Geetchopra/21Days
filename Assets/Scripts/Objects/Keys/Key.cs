using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Key", menuName = "Key", order = 52)]
public class Key : ScriptableObject
{
    public int ID;
    public int costAmount = 50;
    public TimeManager.Times costType = TimeManager.Times.minutes;

    public Image image;
}
