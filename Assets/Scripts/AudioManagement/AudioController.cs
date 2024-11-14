using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public enum Sound
    {
        EnemyDied,
        EnemyHit,
        PlayerHit,
        LevelUp,
        Lose
    }
    
    [SerializeField] AudioConfig[] audioConfigs;
    
    public void PlaySound(Sound sound)
    {
        foreach (var audioConfig in audioConfigs)
        {
            if (audioConfig.sound == sound)
            {
                audioConfig.audioSource.Play();
                return;
            }
        }
    }
    
    [Serializable]
    public class AudioConfig
    {
        public Sound sound;
        public AudioSource audioSource;
    }
}
