using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines throwable actions. Attached to a player to 
/// initiate throwing of different items.
/// </summary>
public class ThrowableActions : MonoBehaviour
{
    private Text text;

    private ThrowableManager currentThrowable;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Reticle reticle;

    //If any throwable is currently equipped
    private bool isEquipped;

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        text = GameObject.Find("Weapon Text").GetComponent<Text>();
        isEquipped = false;
    }

    /// <summary>
    /// Activate or equip the throwable and make it ready for throwing.
    /// </summary>
    /// <param name="name"> The throwable type to equip. </param>
    public void Activate(string name)
    {
        isEquipped = true;
        text.text = name + ": " + PlayerItems.GetThrowableCount(name);
        currentThrowable = inventory.GetThrowable(name);
    }

    /// <summary>
    /// Deactivate or unequip throwables.
    /// </summary>
    public void Deactivate()
    {
        isEquipped = false;
    }

    /// <summary>
    /// Called once every frame.
    /// </summary>
    void Update()
    {
        if (isEquipped && !Input.GetKey(KeyCode.N))
        {
            SetActiveUI(true);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                int remaining = currentThrowable.Throw();
                text.text = currentThrowable.Name + ": " + remaining;

                //No more throwables available of currentType.
                if (remaining == 0)
                {
                    string newType = PlayerItems.GetNextThrowable();

                    if (newType == null)
                    {
                        //Unequip if no throwables available.
                        isEquipped = false;
                    }
                    else
                    {
                        //Equip another throwable type.
                        currentThrowable = inventory.GetThrowable(newType);
                        text.text = newType + ": " + PlayerItems.GetThrowableCount(newType);
                    }
                }
            }
        }
        else
        {
            SetActiveUI(false);
        }
    }

    /// <summary>
    /// Set the UI elements' active state.
    /// </summary>
    /// <param name="displayState"> True to enable it, False to disable it. </param>
    void SetActiveUI(bool displayState)
    {
        text.gameObject.SetActive(displayState);
        reticle.SetActiveCrosshair(displayState);
    }
}
