using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int damage;
    public int maxHealth;
    
    public bool isArmor;
    public string armorType;
    public float armorRate;
}
