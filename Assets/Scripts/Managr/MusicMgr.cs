using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMgr : MonoBehaviour
{
    private float volume;
    private AudioSource audioSource;
    private static MusicMgr instance;
    public static MusicMgr Instance=>instance;
    public const string PLARERPREFS_MUSIC = "PlayerPrefs_Music";
    

    private void Awake()
    {    instance=this;
        audioSource = GetComponent<AudioSource>();
       volume= PlayerPrefs.GetFloat(PLARERPREFS_MUSIC, 1);
    }

    private void Start()
    {
        audioSource.volume = volume;
    }

    private void Update()
    {
         
    }

    public void ChangeVolume(float volume)
    {
        this.volume = volume;
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLARERPREFS_MUSIC, this.volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}