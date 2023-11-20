using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class RhythmMovement : MonoBehaviour
{
    enum WallRunMode { Left, Right, None }
    [SerializeField] float speed = 50;
    [SerializeField] float jumpHeight = 30f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float defualtTimerValue = 0.5f;
    float timer;

    bool movedLeft = false;
    bool movedRight = true;
    [SerializeField] bool WallRunSection = false;
    bool grounded;

    Vector3 playerVelocity;

    CharacterController playerController;

    [SerializeField] LayerMask GroundLayers;

    WallRunMode ChosenWall = WallRunMode.None;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        timer = defualtTimerValue;
    }

    private void Update()
    {
        grounded = playerController.isGrounded;
        ProcessMovement();

        if (WallRunSection)
        {
            RunTimer();
        }
    }

    private void ProcessMovement()
    {
        playerVelocity.y += Time.deltaTime * gravity;
        playerController.Move(playerVelocity * Time.deltaTime);
    }

    private void RunTimer()
    {
        timer -= Time.deltaTime;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
        }
    }

    public void MoveLeftFoot()
    {
        if (!WallRunSection && !movedLeft) 
        {
            GameManager.m_Instance.GetMusicManager().PlaySound();
            playerController.Move(Vector3.forward * speed * Time.deltaTime);

            movedLeft = true;
            movedRight = false;
        }
        if (WallRunSection)
        {
            if (ChosenWall == WallRunMode.None)
            {
                ChosenWall = WallRunMode.Left;
            }
            if (ChosenWall == WallRunMode.Left)
            {
                playerController.Move(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }

    public void MoveRightFoot()
    {
        if (!WallRunSection && !movedRight)
        {
            GameManager.m_Instance.GetMusicManager().PlaySound();
            playerController.Move(Vector3.forward * speed * Time.deltaTime);

            movedLeft = false;
            movedRight = true;
        }

        if (WallRunSection)
        {
            if (ChosenWall == WallRunMode.None)
            {
                ChosenWall = WallRunMode.Right;
            }
            if (ChosenWall == WallRunMode.Right)
            {
                playerController.Move(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }

    public void CrouchInput(InputAction.CallbackContext context)
    {
        if (!WallRunSection)
        {
            if (context.performed)
            {
                playerController.height = playerController.height - 0.5f;
            }
            if (context.canceled)
            {
                playerController.height = playerController.height + 0.5f;
            }
        }
    }

    public void SetWallRunSection(bool newValue) { WallRunSection = newValue; }
}
