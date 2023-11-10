using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private float motorSpeed = 5f;
    private bool isGrounded;
    private float gravity = -9.8f;
    [SerializeField] private float jumpHight = 3f;
    // Start is called before the first frame update
    void Start()
    {
       characterController = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
    }
    //recives input from InputManager.cs and apply to charchtercontroller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        characterController.Move(transform.TransformDirection(moveDir) * motorSpeed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        characterController.Move(playerVelocity * Time.deltaTime);
        

    }

    public void Jump() { 
    if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHight * -3.0f * gravity);
        }
    }
}
