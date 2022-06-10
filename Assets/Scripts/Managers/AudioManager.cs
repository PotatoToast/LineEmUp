using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip backgroundMusicLooped;

    private AudioSource source;

    void Start(){
        source = gameObject.GetComponent<AudioSource>();
    }

    void Update(){
        if (!source.isPlaying){
            source.clip = backgroundMusicLooped;
            source.Play();
            source.loop = true;
        }
    }
    
}
