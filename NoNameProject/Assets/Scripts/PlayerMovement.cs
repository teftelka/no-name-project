using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller2D;
    [SerializeField]private float runSpeed = 40f;
    private bool jump;
    private float horizontal = 0f;

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller2D.Move(horizontal * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
