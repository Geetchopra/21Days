using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    private float sprintSpeed;
    private float moveSpeed;
    private Vector3 direction;
    private CharacterController controller;
    private Animator animator;
    private bool running;

    private float rotation;
    private Quaternion originalRotation;

    [HideInInspector]
    public enum MovementState { idle = 0, walking = 1, running = 2 };
    [HideInInspector]
    public MovementState state;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        sprintSpeed = walkSpeed * 2.0f;
        animator = GetComponent<Animator>();

        originalRotation = transform.localRotation;
        rotation = 0f;

        state = MovementState.idle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        animator.SetFloat("velocityY", inputY * (running ? 2f : 1f));
        animator.SetFloat("velocityX", inputX * (running ? 2f : 1f));

        float inputOffset = (inputX != 0.0f) ? .7071f : 1.0f;

        direction = new Vector3(inputX * inputOffset, 0, inputY * inputOffset);
        direction = transform.TransformDirection(direction) * moveSpeed;

        controller.Move(direction * Time.fixedDeltaTime);
    }

    void Update()
    {
        running = Input.GetKey(KeyCode.LeftShift);

        if (Mathf.Abs(direction.x) > 0.1f || Mathf.Abs(direction.z) > 0.1f) 
        {
            if (running)
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
            moveSpeed = running ? sprintSpeed : walkSpeed;

        if (MoveCamera.inputEnabled)
        {
            rotation += Input.GetAxis("Mouse X") * MoveCamera.sensH;
            Quaternion angle = Quaternion.AngleAxis(rotation, Vector3.up);
            transform.localRotation = originalRotation * angle;
        }
    }
}
