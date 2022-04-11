using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        //Keep this object even when we go to new scene
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Destroy duplicate gameobjects
        else if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    public void PlaySound(AudioClip _sound)
    { 
        source.PlayOneShot(_sound);
    }    
}
