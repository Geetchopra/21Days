using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Plan to combine this with Door.cs eventually.. This is for testing and is the same as Door.cs
/// with an extra animator component.
/// </summary>
public class CupboardOld : MonoBehaviour
{
    //The key with which this door unlocks. Can be unlocked, i.e. no key needed to open door.
    [SerializeField] private string KeyID;

    //Distance at which the UI prompt triggers.
    [SerializeField] private float promptDistance;

    //The prompt Text object.
    private Text prompt;

    //Integer cost amount.
    [SerializeField] private int costAmount;

    //Cost type (h - hours, m - minutes, d - days, s - seconds).
    [SerializeField] private char costType;

    private Animator animator;

    /// <summary>
    /// Initialize objects in scene.
    /// </summary>
    void Awake()
    {
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update - Called every frame.
    /// Contains logic to open door if the player contains the respective keys.
    /// </summary>
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, promptDistance) && !Cursor.visible)
        {
            //Debug.DrawRay(ray.origin, ray.direction * promptDistance, Color.red);
            if (hit.collider.gameObject == gameObject)
            {
                //Set the text prompt and check if the door can be opened.                
                bool canBeOpened = SetPrompt();

                if (canBeOpened && Input.GetKeyDown(KeyCode.E))
                {
                    Open(true);
                    prompt.enabled = false;
                }
                else if (animator.GetBool("open") && Input.GetKeyDown(KeyCode.E))
                {
                    Open(false);
                    prompt.enabled = false;
                }
            }
            else if (!hit.collider.gameObject.CompareTag("Interactable"))
            {
                prompt.enabled = false;
            }
        }
        else
        {
            prompt.enabled = false;
        }
    }

    /// <summary>
    /// Sets the text prompt based on if the player has the key or not.
    /// </summary>
    /// <returns> True, if the door can be opened, otherwise False. </returns>
    bool SetPrompt()
    {
        bool canBeOpened = true;

        if (animator.GetBool("open"))
        {
            prompt.text = "Press E to close door";
            canBeOpened = false;
        }
        else if (KeyID == "unlocked")
        {
            prompt.text = "Press E to open door";
        }
        //If the player has the master key.
        else if (PlayerItems.Find("key", "master"))
        {
            prompt.text = "Use master key.\nPress E to open door";
        }
        //If the player has the door specific key.
        else if (PlayerItems.Find("key", KeyID))
        {
            prompt.text = "Use " + KeyID + ".\nPress E to open door";
        }
        else
        {
            prompt.text = "Locked!";
            canBeOpened = false;
        }

        //Make the prompt visible
        prompt.enabled = true;
        return canBeOpened;
    }

    /// <summary>
    /// Open the door and update the time based on cost amount and type.
    /// Currently destroys the door. Will add logic to use an animation or something.
    /// </summary>
    void Open(bool state)
    {
        TimeManager.ChangeTime(costAmount, costType, "subtract");
        animator.SetBool("open", state);
    }
}
