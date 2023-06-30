using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public CharacterController2D controller2D;
    private int weaponId;
    private int weaponsTotal = 2;
    private bool attack;

    [SerializeField] public int attackDamage;
    public int defaultDamage = 20;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    
    [SerializeField] private Text enemiesText;
    private int enemiesDied = 0;
    
    [SerializeField] private GameObject musicGameObject;
    private float bitTime;
    private float hitTime;
    private float newTime;
    private bool isCriticalHit;
    
    [SerializeField] private Image weaponImage;
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite hammerSprite;
    [SerializeField] private Sprite bowSprite;

    void Start()
    {
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
                attack = true;
                //исправить
                nextAttackTime = Time.time + 1f / attackRate;
                
                
                float hitDelay = hitTime - bitTime;
                if ( hitDelay is <= 0.3f or >= 1.72f)
                {
                    attackDamage = 100;
                    isCriticalHit = true;
                }
                
                Debug.Log(hitDelay);
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
        if (attack)
        {
            controller2D.Attack(weaponId);
            Attack();
            attackDamage = defaultDamage;
            isCriticalHit = false;
            attack = false;
        }
    }
    
    private void SetAttackWeapon()
    {
        weaponId++;
        if (weaponId > weaponsTotal)
        {
            weaponId = 0;
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

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, isCriticalHit);
            if (enemy.GetComponent<Enemy>().IsDead)
            {
                enemiesDied++;
                enemiesText.text = "Enemies: " + enemiesDied;
            }
        }
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
