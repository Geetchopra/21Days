using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Key Manager. Uses the Key object to manage interactions and
/// inventory properties.
/// </summary>
public class KeyManager : MonoBehaviour
{
    [SerializeField] private Key key;
    private Image image;
    private Text prompt;

    public string ID
    {
        get
        {
            return key.ID.ToString();
        }
    }

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
    }

    /// <summary>
    /// Check if the key has been discovered by the player.
    /// </summary>
    /// <returns> True if the key has been found, else False. </returns>
    public bool Discovered()
    {
        return image != null;
    }

    /// <summary>
    /// Instantiate the key image to the inventory.
    /// </summary>
    public void CreateImage()
    {
        image = Instantiate(key.image);
        image.transform.parent = GameObject.Find("Inventory/Keys").transform;
    }

    /// <summary>
    /// Set the enabled state of the key.
    /// </summary>
    /// <param name="displayState"> The state to set it active to. </param>
    public void SetImageActive(bool displayState)
    {
        //Ensure image is not null, i.e. the key has to have been found first.
        if (image != null)
            image.enabled = displayState;
    }

    /// <summary>
    /// Called once every frame.
    /// </summary>
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 3.0f) && !Cursor.visible)
        {
            //Debug.DrawRay(ray.origin, ray.direction * prompt_distance, Color.red);
            if (hit.collider.gameObject == gameObject)
            {
                //Set the prompt text based on appropriate values.
                prompt.text = "Pick key number " + ID + " up using E";
                prompt.text += "\nCost: " + key.costAmount + " " + TimeManager.ParseTime(key.costAmount, key.costType);

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
    /// Destroy the key object and update time accordingly.
    /// </summary>
    void PickUp()
    {
        TimeManager.ChangeTime(key.costAmount, key.costType, TimeManager.Operations.subtract);
        PlayerItems.Equip("key", ID);
        Destroy(gameObject);
    }
}
