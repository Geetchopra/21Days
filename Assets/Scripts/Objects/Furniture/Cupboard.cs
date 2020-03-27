using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cupboard : MonoBehaviour
{
    //UI prompt which updates to indicate interaction.
    private Text prompt;
    private float promptDistance = 3f;

    [SerializeField] private int costAmount = 30;
    [SerializeField] private TimeManager.Times costType = TimeManager.Times.minutes;

    private Animator animator;

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Raycast to see if player is within promptDistance.
        if (Physics.Raycast(ray, out RaycastHit hit, promptDistance) && !Cursor.visible)
        {
            if (hit.collider.gameObject == gameObject)
            {
                bool state = SetPrompt();

                if (Input.GetKeyDown(KeyCode.E) && prompt.enabled)
                    Interact(state);
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
    /// Set the prompt text based on if the cupboard door can be closed or opened.
    /// </summary>
    /// <returns> True if the cupboard can be opened, False if it can be closed. </returns>
    bool SetPrompt()
    {
        if (animator.GetBool("open"))
        {
            if (CanBeClosed())
            {
                prompt.text = "Press E to close door";
                prompt.enabled = true;
            }
            return false;
        }
        else
        {
            prompt.text = "Press E to open door";
            prompt.enabled = true;
            return true;
        }
    }

    /// <summary>
    /// Check if the cupboard can be realistically closed, i.e. if there 
    /// is a drawer open inside the cupboard or not.
    /// </summary>
    /// <returns> True if it can be closed, otherwise False. </returns>
    bool CanBeClosed()
    {
        foreach(Transform obj in transform.parent)
        {
            if (obj.name.Contains("Drawer"))
            {
                Animator animator = obj.GetComponentInChildren<Animator>();
                if (animator.GetBool("open"))
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Set the animator state to either open or close the door and update the time accordingly.
    /// </summary>
    /// <param name="state"> True if the cupboard door is to be opened, False if it is to be closed. </param>
    void Interact(bool state)
    {
        TimeManager.ChangeTime(costAmount, costType, TimeManager.Operations.subtract);
        animator.SetBool("open", state);
        prompt.enabled = false;
    }
}
