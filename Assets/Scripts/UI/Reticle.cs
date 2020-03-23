using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    [SerializeField] private Image main;
    [SerializeField] private Image highlight;
    [SerializeField] private Image crosshair;

    public static float interactablePromptDistance = 3f;

    void Start()
    {
        main.enabled = true;
        highlight.enabled = false;
        crosshair.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.visible)
        {
            SetActiveUI(false);
        }
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

    public void SetActiveCrosshair(bool displayState)
    {
        crosshair.enabled = displayState;
    }

    private void SetActiveUI(bool displayState)
    {
        main.enabled = displayState;
        highlight.enabled = displayState;
        crosshair.enabled = displayState;
    }
}
