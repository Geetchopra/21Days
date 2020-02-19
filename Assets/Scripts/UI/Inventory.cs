using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI manager for the inventory.
/// TODO: Make everything an array of Items.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private Item keys;
    [SerializeField] private Item throwables;
    [SerializeField] private Item sprayGun;
    [SerializeField] private Item torch;
    
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

    void SetActiveUI(bool display_state)
    {
        //Enable all buttons.
        keys.button.gameObject.SetActive(display_state);
        sprayGun.button.gameObject.SetActive(display_state);
        torch.button.gameObject.SetActive(display_state);
        throwables.button.gameObject.SetActive(display_state);

        //Enable the cursor.
        Cursor.visible = display_state;

        //If inventory is visible, disable camera input and vice versa.
        if (display_state)
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
    /// Update the UI values based on the current inventory state.
    /// </summary>
    void UpdateUI()
    {
        //Torch is always present from the start.
        torch.count = 1;
        torch.button.GetComponentInChildren<Text>(true).text = "Torch: " + torch.count.ToString();

        if (PlayerItems.IsSprayEquipped())
        {
            sprayGun.count = 1;
        }
        else
        {
            sprayGun.count = 0;
        }

        sprayGun.button.GetComponentInChildren<Text>(true).text = "Gun: " + sprayGun.count.ToString();

        keys.count = PlayerItems.GetListCount("keys");
        keys.button.GetComponentInChildren<Text>(true).text = "Keys: " + keys.count.ToString();

        throwables.count = PlayerItems.GetListCount("throwables");
        throwables.button.GetComponentInChildren<Text>(true).text = "Throwables: " + throwables.count.ToString();
    }
}

/// <summary>
/// Basic template for an item. Contains a count and the corresponding button for it.
/// </summary>
[System.Serializable]
public class Item 
{
    public int count;
    public Button button;
}