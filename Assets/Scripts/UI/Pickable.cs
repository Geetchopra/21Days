using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Deprecated class. Not being used currently.
/// Will change this to include collectibles.
/// </summary>
public class Pickable : MonoBehaviour
{
    [SerializeField] private string itemType;
    [SerializeField] private string itemID;
    [SerializeField] private int costAmount;
    [SerializeField] private TimeManager.Times costType;
    private Text prompt;
    [SerializeField] private float promptDistance;
    
    /// <summary>
    /// Check if cost values are initialized correctly.
    /// </summary>
    void Start()
    {
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
    }

    /// <summary>
    /// Called every frame.
    /// Displays the UI prompt to pick the item up based on the promptDistance.
    /// </summary>
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);     

        if (Physics.Raycast(ray, out RaycastHit hit, promptDistance) && !Cursor.visible)
        {
            //Debug.DrawRay(ray.origin, ray.direction * prompt_distance, Color.red);
            if (hit.collider.gameObject == gameObject)
            {
                //Set the prompt text based on appropriate values.
                prompt.text = "Pick this " + itemType + " up using E";
                prompt.text += "\nCost: " + costAmount + " " + TimeManager.ParseTime(costAmount, costType);

                prompt.enabled = true;

                if (Input.GetKey(KeyCode.E))
                {
                    PickUp();
                    prompt.enabled = false;
                }
            }           
        }
        else
        {
            prompt.enabled = false;
        }        
    }

    /// <summary>
    /// Equip the item in the inventory and destroy this object.
    /// </summary>
    void PickUp()
    {
        TimeManager.ChangeTime(costAmount, costType, TimeManager.Operations.subtract);
        PlayerItems.Equip(itemType, itemID);
        Destroy(gameObject);
    }
}
