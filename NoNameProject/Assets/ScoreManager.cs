using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text enemiesText;  
    private int enemiesDied;

    public void AddPoints(int points)
    {
        enemiesDied += points;
        enemiesText.text = "Enemies: " + enemiesDied;
    }
}
