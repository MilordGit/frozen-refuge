using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    [Header("Movement")]
    CharacterController controller;

    Vector3 playerVelocity;
    public float playerSpeed = 10f;
    public float sprintMultiplier = 1.8f;

    [Header("Jump")]
    public float jumpForce = 3f;
    public float gravity = -50;
    
    public Transform groundCheck;
    public LayerMask groundLayerMask;
    bool isGrounded;

    [Header("Camera")]
    public Transform neck;
    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;

    private void Start()
    {
        //gets character controller for movement
        controller = GetComponent<CharacterController>();

        //locks the cursor to the middle of the screen and makes it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateMouse();
        UpdateMovement();
    }

    void UpdateMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;

        neck.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        controller.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void UpdateMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayerMask);

        if (isGrounded == true && playerVelocity.y <0)
        {
            playerVelocity.y = 0f;
        }

        //gets movement input
        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        //uses movement input to control transform
        Vector3 move = neck.transform.right * movementX + controller.transform.forward * movementZ;

        //character controller uses movement
        controller.Move(move * playerSpeed * Time.deltaTime);

        //check if player is on ground and jump input is pressed
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}