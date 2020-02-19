using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates player movement using a Character Controller.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    private float sprintSpeed;
    private float moveSpeed;
    private Vector3 direction;
    private CharacterController controller;

    /// <summary>
    /// Start - Called before the first frame update.
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();

        //Initialize sprint speed to be twice the walking speed set by the editor.
        sprintSpeed = walkSpeed * 2.0f;
    }

    /// <summary>
    /// Fixed Update - Called once or multiple times every frame.
    /// Checks for key inputs and moves the player in the respective direction relative to the camera.
    /// </summary>
    void FixedUpdate()
    {
        //Set the move speed based on whether the player is sprinting or not.
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        
        //Calculate move direction, update move speed (if needed) and move player based on respective keybind. 
        //Independent if's enable multiple inputs and diagonal movement.

        //Forward
        if (Input.GetKey(KeyCode.W))
        {
            direction = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            controller.Move(direction * Time.deltaTime * moveSpeed);
        }

        //Backward
        if (Input.GetKey(KeyCode.S))
        {
            direction = new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z);
            controller.Move(direction * Time.deltaTime * moveSpeed);
        }

        //Left
        if (Input.GetKey(KeyCode.D))
        {
            direction = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
            moveSpeed /= 2.0f;
            controller.Move(direction * Time.deltaTime * moveSpeed);
        }

        //Right
        if (Input.GetKey(KeyCode.A))
        {
            direction = new Vector3(-Camera.main.transform.right.x, 0, -Camera.main.transform.right.z);
            moveSpeed /= 2.0f;
            controller.Move(direction * Time.deltaTime * moveSpeed);
        }

        //Update player body to be looking in the correct direction.
        if (direction != Vector3.zero)
            transform.forward = direction;
    }
}
