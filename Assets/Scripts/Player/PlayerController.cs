using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Controls player movements and animations.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    private float sprintSpeed;
    private float moveSpeed;

    //Direction to move the player in.
    private Vector3 direction;
    private float rotation;
    private Quaternion originalRotation;

    private CharacterController controller;
    private Animator animator;

    [HideInInspector]
    public enum MovementState { idle = 0, walking = 1, running = 2 };
    [HideInInspector]
    public MovementState state;

    private float inputX;
    private float inputY;

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
        sprintSpeed = walkSpeed * 2.0f;
        animator = GetComponent<Animator>();

        originalRotation = transform.localRotation;
        rotation = 0f;

        state = MovementState.idle;
    }

    /// <summary>
    /// Called once or multiple times every frame.
    /// </summary>
    void FixedUpdate()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        animator.SetFloat("velocityY", inputY * (state == MovementState.running ? 2f : 1f));
        animator.SetFloat("velocityX", inputX * (state == MovementState.running ? 2f : 1f));

        //Restrict movement if there is some sideways moevement.
        float inputOffset = (inputX != 0.0f) ? 0.5f : 1.0f;

        direction = new Vector3(inputX * inputOffset, 0, inputY * inputOffset);
        direction = transform.TransformDirection(direction) * moveSpeed;

        controller.Move(direction * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Called once every frame.
    /// </summary>
    void Update()
    {
        if (Mathf.Abs(direction.x) > 0.1f || Mathf.Abs(direction.z) > 0.1f) 
        {
            if (Input.GetKey(KeyCode.LeftShift) && inputY > 0.1f)
                state = MovementState.running;
            else
                state = MovementState.walking;
        }
        else
        {
            state = MovementState.idle;
        }

        bool crouched = animator.GetBool("crouch");

        //Reduce movement speed for crouching.
        if (crouched)
            moveSpeed /= 2.0f;

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            animator.SetBool("crouch", !crouched);
        else
            moveSpeed = state == MovementState.running ? sprintSpeed : walkSpeed;

        //Rotate player on the X axis as per mouse movement.
        if (MoveCamera.inputEnabled)
        {
            rotation += Input.GetAxis("Mouse X") * MoveCamera.sensH;
            Quaternion angle = Quaternion.AngleAxis(rotation, Vector3.up);
            transform.localRotation = originalRotation * angle;
        }
    }
}
