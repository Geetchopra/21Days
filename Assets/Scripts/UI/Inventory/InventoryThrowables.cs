using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// UI manager for the keys in the inventory.
/// </summary>
public class InventoryThrowables : MonoBehaviour
{
    //The throwable objects to be instantiated at runtime.
    [SerializeField] private List<ThrowableManager> throwables = new List<ThrowableManager>();

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
    /// Set the active state of the keys in use and enable the cursor.
    /// </summary>
    /// <param name="displayState"> The boolean parameter to set the display state to. </param>
    void SetActiveUI(bool displayState)
    {
        foreach (ThrowableManager throwable in throwables)
        {
            throwable.SetButtonActive(displayState);
        }

        //Enable the cursor.
        Cursor.visible = displayState;

        //If inventory is visible, disable camera input and vice versa.
        if (displayState)
        {
            Cursor.lockState = CursorLockMode.None;
            MoveCamera.DisableInput();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            MoveCamera.EnableInput();
        }
    }

    /// <summary>
    /// Update the button values based on the current inventory state.
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
    }

    public ThrowableManager GetThrowable(string name)
    {
        return throwables.Find(throwable => throwable.CompareName(name));
    }
}
