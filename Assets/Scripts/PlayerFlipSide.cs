using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerFlipSide : MonoBehaviour
{
    public void OnMouseMovement(InputAction.CallbackContext ctx)
    // Add this function to the Player GameObject -> PlayerInput -> Actions -> Player -> Movement -> Interactions -> Add -> PlayerFlipSide -> OnMouseMovement
    // GRaphics GameObject is outdated. Should be removed.
    {
        if (Camera.main)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
            Vector2 playerPos = transform.position;
            Vector2 dir = mousePos - playerPos;
            dir.Normalize();
            if (dir.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
