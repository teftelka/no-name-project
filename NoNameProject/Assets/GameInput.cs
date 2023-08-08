using System;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private List<KeyCode> inputBuffer = new List<KeyCode>();
    private List<KeyCode> playerInput = new List<KeyCode>();
    
    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;
    private bool isDistanceAttack;

    private void Start()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        if (playerCombat == null)
        {
            Debug.LogWarning("PlayerCombat is null!");
        }
        
        inputBuffer.Add(KeyCode.JoystickButton3);
        inputBuffer.Add(KeyCode.JoystickButton3);
        inputBuffer.Add(KeyCode.JoystickButton0);
        inputBuffer.Add(KeyCode.JoystickButton1);
    }
    
    void Update()
    {
        if (Input.anyKeyDown && isDistanceAttack)
        {
            HandleAttackCombination();
        }
        
        else if (!isDistanceAttack)
        {
            HandlePlayerInput();
        }
    }

    private void HandleAttackCombination()
    {
        KeyCode lastButton = GetLastButton();

        if (lastButton != KeyCode.JoystickButton5)
        {
            playerInput.Add(lastButton);

            if (!CheckCombination())
            {
                Clear();
            }
            
            else if (inputBuffer.Count == playerInput.Count)
            {
                playerCombat.HandleDistanceAttack();
                Clear();
            }
        }
        
        else
        {
            Clear();
        }
    }
    
    private void HandlePlayerInput()
    {
        if (Input.GetButtonDown("Attack"))
        {
            playerCombat.HandleMeleeAttack();
        }

        else if (Input.GetButtonDown("DistanceAttack"))
        {
            isDistanceAttack = true;
        }

        else if (Input.GetButtonDown("Weapon"))
        {
            playerCombat.HandleWeaponChange();
        }

        else if (Input.GetButtonDown("Jump"))
        {
            playerMovement.SetJump();
        }
    }
    
    private bool CheckCombination()
    {
        for (int i = 0; i < playerInput.Count; i++)
        {
            if (playerInput[i] == inputBuffer[i])
            {
                continue;
            }

            return false;
        }
        
        return true;
    }

    private void Clear()
    {
        isDistanceAttack = false;
        playerInput.Clear();
    }

    private KeyCode GetLastButton()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None;
    }
}
