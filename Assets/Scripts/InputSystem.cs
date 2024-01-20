using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    Inputs inputs;
    // Start is called before the first frame update
    void Start()
    {
        inputs = new Inputs();
        inputs.Player.Movement.performed += (ctx) => OnInput(ctx); 
    }

    private void OnInput(InputAction.CallbackContext ctx)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
