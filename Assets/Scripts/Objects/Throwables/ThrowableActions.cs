using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowableActions : MonoBehaviour
{
    private Text text;

    private ThrowableManager currentThrowable;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Reticle reticle;

    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("Weapon Text").GetComponent<Text>();
        activated = false;
    }

    public void Activate(string name)
    {
        activated = true;
        text.text = name + ": " + PlayerItems.GetThrowableCount(name);
        currentThrowable = inventory.GetThrowable(name);
    }

    public void Deactivate()
    {
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && !Input.GetKey(KeyCode.N))
        {
            SetActiveUI(true);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                int remaining = currentThrowable.Throw();
                text.text = currentThrowable.Name + ": " + remaining;

                if (remaining == 0)
                {
                    string newType = PlayerItems.GetNextThrowable();

                    if (newType == null)
                    {
                        //Unequip if no throwables available.
                        activated = false;
                    }
                    else
                    {
                        currentThrowable = inventory.GetThrowable(newType);
                        text.text = newType + ": " + PlayerItems.GetThrowableCount(newType);
                    }
                }
            }
        }
        else
        {
            SetActiveUI(false);
        }
    }

    void SetActiveUI(bool displayState)
    {
        text.gameObject.SetActive(displayState);
        reticle.SetActiveCrosshair(displayState);
    }
}
