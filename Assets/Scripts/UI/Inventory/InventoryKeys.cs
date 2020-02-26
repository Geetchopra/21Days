using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// UI manager for the keys in the inventory.
/// </summary>
public class InventoryKeys: MonoBehaviour
{
    //The Key objects to be instantiated at runtime.
    [SerializeField] public List<Keys> keys = new List<Keys>();

    private List<Keys> keysInUse = new List<Keys>();

    private GameObject canvas;

    //Controls the position of each image on the screen.
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
        foreach (Keys key in keysInUse)
        {
            key.image.enabled = displayState;
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
    /// Update the UI based on the current inventory state.
    /// </summary>
    void UpdateUI()
    {
        foreach (Keys key in keys)
        {
            if (PlayerItems.Find("key", key.name) && !keysInUse.Exists(lambda => lambda.name.Equals(key.name)))
            {
                Image image = Instantiate(key.image, canvas.transform, false);
                image.enabled = false;
                
                image.transform.position += new Vector3(40 * x, 40 * y, 0);

                x += 1;

                keysInUse.Add(new Keys(key.name, image));
            }
        }
    }
}


/// <summary>
/// A template for each key with a name and image associated with it.
/// </summary>
[Serializable]
public class Keys
{
    public string name;
    public Image image;

    public Keys(string keyName, Image keyImage)
    {
        name = keyName;
        image = keyImage;
    }
}

