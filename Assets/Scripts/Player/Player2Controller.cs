using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : PlayerController
{
    // Start is called before the first frame update
    void Start()
    {
        controls.Player2.Move.performed += ctx => Move(ctx);
        controls.Player2.Move.canceled += ctx => Move(ctx);

        jumpsRemaining = jumpsAllowed;
    }
}
