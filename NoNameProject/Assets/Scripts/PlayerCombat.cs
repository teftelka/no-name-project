using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    private int weaponId;
    private int weaponsTotal = 2;
    private bool attack;
    
    private Animator _animator;

    [SerializeField] public int attackDamage;
    public int defaultDamage = 20;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    private float attackRate = 4f;
    private float nextAttackTime = 0f;
    
    [SerializeField] private Text enemiesText;
    private int enemiesDied = 0;
    
    [SerializeField] private GameObject musicGameObject;
    private float bitTime;
    private float hitTime;
    private float newTime;
    private bool isCriticalHit;
    
    List<string> enemiesHit = new List<string>();

    private Enemy _enemy;
    
    [SerializeField] private Image weaponImage;
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite hammerSprite;
    [SerializeField] private Sprite bowSprite;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        
        weaponId = 0;
        attackDamage = 20;
        musicGameObject.GetComponent<MusicBit>().ActionBitHit += GetMusicBit;
    }
    
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack"))
            {
                hitTime = Time.time;
                nextAttackTime = Time.time + 1f / attackRate;

                float hitDelay = hitTime - bitTime;
                if ( hitDelay is <= 0.3f or >= 1.72f)
                {
                    SetCrit(true);
                }
                
                Attack();
                
                //Debug.Log(hitDelay);
            }
        }
        if (Input.GetButtonDown("Weapon"))
        {
            SetAttackWeapon();
            SetWeaponImage();
        }
    }

    private void FixedUpdate()
    {

    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
        _animator.SetInteger("Weapon", weaponId);
        
        GetEnemiesHit();
        SetCrit(false);
    }

    private void SetAttackWeapon()
    {
        weaponId++;
        if (weaponId > weaponsTotal)
        {
            weaponId = 0;
        }
    }

    private void SetCrit(bool isCrit)
    {
        if (isCrit)
        {
            attackDamage = 100;
            isCriticalHit = true;
        }
        else
        {
            attackDamage = defaultDamage;
            isCriticalHit = false;
        }
    }

    private void SetWeaponImage()
    {
        weaponImage.sprite = weaponId switch
        {
            0 => swordSprite,
            1 => hammerSprite,
            _ => bowSprite
        };
    }

    private void GetEnemiesHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            _enemy = enemy.GetComponent<Enemy>();
            if (!_enemy.IsDead && !enemiesHit.Contains(enemy.name))
            {
                _enemy.TakeDamage(attackDamage, isCriticalHit);
                enemiesHit.Add(enemy.name);
                if (_enemy.IsDead)
                {
                    enemiesDied++;
                    enemiesText.text = "Enemies: " + enemiesDied;
                }
            }
        }
        enemiesHit.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
           return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void GetMusicBit(float time)
    {
        bitTime = time;
        
        //Debug.Log(bitTime - time);
    }
}
