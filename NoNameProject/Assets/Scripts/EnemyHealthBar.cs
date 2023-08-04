using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnemyHealthBar : MonoBehaviour
{
    private Transform bar;
    private Transform armorBar;
    private SpriteRenderer armorBarSprite;
    
    private void Awake()
    {
        bar = transform.Find("Bar");
        armorBar = transform.Find("ArmorBar");
        armorBarSprite = armorBar.GetComponentInChildren<SpriteRenderer>();
    }

    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }
    
    public void SetArmorSize(float sizeNormalized)
    {
        armorBar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetArmorColor(string armorType)
    {
        if (armorType == "melee")
        {
            armorBarSprite.color = UtilsClass.GetColorFromString("DDD357");
        }
        else
        {
            armorBarSprite.color = UtilsClass.GetColorFromString("9C07FF");
        }
    }
    
}
