using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttackComboVisual : MonoBehaviour
{
    private List<GameObject> images = new List<GameObject>();
    
    void Start()
    {
        var slots = transform.childCount;
        for (int i = 0; i < slots; i++)
        {
            var imageObject = transform.GetChild(i).gameObject;
            images.Add(imageObject);
        }
    }


    public void SetAttackCombo(List<Sprite> combo)
    {
        for (int i = 0; i < combo.Count; i++)
        {
            images[i].GetComponent<Image>().sprite = combo[i];
            images[i].SetActive(true);
        }
    }

    public void RemoveAttackCombo()
    {
        foreach (var image in images)
        {
            image.GetComponent<Image>().sprite = null;
            image.GetComponent<Image>().color = Color.white;
            image.SetActive(false);
        }
    }

    public void SetCorrectArrowColor(int arrowIndex)
    {
        images[arrowIndex].GetComponent<Image>().color = Color.green;
    }

    public void SetIncorrectColor(int arrowIndex)
    {
        images[arrowIndex].GetComponent<Image>().color = Color.red;
    }
}
