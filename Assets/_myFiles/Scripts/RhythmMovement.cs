using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class RhythmMovement : MonoBehaviour
{
    [SerializeField] float speed = 50;
    [SerializeField] float jumpHeight = 30f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float initialHeight = 1.8f;
    [SerializeField] float crouchingHeight = 0.8f;
    
    [SerializeField] WallChosen chosenWall = WallChosen.None;
    GameObject wallObj;

    Animator animator;

    bool WallRunSection = false;
    [SerializeField] bool jumped = false;
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
        ProcessMovement();
        SetWallChosen();
        if (playerController.isGrounded) { jumped = false; animator.SetBool("Grounded", true); }
    }

    private void ProcessMovement()
    {
        playerVelocity.y += Time.deltaTime * gravity;
        if (WallRunSection && jumped)
        {
            animator.SetBool("WallRunning", true);
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
        }

        if (Move)
        {
            Vector2 inputVector = playerInput.Movement.Move.ReadValue<Vector2>();
            Vector3 movementDirection = new Vector3(inputVector.x, 0, inputVector.y);
            
            playerController.Move(Time.deltaTime * transform.TransformDirection(movementDirection) * speed);

            Vector3 controllerRelativeToParent = transform.parent.InverseTransformPoint(playerController.transform.position);


            if (controllerRelativeToParent.x > 2.81f || controllerRelativeToParent.x < -2f)
            {
                Debug.Log("CLAMPING");
                playerController.Move(Time.deltaTime * -transform.TransformDirection(movementDirection) * speed);
            }


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
                animator.SetBool("Grounded", false);
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
        }
    }

    public void CrouchInput(InputAction.CallbackContext context)
    {
        if (!WallRunSection)
        {
            if (context.performed)
            {
                playerController.height = crouchingHeight;
                playerController.center = new Vector3(playerController.center.x, playerController.center.y - 0.5f, playerController.center.z);
                StartCoroutine(TransitionToCrawl());
            }
            if (context.canceled)
            {
                playerController.height = initialHeight;
                playerController.center = new Vector3(playerController.center.x, playerController.center.y + 0.5f, playerController.center.z);
                StartCoroutine(TransitionToRun());
            }
        }
    }

    public void SetWallChosen() 
    {
        Ray rayLeft = new Ray(new Vector3(transform.position.x, transform.position.y + playerController.height, transform.position.z), -Vector3.right);
        Ray rayRight = new Ray(new Vector3(transform.position.x, transform.position.y + playerController.height, transform.position.z), Vector3.right);

        Debug.DrawRay(rayLeft.origin, rayRight.direction);

        if (Physics.Raycast(rayLeft, out RaycastHit hitLeft, 2.5f, LayerMask.GetMask("Wall"))) { chosenWall = WallChosen.Left; }
        else if (Physics.Raycast(rayRight, out RaycastHit hitRight, 2.5f, LayerMask.GetMask("Wall"))) { chosenWall = WallChosen.Right; }
        else { chosenWall = WallChosen.None; }
    }

    public void SetWallRunSection(bool newValue)
    {
        WallRunSection = newValue;

        if (newValue)
        {
            Debug.Log("wallrunsection");
            if (chosenWall == WallChosen.Left)
            {
                playerController.center = new Vector3(playerController.center.x - 0.5f, playerController.center.y, playerController.center.z);
                animator.SetBool("WallRunRight", false);
            }
            if (chosenWall == WallChosen.Right)
            {
                playerController.center = new Vector3(playerController.center.x + 0.5f, playerController.center.y, playerController.center.z);
                animator.SetBool("WallRunRight", true);
            }
        }
        else
        {
            animator.SetBool("WallRunning", false);

            if (chosenWall == WallChosen.Left)
            {
                playerController.center = new Vector3(playerController.center.x + 0.5f, playerController.center.y, playerController.center.z);
            }
            if (chosenWall == WallChosen.Right)
            {
                playerController.center = new Vector3(playerController.center.x - 0.5f, playerController.center.y, playerController.center.z);
            }
        }
    }

    IEnumerator TransitionToCrawl()
    {
        float rate = 1.0f / 7.5F * 15;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            animator.SetFloat("RunCrawl", t);
            yield return null;
        }
    }

    IEnumerator TransitionToRun()
    {
        float rate = 1.0f / 7.5F * 15;
        float t = 1.0f;
        while (t > 0.0f)
        {
            t -= Time.deltaTime * rate;
            animator.SetFloat("RunCrawl", t);
            yield return null;
        }
    }

    public void SetMove(bool newState) { Move = newState; }
}
public enum WallChosen { Left, Right, None }
