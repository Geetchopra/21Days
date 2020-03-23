using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowableManager : MonoBehaviour
{
    [SerializeField] private Throwable throwable;
    private Button button;
    private Text buttonText;
    private Sprite image;
    public string Name
    {
        get
        {
            return throwable.name;
        }
    }

    public Throwable.HitEffects HitEffect
    {
        get
        {
            return throwable.hitEffect;
        }
    }

    public int Damage
    {
        get
        {
            return throwable.damageAmount;
        }
    }

    private Text prompt;

    void Start()
    {
        image = throwable.image;
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
    }

    public void UpdateButton()
    {
        int count = PlayerItems.GetThrowableCount(throwable.name);
        buttonText.text = throwable.name + ": " + count.ToString();
        if (count == 0)
            button.enabled = false;
        else
            button.enabled = true;
    }

    public void CreateButton()
    {
        button = Instantiate(throwable.button);
        button.transform.parent = GameObject.Find("Inventory").transform;
        buttonText = button.GetComponentInChildren<Text>();
        UpdateButton();
    }

    public void SetButtonActive(bool displayState)
    {
        if (button != null)
            button.gameObject.SetActive(displayState);
    }

    public bool IsDiscovered()
    {
        return button != null;
    }

    public bool IsEquipped()
    {
        return PlayerItems.Find("throwable", throwable.name) || button != null;
    }

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
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public int Throw()
    {
        GameObject newThrowable = Instantiate(gameObject, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
        newThrowable.SetActive(true);
        newThrowable.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwable.throwingForce, ForceMode.Impulse);

        PlayerItems.Unequip("throwable", throwable.name);
        TimeManager.ChangeTime(throwable.costAmount, throwable.costType, TimeManager.Operations.subtract);
        return PlayerItems.GetThrowableCount(throwable.name);
    }

    public bool CompareName(string throwableName)
    {
        return throwable.name == throwableName;
    }
}
