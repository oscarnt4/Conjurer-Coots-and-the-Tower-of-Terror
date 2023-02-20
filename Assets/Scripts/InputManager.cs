using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerLocomotion locomotion;
    private PlayerLook look;
    
    public PlayerInput.DefaultActions defaultActions;

    void Awake()
    {
        playerInput = new PlayerInput();
        defaultActions = playerInput.Default;

        locomotion = GetComponent<PlayerLocomotion>();
        look = GetComponent<PlayerLook>();
    }

    void FixedUpdate()
    {
        locomotion.Move(defaultActions.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.Look(defaultActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        defaultActions.Enable();
    }

    private void OnDisable()
    {
        defaultActions.Disable();
    }
}
