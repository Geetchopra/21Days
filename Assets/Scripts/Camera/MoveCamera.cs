﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls camera movement for the player.
/// </summary>
public class MoveCamera : MonoBehaviour
{
    public float sensH = 2.0f;
    public float sensV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private static bool inputEnabled;

    /// <summary>
    /// Initialize private attributes.
    /// </summary>
    void Start()
    {
        inputEnabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Enable camera movement when the mouse moves. Also supports changing the sensitivity of the camera.
    /// </summary>
    void Update()
    {
        if (inputEnabled)
        {
            yaw += sensH * Input.GetAxis("Mouse X");
            pitch -= sensV * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
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
    /// Enable camera movement. Generally called after disable_input() to re-enable camera movement.
    /// </summary>
    public static void EnableInput()
    {
        inputEnabled = true;
    }
}
