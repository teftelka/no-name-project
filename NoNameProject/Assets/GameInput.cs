using System;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameInput : MonoBehaviour
{
    private List<KeyCode> inputBuffer = new List<KeyCode>();
    private List<KeyCode> playerInput = new List<KeyCode>();

    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;
    private bool isDistanceAttack;

    private AttackComboVisual attackComboVisual;

    [SerializeField] private SpellSO[] spellSo;
    private List<Sprite> comboImages;
    private int currentSpell;

    private void Start()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        attackComboVisual = FindObjectOfType<AttackComboVisual>();
        
        if (playerCombat == null)
        {
            Debug.LogWarning("PlayerCombat is null!");
        }
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

    private void SetNextSpell()
    {
        Random rnd = new Random();
        currentSpell  = rnd.Next(spellSo.Length);
        
        inputBuffer = spellSo[currentSpell].keyCodes;
        comboImages = spellSo[currentSpell].arrowSprites;
        attackComboVisual.SetAttackCombo(comboImages);
    }

    private void HandleAttackCombination()
    {
        KeyCode lastButton = GetLastButton();

        if (lastButton != KeyCode.JoystickButton5)
        {
            playerInput.Add(lastButton);

            if (!CheckCombination())
            {
                Time.timeScale = 1;
                Invoke(nameof(Clear), 0.2f);
            }
            
            else if (inputBuffer.Count == playerInput.Count)
            {
                playerCombat.HandleDistanceAttack(spellSo[currentSpell].damage);
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
            SetNextSpell();
            Time.timeScale = 0;
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
                attackComboVisual.SetCorrectArrowColor(i);
                continue;
            }
            attackComboVisual.SetIncorrectColor(i);
            return false;
        }
        
        return true;
    }

    private void Clear()
    {
        isDistanceAttack = false;
        playerInput.Clear();
        attackComboVisual.RemoveAttackCombo();
        Time.timeScale = 1;
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
