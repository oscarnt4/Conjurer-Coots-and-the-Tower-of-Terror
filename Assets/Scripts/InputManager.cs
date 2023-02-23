using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerLocomotion locomotion;
    private PlayerLook look;
    [SerializeField] GameController gameController;

    public PlayerInput.DefaultActions defaultActions;

    void Awake()
    {
        playerInput = new PlayerInput();
        defaultActions = playerInput.Default;

        locomotion = GetComponent<PlayerLocomotion>();
        look = GetComponent<PlayerLook>();
    }

    void Update()
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
        defaultActions.Cheat.performed += CheatButton;
    }

    private void OnDisable()
    {
        defaultActions.Disable();
        defaultActions.Cheat.performed -= CheatButton;
    }

    private void CheatButton(InputAction.CallbackContext obj)
    {
        gameController.Cheat();
    }
}
