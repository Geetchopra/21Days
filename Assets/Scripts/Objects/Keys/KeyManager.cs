using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
    }

    public bool Discovered()
    {
        return image != null;
    }

    public void CreateImage()
    {
        image = Instantiate(key.image);
        image.transform.parent = GameObject.Find("Inventory/Keys").transform;
    }

    public void SetImageActive(bool displayState)
    {
        if (image != null)
            image.enabled = displayState;
    }

    // Update is called once per frame
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

    void PickUp()
    {
        TimeManager.ChangeTime(key.costAmount, key.costType, TimeManager.Operations.subtract);
        PlayerItems.Equip("key", ID);
        Destroy(gameObject);
    }
}
