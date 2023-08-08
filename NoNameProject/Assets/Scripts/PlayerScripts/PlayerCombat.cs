using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScripts
{
    public class PlayerCombat : MonoBehaviour
    {
        private int weaponId;
        private int weaponsTotal;
        private bool attack;
        
        private int attackDamage;
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
        private WeaponSO currentWeaponSO;

        [SerializeField] private Transform sphere;
        

        void Start()
        {
            weapons.Add(swordBasic);
            SetCurrentWeapon(swordBasic);
            weaponId = 0;
        
            musicGameObject.GetComponent<MusicBit>().ActionBitHit += GetMusicBit;
        }
        
        public void HandleMeleeAttack()
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + 1f / attackRateMelee;
                CheckIfCriticalHit();
                Attack();
            }
        }
        
        public void HandleDistanceAttack()
        { 
            if (Time.time >= nextDistanceAttackTime)
            {
                nextDistanceAttackTime = Time.time + 1f / attackRateDistance;
                CheckIfCriticalHit();
                
                DistanceAttack();
            }
        }
        
        public void HandleWeaponChange()
        {
            ChangeAttackWeapon();
            ChangeWeaponImage();
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
            attackDamage = isCrit ? defaultDamage * 3 : defaultDamage;
            isCriticalHit = isCrit;
        }

        private void ChangeWeaponImage()
        {
            SetCurrentWeapon(weapons[weaponId]);
        }

        private void SetCurrentWeapon(WeaponSO weaponSo)
        {
            currentWeaponSO = weaponSo;
            attackDamage = currentWeaponSO.damage;
            defaultDamage = attackDamage;
            rightHandWeapon.sprite = currentWeaponSO.sprite;
            weaponImage.sprite = currentWeaponSO.sprite;
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
                    _enemy.TakeDamage(attackDamage, isCriticalHit, true);
                    enemiesHit.Add(enemy.name);
                }
            }
            enemiesHit.Clear();
        }
        
        
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        private void GetMusicBit(float time)
        {
            bitTime = time;
        
            //Debug.Log(bitTime - time);
        }
    }
}
