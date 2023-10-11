using System;
using System.Collections.Generic;
using System.Linq;
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
    
    private CircleDrawer circleDrawer;
    private Vector3 shootDirection;
    private Vector3 closestEnemy;
    private GameObject lastEnemy;

    private void Start()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        attackComboVisual = FindObjectOfType<AttackComboVisual>();
        
        if (playerCombat == null)
        {
            Debug.LogWarning("PlayerCombat is null!");
        }
        
        if (circleDrawer == null)
        {
            circleDrawer = FindObjectOfType<CircleDrawer>();
        }
    }

    void Update()
    {
        float inputY = Input.GetAxisRaw("Vertical");
        float inputX = Input.GetAxisRaw("Horizontal");
 
        shootDirection = new Vector3(inputX, inputY, 0);
        
        if (Input.anyKeyDown && isDistanceAttack)
        {
            HandleAttackCombination();

        }

        else if (!isDistanceAttack)
        {
            HandlePlayerInput();
        }
        
        if (isDistanceAttack)
        {
            closestEnemy = GetClosestEnemy(GetAllVisibleEnemies(), shootDirection);
            circleDrawer.DrawCircle(closestEnemy);
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
                playerCombat.HandleDistanceAttack(spellSo[currentSpell].damage, closestEnemy); 
                Clear();
            }
        }
        else
        {
            Clear();
        }
    }

    private List<GameObject> GetAllVisibleEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
        List<GameObject> enemiesList = enemies.ToList();

        for (int i = 0; i < enemiesList.Count; i++)
        {
            Vector3 objectScreenPos = Camera.main.WorldToScreenPoint(enemiesList[i].transform.position);
                    
            if (objectScreenPos.x > 0 && objectScreenPos.x < Screen.width &&
                objectScreenPos.y > 0 && objectScreenPos.y < Screen.height &&
                objectScreenPos.z > 0) 
            {
                //Debug.Log("Объект с тегом " + "Enemy" + " виден на экране!");
                // Ваш код обработки видимого объекта
            }
            else
            {
                enemiesList.Remove(enemiesList[i]);
            }
        }

        return enemiesList;
    }

    private Vector3 GetClosestEnemy(List<GameObject> enemies, Vector3 direction)
    {
        if (direction == Vector3.zero && enemies.Contains(lastEnemy))
        {
            return lastEnemy.transform.position;
        }
        
        int counter = 0;
        List <float> angles = new List <float>();

        foreach (var enemy in enemies)
        {
            var angle = Vector3.Angle(direction, enemy.transform.position);
            angles.Add(angle);
        }

        foreach (var angle in angles)
        {
            var min = angles.Min();
            if (angle == min)
            {
                lastEnemy = enemies[counter];
                return lastEnemy.transform.position;
            }
            else
            {
                counter += 1;
            }
        }
        return Vector3.zero;
    }
    
    private void HandlePlayerInput()
    {
        if (Input.GetButtonDown("Attack"))
        {
            playerCombat.HandleMeleeAttack();
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

    public void StartDistanceAttack()
    {
        isDistanceAttack = true;
        SetNextSpell();
        Time.timeScale = 0;
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
        
       Invoke(nameof(RemoveCircle), 0.4f);
    }


    private void RemoveCircle()
    {
        circleDrawer.RemoveCircle();
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
