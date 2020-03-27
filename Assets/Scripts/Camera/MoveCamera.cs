using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls camera movement for the player.
/// Only moves camera on its Y axis. X axis camera movement is 
/// controlled by PlayerController by rotating the player accordingly.
/// </summary>
public class MoveCamera : MonoBehaviour
{
    public static float sensH = 2.0f;
    public static float sensV = 2.0f;

    public static bool inputEnabled;
    private float rotation;

    private Quaternion originalRotation;

    public Light areaLight;

    /// <summary>
    /// Initialize private attributes.
    /// </summary>
    void Start()
    {
        inputEnabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        originalRotation = transform.localRotation;
        rotation = 0f;
    }

    /// <summary>
    /// Enable camera movement when the mouse moves. Also supports changing the sensitivity of the camera.
    /// </summary>
    void Update()
    {
        if (inputEnabled)
        {
            rotation += Input.GetAxis("Mouse Y") * sensV;
            rotation = Mathf.Clamp(rotation, -60f, 60f);

            Quaternion angle = Quaternion.AngleAxis(rotation, Vector3.left);
            transform.localRotation = originalRotation * angle;
            areaLight.transform.localRotation = transform.localRotation;
        }       
    }

    /// <summary>
    /// Disable camera movement. Used when UI menus like the Inventory are being used.
    /// </summary>
    public static void DisableInput()
    {
        inputEnabled = false;
    }

    /// <summary>
    /// Enable camera movement. Generally called after DisableInput() to re-enable camera movement.
    /// </summary>
    public static void EnableInput()
    {
        inputEnabled = true;
    }
}
