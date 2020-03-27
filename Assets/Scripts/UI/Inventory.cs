using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// UI manager for the different items in the inventory.
/// Also contains a reference to all possible items available to the player.
/// </summary>
public class Inventory : MonoBehaviour
{
    //The throwable objects to be instantiated.
    [SerializeField] private List<ThrowableManager> throwables = new List<ThrowableManager>();

    //The Key objects to be instantiated.
    [SerializeField] public List<KeyManager> keys = new List<KeyManager>();

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// Show the UI if the respective key is pressed.
    /// </summary>
    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            UpdateUI();
            SetActiveUI(true);
        }
        else
        {
            SetActiveUI(false);
        }
    }

    /// <summary>
    /// Set the active state of the items in use and enable the cursor.
    /// </summary>
    /// <param name="displayState"> The boolean parameter to set the display state to. </param>
    void SetActiveUI(bool displayState)
    {
        foreach (ThrowableManager throwable in throwables)
        {
            throwable.SetButtonActive(displayState);
        }

        foreach (KeyManager key in keys)
        {
            key.SetImageActive(displayState);
        }

        //Enable the cursor.
        Cursor.visible = displayState;

        //If inventory is visible, disable camera input and vice versa.
        if (displayState)
        {
            Cursor.lockState = CursorLockMode.None;
            MoveCamera.DisableInput();
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            MoveCamera.EnableInput();
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Update the UI values based on the current inventory state.
    /// </summary>
    void UpdateUI()
    {
        foreach (ThrowableManager throwable in throwables)
        {
            if (throwable.IsEquipped())
            {
                if (throwable.IsDiscovered())
                    throwable.UpdateButton();
                else
                    throwable.CreateButton();
            }
        }

        //Only need to update keys if a new key has been found.
        foreach (KeyManager key in keys)
        {
            if (PlayerItems.Find("key", key.ID) && !key.Discovered())
            {
                key.CreateImage();
            }
        }
    }

    /// <summary>
    /// Get the ThrowableManager object of a specified name (type).
    /// </summary>
    /// <param name="name"> The type of object to return. </param>
    /// <returns> ThrowableManager of type 'name'. </returns>
    public ThrowableManager GetThrowable(string name)
    {
        return throwables.Find(throwable => throwable.CompareName(name));
    }
}
