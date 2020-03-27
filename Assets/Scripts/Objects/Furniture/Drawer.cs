using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains logic to interact with drawers.
/// The drawers can be inside other furniture as well, e.g. wardrobes.
/// </summary>
public class Drawer : MonoBehaviour
{
    //The prompt Text object.
    private Text prompt;
    private float promptDistance = 3f;

    [SerializeField] private int costAmount = 30;
    [SerializeField] private TimeManager.Times costType = TimeManager.Times.minutes;

    private Animator animator;

    /// <summary>
    /// Initialize objects in scene.
    /// </summary>
    void Start()
    {
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update - Called every frame.
    /// </summary>
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, promptDistance) && !Cursor.visible)
        {
            if (hit.collider.gameObject == gameObject)
            {
                bool state = SetPrompt();

                if (Input.GetKeyDown(KeyCode.E))
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
    /// Set the UI prompt based on whether the drawer is to open to close.
    /// </summary>
    /// <returns> True if opening, False if closing. </returns>
    bool SetPrompt()
    {
        if (animator.GetBool("open"))
        {
            prompt.text = "Press E to close the drawer";
            prompt.enabled = true;
            return false;
        }
        else 
        {
            prompt.text = "Press E to open the drawer";
            prompt.enabled = true;
            return true;
        }
    }

    /// <summary>
    /// Open or close the drawer.
    /// Also updates time accordingly.
    /// </summary>
    /// <param name="state"> True if opening, False if closing. </param>
    void Interact(bool state)
    {
        TimeManager.ChangeTime(costAmount, costType, TimeManager.Operations.subtract);
        animator.SetBool("open", state);
        prompt.enabled = false;
    }
}
