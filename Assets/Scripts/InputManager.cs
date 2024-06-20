using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerInput.OnFootActions onFootActions;
    public PlayerMotor motor;
    public PlayerLook look;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFootActions = playerInput.onFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        onFootActions.Jump.performed += ctx => motor.Jump();
        onFootActions.Sprint.performed += ctx => motor.Sprint();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //playermotor moves using the value from movment action
        motor.ProcessMove(onFootActions.Movment.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFootActions.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFootActions.Enable();
    }

    private void OnDisable()
    {
        onFootActions.Disable();
    }
}
