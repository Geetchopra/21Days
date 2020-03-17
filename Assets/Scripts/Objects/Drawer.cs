using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drawer : MonoBehaviour
{
    //The prompt Text object.
    private Text prompt;

    //Integer cost amount.
    [SerializeField] private int costAmount = 30;

    //Cost type (h - hours, m - minutes, d - days, s - seconds).
    [SerializeField] private char costType = 'm';

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
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject == gameObject)
            {
                bool state = SetPrompt();

                if (Input.GetKeyDown(KeyCode.E))
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
            prompt.text = "Press E to close the drawer";
            prompt.enabled = true;
            return false;
        }
        else 
        {
            prompt.text = "Press E to open the drawer";
            prompt.enabled = true;
            return true;
        }
    }

    void Open(bool state)
    {
        TimeManager.ChangeTime(costAmount, costType, "subtract");
        animator.SetBool("open", state);
        prompt.enabled = false;
    }
}
