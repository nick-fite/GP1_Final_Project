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
    [SerializeField] float initialHeight = 1.8f;
    [SerializeField] float crouchingHeight = 0.8f;
    WallChosen chosenWall = WallChosen.None;

    Animator animator;

    bool WallRunSection = false;
    [SerializeField]bool jumped = false;
    bool grounded;
    private bool Move = true;

    Vector3 playerVelocity;

    CharacterController playerController;

    [SerializeField] LayerMask GroundLayers;

    PlayerInputActions playerInput;

    private void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Movement.Enable();

        playerController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        grounded = playerController.isGrounded;
        SetWallChosen();
        ProcessMovement();
        if (playerController.isGrounded) { jumped = false; }
    }

    private void ProcessMovement()
    {
        if (WallRunSection && jumped)
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

        playerVelocity.y += Time.deltaTime * gravity;
        
        if (Move)
        {
            Vector2 inputVector = playerInput.Movement.Move.ReadValue<Vector2>();
            Vector3 MovementDirection = new Vector3(inputVector.x, 0, inputVector.y);
            playerController.Move(Time.deltaTime * transform.TransformDirection(MovementDirection) * speed);


            playerController.Move(Vector3.forward * speed * Time.deltaTime);
            playerController.Move(playerVelocity * Time.deltaTime);

        }
    }



    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                jumped = true;
                animator.SetTrigger("Jump");
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
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
                playerController.center = new Vector3(playerController.center.x, playerController.center.y - 0.5f, playerController.center.z);
                animator.SetBool("Crouch", true);
            }
            if (context.canceled)
            {
                playerController.height = initialHeight;
                playerController.center = new Vector3(playerController.center.x, playerController.center.y + 0.5f, playerController.center.z);
                animator.SetBool("Crouch", false);
            }
        }
    }

    public void SetWallChosen() 
    {
        Ray rayLeft = new Ray(new Vector3(transform.position.x, transform.position.y + playerController.height, transform.position.z), -Vector3.right);
        Ray rayRight = new Ray(new Vector3(transform.position.x, transform.position.y + playerController.height, transform.position.z), Vector3.right);

        Debug.DrawRay(rayLeft.origin, rayRight.direction);

        if (Physics.Raycast(rayLeft, out RaycastHit hitLeft, 5, LayerMask.GetMask("Wall"))) { chosenWall = WallChosen.Left; }
        else if (Physics.Raycast(rayRight, out RaycastHit hitRight, 5, LayerMask.GetMask("Wall"))) { chosenWall = WallChosen.Right; }
        else { chosenWall = WallChosen.None; }
    }

    public void SetWallRunSection(bool newValue)
    {
        WallRunSection = newValue;

        if (!newValue)
        {
            //WallRunning = false;
            animator.SetBool("WallRunning", false);
        }
        else
        {
            Debug.Log("wallrunsection");
            if (chosenWall == WallChosen.Left) {
                playerController.center = new Vector3(playerController.center.x, playerController.center.y, playerController.center.z);
                animator.SetBool("WallRunning", true);
                animator.SetBool("WallRunRight", false);
            }
            if (chosenWall == WallChosen.Right) { 
                playerController.center = new Vector3(playerController.center.x, playerController.center.y, playerController.center.z);
                animator.SetBool("WallRunning", true);
                animator.SetBool("WallRunRight", true);
            }
        }
    }

    public void SetMove(bool newState) { Move = newState; }
}
public enum WallChosen { Left, Right, None }
