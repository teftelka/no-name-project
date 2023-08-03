using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private Transform bar;
    private Transform armorBar;
    
    private void Awake()
    {
        bar = transform.Find("Bar");
        armorBar = transform.Find("ArmorBar");
    }

    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }
    
    public void SetArmorSize(float sizeNormalized)
    {
        armorBar.localScale = new Vector3(sizeNormalized, 1f);
    }
    
}
