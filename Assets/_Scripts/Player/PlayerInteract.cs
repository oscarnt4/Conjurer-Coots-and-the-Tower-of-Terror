using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float interactibleDistance = 3f;
    [SerializeField] LayerMask mask;

    private Camera _camera;
    private PlayerUI playerUI;
    private InputManager inputManager;
    private PlayerAttack playerAttack;
    private Interactable interactable;

    void Awake()
    {
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        _camera = Camera.main;
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactibleDistance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, interactibleDistance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                playerAttack.SetActive(false);
                if (inputManager.defaultActions.Interact.IsPressed())
                {
                    interactable.BaseInteract();
                }
                else
                {
                    interactable.BaseReleaseInteract();
                    interactable = null;
                }
            }
            else
            {
                playerUI.UpdateText("");
                playerAttack.SetActive(true);
                if (interactable != null)
                {
                    interactable.BaseReleaseInteract();
                    interactable = null;
                }
            }
        }
        else
        {
            playerUI.UpdateText("");
            playerAttack.SetActive(true);
            if (interactable != null)
            {
                interactable.BaseReleaseInteract();
                interactable = null;
            }
        }
    }
}
