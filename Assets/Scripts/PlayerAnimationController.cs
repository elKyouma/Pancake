using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void OnPlayerMovement(InputAction.CallbackContext ctx)
    {
        // Add this function to the Player GameObject -> PlayerInput -> Actions -> Player -> Movement -> Interactions -> Add -> PlayerAnimationController -> OnPlayerMovement
        Vector2 movement = ctx.ReadValue<Vector2>();
        Debug.Log(movement.magnitude);
        animator.SetBool("isWalking", movement.magnitude > 0);
    }
}