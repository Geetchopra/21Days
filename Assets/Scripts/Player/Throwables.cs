using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to the player object. Controls throwable item actions.
/// </summary>
public class Throwables : MonoBehaviour
{
    private Image reticle;
    private Text text;

    //Throwable objects to use.
    [SerializeField] private List<GameObject> throwables = new List<GameObject>();

    //Checks if any throwable is equipped.
    private bool isEquipped;

    //Speed at which all throwables are thrown.
    public float speed;

    private string currentType;

    /// <summary>
    /// Initialize equipped to false.
    /// </summary>
    void Start()
    {
        reticle = GameObject.Find("Reticle").GetComponent<Image>();
        text = GameObject.Find("Weapon Text").GetComponent<Text>();
        isEquipped = false;
    }

    /// <summary>
    /// Activate the item for the player, i.e. enable the reticle.
    /// </summary>
    public void Activate(string type)
    {
        if (PlayerItems.GetThrowableCount(type) > 0)
        {
            isEquipped = true;
            currentType = type;
            text.text = currentType + ": " + PlayerItems.GetThrowableCount(currentType);
        }
    }

    /// <summary>
    /// Deactivate the throwable item, i.e. disable the reticle.
    /// </summary>
    public void Deactivate()
    {
        isEquipped = false;
    }

    /// <summary>
    /// Update - Called once per frame.
    /// Check if respective keybind is pressed and throw a currentType object.
    /// </summary>
    void Update()
    {
        if (isEquipped)
        {
            SetActiveUI(true);

            if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.N))
            {
                Throw();

                //If no throwables of the current type are left, equip a different throwable.
                if (PlayerItems.GetThrowableCount(currentType) == 0)
                {
                    currentType = PlayerItems.GetNextThrowable();

                    if (currentType == null)
                    {
                        //Unequip if no throwables available.
                        isEquipped = false;
                    }
                    else
                    {
                        text.text = currentType + ": " + PlayerItems.GetThrowableCount(currentType);
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
    /// Set the active state of the UI elements.
    /// </summary>
    /// <param name="displayState"> The boolean parameter to set the display state to. </param>
    void SetActiveUI(bool displayState)
    {
        reticle.enabled = displayState;
        text.gameObject.SetActive(displayState);
    }


    /// <summary>
    /// Instantiate a new object of currentType and add a force to it.
    /// </summary>
    void Throw()
    {
        GameObject throwable = throwables.Find(x => x.name == currentType);

        GameObject obj = Instantiate(throwable, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
        obj.SetActive(true);
        obj.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * speed, ForceMode.Impulse);

        PlayerItems.Unequip("throwable", currentType);
        text.text = currentType + ": " + PlayerItems.GetThrowableCount(currentType);
    }
}
