using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;

    [Space(5)]

    [SerializeField] AudioClip s_gameover;
    [SerializeField] AudioClip s_victory;
    [SerializeField] AudioClip s_tick;
    [SerializeField] AudioClip s_positive;
    [SerializeField] AudioClip s_damage;
    
    public void playSound(AudioClip s)
    {
        if(audioSource == null) return;
        audioSource.Stop();
        audioSource.clip = s;
        audioSource.Play();
    }

    public void gameOver()
    {
        playSound(s_gameover);
    }

    public void victory()
    {
        playSound(s_victory);
    }

    public void tick()
    {
        if(audioSource.isPlaying) return; //do not prioritize ticks...
        playSound(s_tick);
    }

    public void positive()
    {
        playSound(s_positive);
    }

    public void damage()
    {
        playSound(s_damage);
    }
}
