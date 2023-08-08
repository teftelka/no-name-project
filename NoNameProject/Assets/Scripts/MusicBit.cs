using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class MusicBit : MonoBehaviour
{
    [SerializeField] private BackgroundMusicSO music;
    [SerializeField] private Transform bitBar;
    private AudioSource _audioSource;
    
    public delegate void AccountHandler(float message);
    public event AccountHandler ActionBitHit; 
    //public event Action ActionBitHit;
    private float nextActionTime;
    private float period;

    private Color currentBitBarColor;
    private Color redColor = Color.red;
    private float previousBit;
    
    void Start()
    {
        currentBitBarColor = bitBar.GetComponent<SpriteRenderer>().color;
        _audioSource = GetComponent<AudioSource>();
        nextActionTime = 0;
        period = (60 / music.BMP) * music.bit;

        _audioSource.clip = music.backgroundSong;
        _audioSource.Play();
    }
    
    void Update()
    {
        
        if (Time.time >= nextActionTime )
        {
            previousBit = nextActionTime;
            nextActionTime += period;
            
            ActionBitHit?.Invoke(previousBit);
            
            bitBar.GetComponent<SpriteRenderer>().color = Color.Lerp(currentBitBarColor, redColor, 0.5f);
            //ChangeBitBarColor(redColor);
            
            //Debug.Log("Bit");
            //Debug.Log(nextActionTime - pre);
            
        }
        
        //Debug.Log(Time.time + " Time time");
        //Debug.Log(pre + 0.03f + " next Time");
        else if (Time.time >= previousBit + 0.3f)
        {
            ChangeBitBarColor(currentBitBarColor);
            //bitBar.GetComponent<SpriteRenderer>().color = Color.Lerp(redColor,currentBitBarColor, Mathf.Abs(Mathf.Sin(Time.time) * 2));
        }

    }

    private void ChangeBitBarColor(Color color)
    {
        bitBar.GetComponent<SpriteRenderer>().color = color;
    }
}
