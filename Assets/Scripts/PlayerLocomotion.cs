using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;
    }

    public void Move(Vector2 input)
    {
        Vector3 direction = Vector3.zero;
        direction.x = input.x;
        direction.z = input.y;
        characterController.Move(transform.TransformDirection(direction) * speed * Time.deltaTime);

        //Adding gravity
        velocity += Physics.gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        characterController.Move(velocity * Time.deltaTime);
    }
}
