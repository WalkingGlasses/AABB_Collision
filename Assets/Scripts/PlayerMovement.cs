using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpForce = 3f;
    public bool isGrounded = true;

    [HideInInspector]
    public Vector2 velocity;

    

    private bool jumpRequested;

    void Update()
    {
        //actual movement and jumping
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) input.x -= 1f;
            if (Keyboard.current.dKey.isPressed) input.x += 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                jumpRequested = true;
            }
        }

        velocity.x = input.normalized.x * speed;
    }
    
    void FixedUpdate()
    {
        //the horrors
        isGrounded = false;
        if (jumpRequested)
        {
            velocity.y = jumpForce;
            jumpRequested = false; 
        }
        else
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
    }
}