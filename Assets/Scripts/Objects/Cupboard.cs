using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cupboard : MonoBehaviour
{
    //The prompt Text object.
    private Text prompt;

    //Integer cost amount.
    [SerializeField] private int costAmount = 30;

    [SerializeField] private TimeManager.Times costType = TimeManager.Times.minutes;

    private Animator animator;

    private float promptDistance = 3f;

    // Start is called before the first frame update
    void Start()
    {
        prompt = GameObject.Find("Interactable Prompt").GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, promptDistance) && !Cursor.visible)
        {
            if (hit.collider.gameObject == gameObject)
            {
                bool state = SetPrompt();

                if (Input.GetKeyDown(KeyCode.E) && prompt.enabled)
                    Open(state);
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

    bool SetPrompt()
    {
        if (animator.GetBool("open"))
        {
            if (CanBeClosed())
            {
                prompt.text = "Press E to close door";
                prompt.enabled = true;
            }
            return false;
        }
        else
        {
            prompt.text = "Press E to open door";
            prompt.enabled = true;
            return true;
        }
    }

    bool CanBeClosed()
    {
        foreach(Transform obj in transform.parent)
        {
            if (obj.name.Contains("Drawer"))
            {
                Animator animator = obj.GetComponentInChildren<Animator>();
                if (animator.GetBool("open"))
                    return false;
            }
        }
        return true;
    }

    void Open(bool state)
    {
        TimeManager.ChangeTime(costAmount, costType, TimeManager.Operations.subtract);
        animator.SetBool("open", state);
        prompt.enabled = false;
    }
}
