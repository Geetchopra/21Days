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
    [SerializeField] private List<Throwable> throwables = new List<Throwable>();

    private List<Throwable> throwablesInUse = new List<Throwable>();

    private GameObject canvas;

    //Controls the position of each button on the screen.
    private float x, y;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        UpdateUI();
        canvas = GameObject.Find("Canvas");

        x = 0;
        y = 0;
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
        foreach (Throwable throwable in throwablesInUse)
        {
            throwable.button.gameObject.SetActive(displayState);
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
        foreach (Throwable throwable in throwables)
        {
            //If player already has / had the throwable, then just update the values for its button.
            if (throwablesInUse.Exists(lambda => lambda.type.Equals(throwable.type)))
            {
                int index = throwablesInUse.FindIndex(lambda => lambda.type.Equals(throwable.type));
                int count = PlayerItems.GetThrowableCount(throwable.type);
                if (throwablesInUse[index].count != count)
                {
                    throwablesInUse[index].count = count;
                    throwablesInUse[index].button.GetComponentInChildren<Text>().text = throwable.type + ": " + count.ToString();
                    if (count == 0)
                    {
                        throwablesInUse[index].button.enabled = false;
                    }
                    else
                    {
                        throwablesInUse[index].button.enabled = true;
                    }
                }
            }
            //Create a new button if the player found a new item.
            else if (PlayerItems.Find("throwable", throwable.type))
            {
                Button button = Instantiate(throwable.button, canvas.transform, false);
                button.enabled = true;
                button.transform.position += new Vector3(100 * x, 10 * y, 0);
                button.GetComponentInChildren<Text>().text = throwable.type + ": " + 1.ToString();
                x += 1;
                throwablesInUse.Add(new Throwable(throwable.type, 1, button));
            }
        }
    }
}

/// <summary>
/// A template for each throwable item with a throwable type, 
/// count and UI button associated with it.
/// </summary>
[Serializable]
public class Throwable
{
    public string type;
    public int count;
    public Button button;

    public Throwable(string tType, int tCount, Button tButton)
    {
        type = tType;
        count = tCount;
        button = tButton;
    }
}
