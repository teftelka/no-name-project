using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class MusicBit : MonoBehaviour
{
    [SerializeField] private BackgroundMusicSO music;
    private AudioSource _audioSource;
    
    public delegate void AccountHandler(float message);
    public event AccountHandler ActionBitHit; 
    //public event Action ActionBitHit;
    private float nextActionTime;
    private float period;
    
    void Start()
    {
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
            var pre = nextActionTime;
            nextActionTime += period;
            Debug.Log("Bit");
            Debug.Log(nextActionTime - pre);
            ActionBitHit?.Invoke(pre);
        }
    }
}
