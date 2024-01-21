using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]

    public void OnPlayerMovement(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();
        animator.SetBool("isWalking", movement.magnitude > 0);
    }
}