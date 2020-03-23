using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    private Button button;

    public string buttonType;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        ThrowableActions throwables = FindObjectOfType(typeof(ThrowableActions)) as ThrowableActions;
        button.onClick.AddListener(delegate { throwables.Activate(buttonType); });
    }
}
