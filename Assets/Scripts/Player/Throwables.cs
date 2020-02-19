using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to the player object. Controls throwable item actions.
/// </summary>
public class Throwables : MonoBehaviour
{
    [SerializeField] private Image reticle;

    //Throwable object to use.
    [SerializeField] private GameObject throwable;

    //Checks if the throwable is equipped using the inventory.
    private bool equipped;

    //Speed at which the throwable is thrown.
    public float speed;

    /// <summary>
    /// Initialize equipped to false.
    /// </summary>
    void Start()
    {
        equipped = false;
    }

    /// <summary>
    /// Activate the throwable item for the player, i.e. enable the reticle.
    /// </summary>
    public void Activate()
    {
        if (PlayerItems.GetListCount("throwables") > 0)
            equipped = true;
    }

    /// <summary>
    /// Deactivate the throwable item, i.e. disable the reticle.
    /// </summary>
    public void Deactivate()
    {
        equipped = false;
    }

    /// <summary>
    /// Update - Called once per frame.
    /// Check if respective keybind is pressed and throw the object.
    /// </summary>
    void Update()
    {
        if (equipped)
        {
            reticle.enabled = true;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Instantiate a new object and throw it in the direction the camera is facing.
                GameObject knife = Instantiate(throwable, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
                knife.SetActive(true);
                knife.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * speed, ForceMode.Impulse);
                Destroy(knife, 5.0f);
            }
        }
        else
        {
            reticle.enabled = false;
        }
    }
}
