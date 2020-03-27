using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Static reticle controller class. Updates the
/// reticle based on appropriate events happening in game.
/// </summary>
public class Reticle : MonoBehaviour
{
    [SerializeField] private Image main;
    [SerializeField] private Image highlight;
    [SerializeField] private Image crosshair;

    public static float interactablePromptDistance = 3f;

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        main.enabled = true;
        highlight.enabled = false;
        crosshair.enabled = false;
    }

    /// <summary>
    /// Called once every frame.
    /// </summary>
    void Update()
    {
        //There can only be one...
        if (Cursor.visible)
        {
            SetActiveUI(false);
        }
        //Change the reticle if there is an interactable object being pointed at.
        else
        {
            main.enabled = !crosshair.enabled;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, interactablePromptDistance))
            {
                highlight.enabled = hit.collider.CompareTag("Interactable");
            }
            else
            {
                highlight.enabled = false;
            }
        }

    }

    /// <summary>
    /// Set the active state of the crosshair reticle.
    /// </summary>
    /// <param name="displayState"> True if enabled, else False. </param>
    public void SetActiveCrosshair(bool displayState)
    {
        crosshair.enabled = displayState;
    }

    /// <summary>
    /// Set the active state of the entire reticle, i.e. all possible reticle images.
    /// </summary>
    /// <param name="displayState"> True if enabled, else False. </param>
    private void SetActiveUI(bool displayState)
    {
        main.enabled = displayState;
        highlight.enabled = displayState;
        crosshair.enabled = displayState;
    }
}
