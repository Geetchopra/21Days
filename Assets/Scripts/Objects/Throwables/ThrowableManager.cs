using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Throwable Manager. Uses the Throwable object to manage interactions and
/// inventory properties along with actions like throwing.
/// </summary>
[RequireComponent(typeof(Rigidbody)) ]
public class ThrowableManager : MonoBehaviour
{
    [SerializeField] private Throwable throwable;
    private Button button;
    private Text buttonText;
    private Sprite sprite;
    private Text prompt;
    public string Name
    {
        get
        {
            return throwable.name;
        }
    }

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        sprite = throwable.sprite;
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();

        if (!gameObject.CompareTag("Interactable"))
            throw new System.Exception("Didn't assign Interactable tag to " + throwable.name);
    }

    /// <summary>
    /// Update button text with the count of this throwable in the inventory.
    /// </summary>
    public void UpdateButton()
    {
        int count = PlayerItems.GetThrowableCount(throwable.name);
        buttonText.text = throwable.name + ": " + count.ToString();
        if (count == 0)
            button.enabled = false;
        else
            button.enabled = true;
    }

    /// <summary>
    /// Instantiate the button in the inventory UI.
    /// </summary>
    public void CreateButton()
    {
        button = Instantiate(throwable.button);
        button.transform.SetParent(GameObject.Find("Inventory/Throwables").transform);
        buttonText = button.GetComponentInChildren<Text>();

        ThrowableActions throwables = FindObjectOfType(typeof(ThrowableActions)) as ThrowableActions;
        button.onClick.AddListener(delegate { throwables.Activate(throwable.name); });

        UpdateButton();
    }

    /// <summary>
    /// Set the button active state.
    /// </summary>
    /// <param name="displayState"> True if enabled, else False. </param>
    public void SetButtonActive(bool displayState)
    {
        //Check if button is null, i.e. the throwable has been discovered.
        if (button != null)
            button.gameObject.SetActive(displayState);
    }

    /// <summary>
    /// Check if the throwable has been found yet by the player.
    /// </summary>
    /// <returns> True if found, else False. </returns>
    public bool IsDiscovered()
    {
        return button != null;
    }

    /// <summary>
    /// Check if at least one throwable is present in the inventory.
    /// </summary>
    /// <returns></returns>
    public bool IsEquipped()
    {
        return PlayerItems.Find("throwable", throwable.name) || button != null;
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
                prompt.text = "Pick this " + throwable.name + " up using E";
                prompt.text += "\nCost: " + throwable.costAmount + " " + TimeManager.ParseTime(throwable.costAmount, throwable.costType);

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
        TimeManager.ChangeTime(throwable.costAmount, throwable.costType, TimeManager.Operations.subtract);
        PlayerItems.Equip("throwable", throwable.name);
        Destroy(gameObject);
    }

    /// <summary>
    /// Instantiate a new object of this type and add force to it.
    /// </summary>
    /// <returns> The remaining count of the throwable of this type. </returns>
    public int Throw()
    {
        GameObject newThrowable = Instantiate(gameObject, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
        newThrowable.SetActive(true);
        newThrowable.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwable.throwingForce, ForceMode.Impulse);

        PlayerItems.Unequip("throwable", throwable.name);
        TimeManager.ChangeTime(throwable.costAmount, throwable.costType, TimeManager.Operations.subtract);
        return PlayerItems.GetThrowableCount(throwable.name);
    }

    /// <summary>
    /// Compare the name of this throwable.
    /// </summary>
    /// <param name="throwableName"> The name of the throwable to compare to. </param>
    /// <returns> True if equal, else False. </returns>
    public bool CompareName(string throwableName)
    {
        return throwable.name == throwableName;
    }

    /// <summary>
    /// Collision check to trigger appropriate effect if an AI character is hit with this.
    /// </summary>
    /// <param name="collision"> The collision object that this object collided with. </param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AI"))
        {
            AIController controller = collision.gameObject.GetComponent<AIController>();

            if (throwable.hitEffect == Throwable.HitEffects.stun)
                controller.Stun();
            else if (throwable.hitEffect == Throwable.HitEffects.slow)
                controller.Slow();
            else if (throwable.hitEffect == Throwable.HitEffects.damage)
                controller.Hit(collision.gameObject.transform.position, throwable.damageAmount);
        }

        //Fragile items.
        if (throwable.fragile && !collision.gameObject.CompareTag("Glass"))
            Destroy(gameObject);
    }
}
