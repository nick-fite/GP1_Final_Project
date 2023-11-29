using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class RhythmMovement : MonoBehaviour
{
    [SerializeField] float speed = 50;
    [SerializeField] float jumpHeight = 30f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float defualtTimerValue = 0.5f;
    [SerializeField] float initialHeight = 1.8f;
    [SerializeField] float crouchingHeight = 0.8f;
    [SerializeField] IKFootSolver[] footSolvers;
    float timer;
    WallChosen chosenWall = WallChosen.None;

    [SerializeField] bool WallRunSection = false;
    bool grounded;
    private bool Move = true;

    Vector3 playerVelocity;

    CharacterController playerController;

    [SerializeField] LayerMask GroundLayers;

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
        if (!WallRunSection)
        {
            if (transform.localPosition.x < -0.4f)
            {
                playerVelocity.x = 10f;
            }
            else if (transform.localPosition.x > 0.4f)
            {
                playerVelocity.x = -10f;
            }
            else { playerVelocity.x = 0f; }
            playerVelocity.y += Time.deltaTime * gravity;
        }
        else
        {
            if (timer > 0)
            {
                if (chosenWall == WallChosen.Left)
                {
                    playerVelocity.y = 3f;
                    playerVelocity.x = -10f;
                }
                else if (chosenWall == WallChosen.Right)
                {
                    playerVelocity.y = 3f;
                    playerVelocity.x = 10f;
                }
            }
            else
            {
                playerVelocity.x = 0f;
                playerVelocity.y += Time.deltaTime * gravity;
            }
        }
        if (Move)
        {
            playerController.Move(Vector3.forward * speed * Time.deltaTime);
            playerController.Move(playerVelocity * Time.deltaTime);

        }
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

    public void WallRunLeft()
    {
        if (WallRunSection && (chosenWall == WallChosen.Left || chosenWall == WallChosen.None))
        {
            foreach (IKFootSolver foot in footSolvers)
            {
                foot.SetChosenWall(WallChosen.Left);
            }
            chosenWall = WallChosen.Left;
            timer = 0.25f;
        }
    }

    public void WallRunRight()
    {
        if (WallRunSection && (chosenWall == WallChosen.Right || chosenWall == WallChosen.None))
        {
            foreach (IKFootSolver foot in footSolvers)
            {
                foot.SetChosenWall(WallChosen.Right);
            }
            chosenWall = WallChosen.Right;
            timer = 0.25f;
        }
    }

    /*
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
    }*/

    public void CrouchInput(InputAction.CallbackContext context)
    {
        if (!WallRunSection)
        {
            if (context.performed)
            {
                playerController.height = crouchingHeight;
            }
            if (context.canceled)
            {
                playerController.height = initialHeight;
            }
        }
    }

    public void SetWallRunSection(bool newValue)
    {
        chosenWall = WallChosen.None;
        timer = 0f;
        WallRunSection = newValue;
        foreach (IKFootSolver foot in footSolvers)
        {
            foot.SetWallRunning(newValue);
        }
    }

    public void SetMove(bool newState) { Move = newState; }
}
public enum WallChosen { Left, Right, None }
