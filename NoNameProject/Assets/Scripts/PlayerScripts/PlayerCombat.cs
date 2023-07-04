using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScripts
{
    public class PlayerCombat : MonoBehaviour
    {
        private int weaponId;
        private int weaponsTotal = 2;
        private bool attack;
        
        [SerializeField] public int attackDamage;
        public int defaultDamage;

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

        private readonly List<string> enemiesHit = new List<string>();

        private Enemy _enemy;
    
        [SerializeField] private Image weaponImage;
        [SerializeField] private WeaponSO swordBasic;
        [SerializeField] private WeaponSO swordRed;
        [SerializeField] private WeaponSO swordVip;

        [SerializeField] private SpriteRenderer rightHandWeapon;
        private WeaponSO currentWeapon;
    
        void Start()
        {
            SetCurrentWeapon(swordBasic);
            weaponId = 0;
        
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
            transform.GetComponent<AnimationController>().SetAttackAnimation(weaponId);
        
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
                attackDamage *=3;
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
            switch (weaponId)
            {
                case 0:
                    SetCurrentWeapon(swordBasic);
                    break;
                case 1:
                    SetCurrentWeapon(swordRed);
                    break;
                case 2:
                    SetCurrentWeapon(swordVip);
                    break;
            }
        }

        private void SetCurrentWeapon(WeaponSO weaponSo)
        {
            currentWeapon = weaponSo;
            attackDamage = currentWeapon.damage;
            defaultDamage = attackDamage;
            rightHandWeapon.sprite = currentWeapon.sprite;
            weaponImage.sprite = currentWeapon.sprite;
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
}
