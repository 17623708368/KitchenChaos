using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSound : MonoBehaviour
{
    [SerializeField]private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private bool show;
    private float warniSoundTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += stoveCounter_OnProgressChanged;
        stoveCounter.OnStateChanged += stoveCounter_OnStateChanged;
    }
    private void stoveCounter_OnProgressChanged(object sender, StoveCounter.OnProgressChangedArgs e)
    {
        float burnShowProgressAount = 0.5f;
        show = stoveCounter.IsFried()&&e.progressNomel >= burnShowProgressAount;
    
        
       
    }
    private void stoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventAgr e)
    {
        show = false;   
        bool isPlaying = e.state == StoveCounter.State.Fride ||e.state ==  StoveCounter.State.Frying;
        if (isPlaying)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        if (show)
        {
            warniSoundTime -= Time.deltaTime;
            if (warniSoundTime <= 0)
            {
                float warniSoundTimeMaix = .2f;
                warniSoundTime = warniSoundTimeMaix;
                SoundManager.Instance.PlaywarniSound();
            }
        }

    }
}   