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

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        sprintSpeed = walkSpeed * 2.0f;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Set the move speed based on whether the player is sprinting or not.
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        animator.SetFloat("velocityY", Input.GetAxis("Vertical") * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));
        animator.SetFloat("velocityX", Input.GetAxis("Horizontal") * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));

        //Independent if's enable multiple inputs and diagonal movement.
        float offset;

        if (Input.GetKey(KeyCode.W))
        {
            direction = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            offset = Mathf.Clamp(Math.Abs(animator.GetFloat("velocityY")), -1, 1);
            controller.Move(direction * Time.deltaTime * moveSpeed * offset);
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction = new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z);
            offset = Mathf.Clamp(Math.Abs(animator.GetFloat("velocityY")), -1, 1);
            controller.Move(direction * Time.deltaTime * moveSpeed * offset);
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
            moveSpeed /= 2.0f;
            offset = Mathf.Clamp(Math.Abs(animator.GetFloat("velocityX")), -1, 1);
            controller.Move(direction * Time.deltaTime * moveSpeed * offset);
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction = new Vector3(-Camera.main.transform.right.x, 0, -Camera.main.transform.right.z);
            moveSpeed /= 2.0f;
            offset = Mathf.Clamp(Math.Abs(animator.GetFloat("velocityX")), -1, 1);
            controller.Move(direction * Time.deltaTime * moveSpeed * offset);
        }

        transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
    }
}
