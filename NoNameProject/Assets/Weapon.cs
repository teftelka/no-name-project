using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSo;


    public WeaponSO GetWeaponSO()
    {
        return weaponSo;
    }
}
