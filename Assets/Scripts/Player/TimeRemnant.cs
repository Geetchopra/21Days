using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adds a time remnant to the screen if the respective key is held for 
/// the respective amount of time.
/// </summary>
public class TimeRemnant : MonoBehaviour
{
    [SerializeField] private GameObject timeRemnant;

    /// <summary>
    /// Update - Called once every frame.
    /// Launches a bar if the respective key is pressed and held. 
    /// If the bar finishes loading, then spawns a new time remnant.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RadialLoader.Launch(2.0f, "time", true, KeyCode.Q);
        }

        if (RadialLoader.Completed())
        {
            GameObject remnant = Instantiate(timeRemnant, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
            remnant.SetActive(true);
        }
    }
}
