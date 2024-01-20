using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;

    private Vector2 moveVector;
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * (Vector3)moveVector;
    }
}
