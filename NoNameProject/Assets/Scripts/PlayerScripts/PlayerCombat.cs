using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlayerScripts
{
    public class PlayerCombat : MonoBehaviour
    {
        private int weaponId;
        private int weaponsTotal;
        private bool attack;
        
        [SerializeField] public int attackDamage;
        public int defaultDamage;

        public Transform attackPoint;
        public float attackRange = 0.5f;
        public LayerMask enemyLayers;

        private float attackRateMelee = 4f;
        private float attackRateDistance = 4f;
        private float nextAttackTime = 0f;
        private float nextDistanceAttackTime = 0f;
        
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
        
        
        private readonly List<WeaponSO> weapons = new List<WeaponSO>();
        
        [SerializeField] private SpriteRenderer rightHandWeapon;
        private WeaponSO currentWeapon;

        [SerializeField] private Transform sphere;
        

        void Start()
        {
            weapons.Add(swordBasic);
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
                    nextAttackTime = Time.time + 1f / attackRateMelee;
                    CheckIfCriticalHit();
                    
                    Attack();
                }
            }
            
            if (Time.time >= nextDistanceAttackTime)
            {
                if (Input.GetButtonDown("DistanceAttack"))
                {
                    nextDistanceAttackTime = Time.time + 1f / attackRateDistance;
                    CheckIfCriticalHit();
                
                    DistanceAttack();
                }
            }
            
            if (Input.GetButtonDown("Weapon"))
            {
                ChangeAttackWeapon();
                ChangeWeaponImage();
            }
        }

        private void CheckIfCriticalHit()
        {
            hitTime = Time.time;
            float hitDelay = hitTime - bitTime;
            if ( hitDelay is <= 0.3f or >= 1.72f)
            {
                SetCrit(true);
            }
        }

        private void DistanceAttack()
        {
            Transform sphereTransform = Instantiate(sphere, attackPoint.position, Quaternion.identity);

            var shootDir = transform.localScale.x > 0 ? Vector3.left : Vector3.right;
            sphereTransform.GetComponent<Sphere>().Setup(shootDir, isCriticalHit);
            SetCrit(false);
        }

        private void Attack()
        {
            transform.GetComponent<AnimationController>().SetAttackAnimation();

            GetEnemiesHit();
            SetCrit(false);
        }

        private void ChangeAttackWeapon()
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

        private void ChangeWeaponImage()
        {
            SetCurrentWeapon(weapons[weaponId]);
        }

        private void SetCurrentWeapon(WeaponSO weaponSo)
        {
            currentWeapon = weaponSo;
            attackDamage = currentWeapon.damage;
            defaultDamage = attackDamage;
            rightHandWeapon.sprite = currentWeapon.sprite;
            weaponImage.sprite = currentWeapon.sprite;
        }

        public void AddWeapon(WeaponSO weaponSo)
        {
            weapons.Add(weaponSo);
            weaponsTotal++;
        }

        private void GetEnemiesHit()
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                _enemy = enemy.GetComponent<Enemy>();
                if (!_enemy.IsDead() && !enemiesHit.Contains(enemy.name))
                {
                    _enemy.TakeDamage(attackDamage, isCriticalHit);
                    enemiesHit.Add(enemy.name);
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
