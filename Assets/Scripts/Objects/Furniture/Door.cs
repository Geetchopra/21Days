using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The door object. Contains logic to open it if the player has its particular key or the master key.
/// </summary>
public class Door : MonoBehaviour
{
    //The key with which this door unlocks. Can be unlocked (-1), i.e. no key needed to open door.
    [SerializeField] private int KeyID;

    //Distance at which the UI prompt triggers.
    [SerializeField] private float promptDistance = 3;
    private Text prompt;

    [SerializeField] private int costAmount = 30;
    [SerializeField] private TimeManager.Times costType = TimeManager.Times.minutes;

    //0 if door is closed, 1 if door is opened in its forward (z) axis, 2 if back axis.
    private int openDirection;

    private GameObject player;
    private Animator animator;

    /// <summary>
    /// Initialize objects in scene.
    /// </summary>
    void Start()
    {
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
        animator = GetComponent<Animator>();
        openDirection = 0;
        player = GameObject.Find("Player");
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
            //Debug.DrawRay(ray.origin, ray.direction * prompt_distance, Color.red);
            if (hit.collider.gameObject == gameObject)
            {
                //Door is to be closed
                if (openDirection != 0)
                {
                    prompt.text = "Press E to close door";
                    prompt.enabled = true;

                    if (Input.GetKeyDown(KeyCode.E))
                        Close();
                }
                else if (SetPrompt() && Input.GetKeyDown(KeyCode.E))
                {
                    Open();
                }
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

        if (KeyID == -1)
        {
            prompt.text = "Press E to open door";
        }
        //If the player has the master key.
        else if (PlayerItems.Find("key", "0"))
        {
            prompt.text = "Use master key.\nPress E to open door";
        }
        //If the player has the door specific key.
        else if (PlayerItems.Find("key", KeyID.ToString()))
        {
            prompt.text = "Use Key " + KeyID + ".\nPress E to open door";
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
    /// Open the door and update the time accordingly.
    /// Always opens outwards relative to the player.
    /// </summary>
    void Open()
    {
        TimeManager.ChangeTime(costAmount, costType, TimeManager.Operations.subtract);

        if (Vector3.Dot(transform.forward, player.transform.forward) > 0)
            openDirection = 1;
        else
            openDirection = 2;
        
        prompt.enabled = false;
        animator.SetInteger("open", openDirection);
    }

    /// <summary>
    /// Close the door (Set respective animator triggers) and update time.
    /// </summary>
    void Close()
    {
        TimeManager.ChangeTime(costAmount, costType, TimeManager.Operations.subtract);

        prompt.enabled = false;

        openDirection = 0;
        animator.SetInteger("open", openDirection);

        animator.SetTrigger("close");
    }
}
